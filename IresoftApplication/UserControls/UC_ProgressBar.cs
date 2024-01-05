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

        private Form1? parent;
        private int currentValue = 0;

        private int MaxValue = 0;

        public UC_ProgressBar()
        {
            InitializeComponent();
        }

        public void setParent(Form1? form)
        {
            parent = form as Form1 ?? throw new Exception();

        }

        private void UC_ProgressBar_Load(object sender, EventArgs e)
        {

        }


        public void HandleValueChanged(int val, bool max)
        {
            if (val == -1) { currentValue = 0; return; }

            if (max)
            {
                setMaxValue(val);
                return;
            }

            var value = ProcessValue(val);

            if (label_currentV.InvokeRequired)
            {
                label_currentV.Invoke(new Action(() => { label_currentV.Text = value.ToString(); }));
            }
            else
            {
                label_currentV.Text = value.ToString();
            }
        }

        private string ProcessValue(int value)
        {
            this.currentValue = currentValue + value;
            var progressBarValue = Convert.ToInt16(Math.Round((double)currentValue / MaxValue * 100));
            this.setProgressBarValue(progressBarValue);

            return progressBarValue.ToString();
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
            // Zrušte asynchronní načítání souboru, pokud je aktivní
            if (parent != null)
            {
                parent.StopProcess();
            }
        }
    }
}
