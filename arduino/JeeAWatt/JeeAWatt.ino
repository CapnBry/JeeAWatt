#include <Ports.h>
#include <RF12.h>
#include <SerialCmd.h>
#include <inttypes.h>
#include <avr/wdt.h>;
#include <avr/sleep.h>;

#define CONF_USE_PHASE 1
#define CONF_SERIAL    1

#define APIN_CURRENT  0
#define APIN_VOLTAGE  3

static float g_VoltFactor = 0.64f;
static float g_AmpFactor = 0.056; // 0.0315f;
#if CONF_USE_PHASE
static float g_PhaseFactor = 0.00067f;
#endif

static int16_t g_VoltBaseline;
static int16_t g_AmpBaseline;
static uint8_t g_Dump;

#define RF12NODE(x) (x - 'A' + 1)

#ifndef M_SQRT1_2
#define M_SQRT1_2 7.0710678118654752440E-1
#endif

#ifndef M_SQRT2
#define M_SQRT2 1.4142135623730950488016887 
#endif

extern SerialCmdHandler _serialcmds[];

ISR(WDT_vect) { Sleepy::watchdogEvent(); }

static void Serial_nl(void)
{
  Serial.write('\n');
  Serial.flush(); 
}

static volatile uint8_t _adcBusy;
ISR(ADC_vect) { _adcBusy = 0; }

static unsigned int analogReadSleep(unsigned char pin)
{
  set_sleep_mode(SLEEP_MODE_IDLE);
  ADMUX = (DEFAULT << 6) | pin;
  bitSet(ADCSRA, ADIE);
  _adcBusy = 1;
  while (_adcBusy)
    sleep_mode();
  bitClear(ADCSRA, ADIE);
  return ADC;
}

#define WHICH_VOLT 0
#define WHICH_AMP  1

static void readCycles(
  void (*cycleCallback)(uint8_t which, uint16_t val, void *priv), 
  void *priv,
  uint8_t cycleCnt = 1)
{
  enum CycleState { csPositive, csNegative, csDone } cycleState;
  uint16_t val;
  uint8_t cnt = 0;

  // Wait for the voltage to zero cross to negative
  while (analogReadSleep(APIN_VOLTAGE) > g_VoltBaseline);
  // Wait at least 5 samples and for it to go positive again
  while ((analogReadSleep(APIN_VOLTAGE) < g_VoltBaseline) || (cnt < 5))
    ++cnt;

  bitSet(ADCSRA, ADIE);
  set_sleep_mode(SLEEP_MODE_ADC);
  // this should grab the first = or > volt
  bitSet(ADCSRA, ADSC);
  _adcBusy = 1;

  while (cycleCnt--)
  {
    cycleState = csPositive; 
    cnt = 0;
    do {
      ++cnt;
      // Wait for volt reading
      while (_adcBusy)
        sleep_mode();
      val = ADC;

      // Start next amp reading
      ADMUX = (DEFAULT << 6) | APIN_CURRENT;
      bitSet(ADCSRA, ADSC);
      _adcBusy = 1;

      // Do some work while waiting for ADC completion
      cycleCallback(WHICH_VOLT, val, priv);
      switch (cycleState)
      {
        case csPositive:
          if (cnt > 5 && val < g_VoltBaseline)
          {
            cycleState = csNegative;
            cnt = 0;
          }
          break;
        case csNegative:
          if (cnt > 5 && val > g_VoltBaseline)
            cycleState = csDone;
          break;
      } /* switch (cycleState) */

      // Wait for amp reading
      while (_adcBusy)
        sleep_mode();
      val = ADC;

      // Start next reading 
      ADMUX = (DEFAULT << 6) | APIN_VOLTAGE;
      bitSet(ADCSRA, ADSC);
      _adcBusy = 1;

      // Do some work while waiting for ADC completion
      cycleCallback(WHICH_AMP, val, priv);
    } while (cycleState != csDone);
  }  /* while cycle cnt */

  while (_adcBusy)
    sleep_mode();
  bitClear(ADCSRA, ADIE);
}

struct watts_priv
{
  uint16_t cnt;
  int16_t lastSampleV, lastSampleI;
  float filteredV, filteredI;
  float sumV, sumI, sumP;
  float phaseShiftedV;
  // results
  float Vrms, Irms;
  float realP, apparentP;
  float pf;
};

