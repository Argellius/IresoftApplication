using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IresoftApplication.UserControls
{
    public partial class UC_ProgressBar : UserControl
    {

        private Form1? parent;

        // Define a delegate that matches the signature of the event
        public delegate void SignalEventHandler(int value);

        // Define an event using the delegate
        public event SignalEventHandler SignalEvent;


        public UC_ProgressBar()
        {
            InitializeComponent();
            parent = this.Parent as Form1 ?? throw new Exception();

        }

        private void UC_ProgressBar_Load(object sender, EventArgs e)
        {

        }

        public void setCurrentValue(string value)
        {            
            this.label_currentV.Text = value;
        }


        public void setMaxValue(string value)
        {
            this.label_maxV.Text = value + " %";
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
