using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Forms.DataVisualization.Charting;

namespace JeeAWattVis
{
    public partial class frmMain : Form
    {
        private delegate void GraphDataHandler(int volt, int amp);
        private delegate void ValueHandler(StringDictionary vals);

        private Thread _serialThread;
        private List<int> _dataVolt = new List<int>();
        private List<int> _dataAmp = new List<int>();
        private int _baseAmp = 0;
        private int _baseVolt = 0;

        private void Serial_Line(string s)
        {
            if (s.Equals("V,A"))
                this.Invoke(new Action(GraphResetData));
            else if (s.Equals("."))
                this.Invoke(new Action(GraphDataDone));
            else
            {
                string[] data = s.Split(',');
                if (data.Length > 1)
                {
                    int v = 0, a = 0;
                    if (Int32.TryParse(data[0], out v) && Int32.TryParse(data[1], out a))
                        this.Invoke(new GraphDataHandler(GraphData), new object[] { v, a });
                }
                else
                {
                    data = s.Split(' ');
                    if (data.Length > 0)
                    {
                        StringDictionary vals = new StringDictionary();
                        foreach (var val in data)
                        {
                            string[] parts = val.Split('=');
                            if (parts.Length > 1)
                                vals.Add(parts[0], parts[1]);
                        }
                        if (vals.Count > 0)
                            this.Invoke(new ValueHandler(OnValues), new object[] { vals });
                    }
                }
            }
        }

        private void Serial_Execute()
        {
            try
            {
                while (serial.IsOpen)
                {
                    try
                    {
                        string s = serial.ReadLine();
                        if (String.IsNullOrWhiteSpace(s))
                            continue;
                        Serial_Line(s);
                    }
                    catch (TimeoutException ex)
                    { }
                } /* while open */
            }
            catch (IOException e)
            { 
            }
        }

        private void OnValues(StringDictionary vals)
        {
            foreach (DictionaryEntry item in vals)
            {
                if ((string)item.Key == "ampbase")
                {
                    lblAmpBase.Text = "Amp Base: " + item.Value;
                    _baseAmp = Int32.Parse((string)item.Value);
                    Properties.Settings.Default.AmpBase = _baseAmp;
                }
                else if ((string)item.Key == "voltbase")
                {
                    lblVoltBase.Text = "Volt Base: " + item.Value;
                    _baseVolt = Int32.Parse((string)item.Value);
                    Properties.Settings.Default.VoltBase = _baseVolt;
                }
                else if ((string)item.Key == "cnt")
                    lblCount.Text = "Count: " + item.Value;
                else if ((string)item.Key == "a")
                {
                    lblCurrent.Text = item.Value + " amps";
                    while (chtVis.Series[1].Points.Count > 60)
                        chtVis.Series[1].Points.RemoveAt(0);
                    chtVis.Series[1].Points.Add(Double.Parse((string)item.Value));
                }
                else if ((string)item.Key == "pf")
                {
                    lblPowerFactor.Text = (string)item.Value + " PF";
                }
                else if ((string)item.Key == "v")
                {
                    lblVolts.Text = (string)item.Value + " volts";
                }
                else if ((string)item.Key == "w")
                    lblWatts.Text = item.Value + " watts";
            }
        }

        private void GraphResetData()
        {
            _dataVolt.Clear();
            _dataAmp.Clear();
        }

        private void GraphDataDone()
        {
            chtVis.Series[0].Points.DataBindY(_dataVolt);
            chtVis.Series[1].Points.DataBindY(_dataAmp);

            if (_baseAmp != 0)
            {
                int ampTotal = 0;
                foreach (int amp in _dataAmp)
                {
                    ampTotal += Math.Abs(amp - _baseAmp);
                }
                double ea = (double)ampTotal / _dataAmp.Count;
                lblAmpTotal.Text = String.Format("Amp total: {0}\n({1:f2}ea) ", 
                    ampTotal, ea);

                double f = 0.0;
                if (Double.TryParse(txtAmpFactor.Text, out f) && f != 0.0)
                {
                    f = ea * f;
                    lblCurrent.Text = String.Format("{0:f3} amps", f);
                }
            }  /* if baseamp */
        }

        private void GraphData(int volt, int amp)
        {
            _dataVolt.Add(volt);
            _dataAmp.Add(amp);
        }

        public frmMain()
        {
            InitializeComponent();
            serial.ReadTimeout = 500;
            serial.NewLine = "\n";
             _serialThread = new Thread(Serial_Execute);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            serial.Open();
            _serialThread.Start();
            serial.WriteLine("");

            _baseVolt = Properties.Settings.Default.VoltBase;
            lblVoltBase.Text = "Volt Base: " + _baseVolt;
            _baseAmp = Properties.Settings.Default.AmpBase;
            lblAmpBase.Text = "Amp Base: " + _baseAmp;

            cbxMode.SelectedIndex = Properties.Settings.Default.Mode;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
            if (serial.IsOpen)
            {
                serial.Close();
                _serialThread.Join();
            }
        }

        private void btnBase_Click(object sender, EventArgs e)
        {
            SerialCommand("base");
        }

        private void SerialCommand(string p)
        {
            serial.WriteLine("/" + p);
        }

        private void chkShowLabels_CheckStateChanged(object sender, EventArgs e)
        {
            foreach (var srs in chtVis.Series)
                srs.IsValueShownAsLabel = chkShowLabels.Checked;
        }

        private void btnBaseSingle_Click(object sender, EventArgs e)
        {
            SerialCommand("basesingle");
        }

        private void cbxMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Mode = cbxMode.SelectedIndex;
            SerialCommand("dump=" + cbxMode.SelectedIndex);
            switch (cbxMode.SelectedIndex)
            {
                case 0:
                    lblAmpTotal.Text = "-";
                    break;
                case 1:
                    lblWatts.Text = "- watts";
                    break;
            }
            foreach (var srs in chtVis.Series)
                srs.Points.Clear();
        }

        private void txtAmpFactor_Leave(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            double dummy;
            if (Double.TryParse(txt.Text, out dummy))
                SerialCommand(txt.Tag + txt.Text);
        }

        private void txtAmpFactor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                txtAmpFactor_Leave(sender, null);
        }
    }
}
