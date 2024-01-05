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

        // Obslužná metoda pro událost změny hodnoty
        public void HandleValueChanged(int val, bool max)
        {

            if (max) { 
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
            return currentValue.ToString();
        }

        public void setMaxValue(int value)
        {
            if (label_maxV.InvokeRequired)
            {
                label_maxV.Invoke(new Action(() => { label_maxV.Text = value.ToString(); }));
            }
            else
            {
                label_maxV.Text = value.ToString();
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