static void wattsRead_cb(uint8_t which, uint16_t val, void *priv)
{
  /******
     Calculuations copied from emonLib - Library for openenergymonitor
     GNU GPL
     https://github.com/openenergymonitor/EmonLib
  ******/
  struct watts_priv *data = (struct watts_priv *)priv;
  int ival = val;

  if (which == WHICH_VOLT)
  {
    float lastFilteredV = data->filteredV;
    data->filteredV = 0.996 * (ival - data->lastSampleV + lastFilteredV);
    data->lastSampleV = ival;
    data->sumV += (data->filteredV * data->filteredV);
#if CONF_USE_PHASE
    data->phaseShiftedV = lastFilteredV + g_PhaseFactor * (data->filteredV - lastFilteredV);
#endif
  }
  else 
  {
    float lastFilteredI = data->filteredI;
    data->filteredI = 0.996 * (ival - data->lastSampleI + lastFilteredI);
    data->lastSampleI = ival;
    data->sumI += (data->filteredI * data->filteredI);
#if CONF_USE_PHASE
    data->sumP += data->phaseShiftedV * data->filteredI;
#else
    data->sumP += data->filteredV * data->filteredI;
#endif

    ++data->cnt;
  }
}

static void measureForCycles(struct watts_priv *data, uint8_t cycles)
{
  /*
  static struct watts_priv mydata;
  mydata.cnt = 0;
  mydata.sumI = 0;
  mydata.sumV = 0;
  mydata.sumP = 0;

  readCycles(&wattsRead_cb, &mydata, cycles);
  *data = mydata;
  */

  memset(data, 0, sizeof(struct watts_priv));
  data->lastSampleV = g_VoltBaseline;
  data->lastSampleI = g_AmpBaseline;

  readCycles(&wattsRead_cb, data, cycles);

  data->Vrms = sqrt(data->sumV / data->cnt);
  data->Irms = sqrt(data->sumI / data->cnt);
  data->realP = data->sumP / data->cnt;
  data->apparentP = data->Vrms * data->Irms;
  data->pf = data->realP / data->apparentP;

}

static void measureWatts(void)
{
  struct watts_priv data;
  measureForCycles(&data, 10);

  Serial.print("cnt="); Serial.print(data.cnt, DEC);
  Serial.print(" v="); Serial.print(g_VoltFactor * data.Vrms, 1);
  Serial.print(" a="); Serial.print(g_AmpFactor * data.Irms / data.pf, 2);
  Serial.print(" w="); Serial.print(g_VoltFactor * g_AmpFactor * data.realP, 2);
  Serial.print(" va="); Serial.print(g_VoltFactor * g_AmpFactor * data.apparentP, 2);
  Serial.print(" pf="); Serial.print(data.pf, 2);
  Serial_nl();
}

static void calibrateV(int16_t expectedRmsVoltsX10)
{
  struct watts_priv data;
  measureForCycles(&data, 30);
  
  g_VoltFactor = expectedRmsVoltsX10 / 10.0f / data.Vrms;

  Serial.print("cnt="); Serial.print(data.cnt);
  Serial.print(" voltFactor="); Serial.print(g_VoltFactor, 5);
  Serial_nl();
}

static void calibrateA(int16_t expectedAmpsX100)
{
  struct watts_priv data;
  measureForCycles(&data, 30);

  // Amp factor is for 1.0 Power Factor
  g_AmpFactor = expectedAmpsX100 / 100.0f / data.Irms * data.pf;

  Serial.print("cnt="); Serial.print(data.cnt, DEC);
  Serial.print(" ampFactor="); Serial.print(g_AmpFactor, 5);
  Serial_nl();
}

struct singlebase_priv
{
    uint16_t v[100];
    uint16_t a[100];
    uint8_t cnt;
};

static void singleBase_cb(uint8_t which, uint16_t val, void *priv)
{
  struct singlebase_priv *data = (struct singlebase_priv *)priv;
  uint8_t cnt = data->cnt;
  if (which == WHICH_VOLT)
    data->v[cnt] = val;
  else
  {
    data->a[cnt] = val;
    data->cnt = cnt + 1;
  }
}

