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

        private void UC_ProgressBar_Load(object sender, EventArgs e)
        {

        }


        public void HandleValueChanged(int val, bool max)
        {
            if (val == -1)
            {
                currentValue = 0;
                currentValueCalc = 0;
                setCurrentValue(currentValue);
                this.setProgressBarValue(currentValue);
                this.SetVisibility(false);
                return;
            }

            if (max)
            {
                setMaxValue(val);
                return;
            }

            ProcessValue(val);
            this.setProgressBarValue(this.progressBarValue);
            setCurrentValue(this.progressBarValue);

            if (this.progressBarValue == 100)
                this.SetVisibility(true);
        }

        private async void SetVisibility(bool hide)
        {
            if (this.Visible == false)
                this.Visible = true;

            if (hide)
            {
                await Task.Delay(1000);
                if (this.InvokeRequired)
                {
                    this.Invoke((new Action(() => { this.Visible = false; }))); ;
                }
                else
                    this.Visible = false;
            }
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
