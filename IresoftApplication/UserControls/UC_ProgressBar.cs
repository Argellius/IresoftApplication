using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace IresoftApplication.UserControls
{
    public partial class UC_ProgressBar : UserControl
    {
        //Value to display
        private int currentValue = 0;
        private int progressBarValue = 0;
        private int MaxValue = 0;

        private double currentValueCalc = 0;


        //Komunikace s OperationsManagerem
        public delegate void TriggerStopProcessingHandler();
        public event TriggerStopProcessingHandler StopProcessing;

        public UC_ProgressBar()
        {
            InitializeComponent();
        }

        public void TriggerStopProcessing()
        {
            // Zajistění, aby byla událost volána v případě, že je nějaký posluchač připojen
            StopProcessing?.Invoke();
        }

        public async void HandleValueChanged(int val, bool max)
        {
            //Hodnota restartování progressBaru
            if (val == -1 || val == -3)
            {
                currentValue = 0;
                currentValueCalc = 0;
                setCurrentValue(currentValue);
                this.setProgressBarValue(currentValue);
                this.SetVisibility(true);
                return;
            }

            if (val == -3) { 
                this.SetVisibility(false);
            }

            //Nastavení maximální hodnoty
            if (max)
            {
                setMaxValue(val);
                return;
            }

            //Nastavení aktuální hodnoty
            ProcessValue(val);
            this.setProgressBarValue(this.progressBarValue);
            setCurrentValue(this.progressBarValue);

            if (this.progressBarValue == 100)
            {
                await Task.Delay(750);
                this.SetVisibility(false);
            }
        }

        private void SetVisibility(bool hide)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((new Action(() => { this.Visible = hide; }))); ;
            }
            else
                this.Visible = hide;

        }

        private void setCurrentValue(int value)
        {
            if (label_currentV.InvokeRequired)
            {
                label_currentV.Invoke(new Action(() => { label_currentV.Text = value.ToString(); }));
            }
            else
            {
                label_currentV.Text = value.ToString();
            }
        }

        private void ProcessValue(int value)
        {
            this.currentValueCalc += value;

            this.currentValue = Convert.ToInt16(Math.Round((currentValueCalc / MaxValue) * 100));
            this.progressBarValue = currentValue;
        }

        private void setProgressBarValue(int progressBarValue)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action(() => { progressBar.Value = progressBarValue; }));
            }
            else
            {
                progressBar.Value = progressBarValue;
            }
        }

        public void setMaxValue(int value)
        {
            this.MaxValue = value;
            value = 100;

            if (label_maxV.InvokeRequired)
            {
                label_maxV.Invoke(new Action(() => { label_maxV.Text = value.ToString() + "%"; }));
            }
            else
            {
                label_maxV.Text = value.ToString() + "%";
            }
        }

        private void button_storno_Click(object sender, EventArgs e)
        {

            TriggerStopProcessing();

        }
    }
}