static void calibrateBaseSingle(char *param)
{
  struct singlebase_priv data;
  data.cnt = 0;
  
  readCycles(&singleBase_cb, &data, 1);
    
  uint32_t voltAvg = 0;
  uint32_t ampAvg = 0;
  for (uint8_t i=0; i<data.cnt; ++i)
  {
    voltAvg += data.v[i];
    ampAvg += data.a[i];
  }

  Serial.print("cnt="); Serial.print(data.cnt, DEC);
  if (data.cnt > 0)
  {
    g_VoltBaseline = voltAvg / data.cnt;
    g_AmpBaseline = ampAvg / data.cnt;
    Serial.print(" voltBase="); Serial.print(g_VoltBaseline, DEC);
    Serial.print(" ampBase="); Serial.print(g_AmpBaseline, DEC);
  }
  Serial_nl();

  if (g_Dump != 0)
  {
    Serial.print("V,A\n");
    for (uint8_t i=0; i<data.cnt; ++i)
    {
      Serial.print(data.v[i]); Serial.print(',');
      Serial.print(data.a[i]); Serial_nl();
    }
    Serial.print('.');
    Serial_nl();
  }
}

static void stabilizeAdc(void)
{
  uint16_t last = 0;
  uint16_t totalCnt = 0;
  uint16_t sameCnt = 0;
  uint16_t curr;

  do {
    // Set to read Vbg using VREF=AVCC
    curr = analogReadSleep(0b1110);
    if (last == curr)
      ++sameCnt;
    else
      sameCnt = 0;
    last = curr;
    ++totalCnt;
  } while ((totalCnt < 64) && (sameCnt < 3));
  //Serial.print("ADC stabilized in "); Serial.print(totalCnt, DEC); Serial_nl();
}

static void cmdReboot(char *param)
{
  wdt_enable(WDTO_15MS);
}

static void cmdDump(char *param)
{
  g_Dump = atoi(param);
}

static void cmdAmpFactor(char *param)
{
  g_AmpFactor = atof(param);
}

static void cmdVoltFactor(char *param)
{
  g_VoltFactor = atof(param);
}

static void cmdPhaseFactor(char *param)
{
  g_PhaseFactor = atof(param);
}

static void cmdCalibrateA(char *param)
{
  calibrateA(atoi(param));
}

static void cmdCalibrateV(char *param)
{
  calibrateV(atoi(param));
}

void setup() 
{
  PRR = bit(PRTWI) | bit(PRTIM2) | bit(PRTIM1) | bit(PRUSART0); //bit(PRSPI)
#if CONF_SERIAL
  bitClear(PRR, PRUSART0);
  Serial.begin(115200);
  Serial_nl();
  serialcmd_begin(_serialcmds);
#endif

  //rf12_initialize(3, RF12_915MHZ);

  stabilizeAdc();
  g_VoltBaseline = 512;
  g_AmpBaseline = 512;
  calibrateBaseSingle(NULL);

  //calibrateV(1210);
  //calibrateA(45);
}

void loop() 
{
  if (g_Dump != 0)
    calibrateBaseSingle(NULL);
  else
    measureWatts();

#if CONF_SERIAL
  delay(2000);
  serialcmd_update();
#else
  Sleepy::loseSomeTime(10000);
  stabilizeAdc();
#endif
}


static prog_char CMD_BASESINGLE[] PROGMEM = "basesingle";
static prog_char CMD_REBOOT[] PROGMEM = "reboot";
static prog_char CMD_DUMP[] PROGMEM = "dump=";
static prog_char CMD_AMPFACTOR[] PROGMEM = "ampFactor=";
static prog_char CMD_VOLTFACTOR[] PROGMEM = "voltFactor=";
static prog_char CMD_PHASEFACTOR[] PROGMEM = "phaseFactor=";
static prog_char CMD_CALIBRATEA[] PROGMEM = "ampCal=";
static prog_char CMD_CALIBRATEV[] PROGMEM = "voltCal=";

SerialCmdHandler _serialcmds[] = 
{
  { CMD_BASESINGLE, &calibrateBaseSingle },
  { CMD_REBOOT, &cmdReboot },
  { CMD_DUMP, &cmdDump },
  { CMD_AMPFACTOR, &cmdAmpFactor },
  { CMD_VOLTFACTOR, &cmdVoltFactor },
  { CMD_PHASEFACTOR, &cmdPhaseFactor },
  { CMD_CALIBRATEA, &cmdCalibrateA },
  { CMD_CALIBRATEV, &cmdCalibrateV },
};