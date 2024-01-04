using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace IresoftApplication
{
    public partial class Form1 : Form
    {
        private OperationsManager operationsManager;


        public object CilovaCesta { get; private set; }

        public Form1()
        {
            InitializeComponent();
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Naètení souboru";
            openFileDialog.FileName = String.Empty;
            saveFileDialog.Filter = "Textové soubory (*.txt)|*.txt|CSV soubory (*.csv)|*.csv|Všechny soubory (*.*)|*.*";

        }


        private void UserControl_SignalReceived(object sender, EventArgs e)
        {

        }

        private void button_load_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.operationsManager.StartLoadFile(openFileDialog.FileName.ToString());
            }
        }


        public void setEnableUI(bool v)
        {
            this.button_blank_line.Enabled = v;
            this.button_load.Enabled = v;
            this.button_save.Enabled = v;
            this.button_copy.Enabled = v;
            this.button_diacritic.Enabled = v;
            this.button_blank_line.Enabled = v;
            this.button_white_punc.Enabled = v;
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.CilovaCesta = saveFileDialog.FileName;

            }
        }


        //OPERATION BUTTONS

        private void button_diacritic_Click(object sender, EventArgs e)
        {
            this.operationsManager.DeleteDiacticInText();
        }

        private void button_copy_Click(object sender, EventArgs e)
        {
            this.operationsManager.CopyOperatin();
        }

        private void button_blank_lines_Click(object sender, EventArgs e)
        {
            this.operationsManager.DeleteBlackLines();

        }

        private void button_white_punc_Click(object sender, EventArgs e)
        {
            this.operationsManager.DeleteWhiteLinesPuncChars();
        }

        internal void StopProcess()
        {
            this.operationsManager.StopProcessingFile();
        }

        internal void sendValueToProgressBar(int value, bool maxValue)
        {
            if (uC_ProgressBar1.Visible == false) { uC_ProgressBar1.Visible = true; }

            if (maxValue)
                uC_ProgressBar1.setMaxValue(value.ToString());

            uC_ProgressBar1.setCurrentValue(value.ToString());
        }

        internal void SetProgressBar(CancellationToken cts)
        {

        }

        internal void ShowProgressBar(bool v)
        {
            uC_ProgressBar1.Visible = v;
        }
    }
}
