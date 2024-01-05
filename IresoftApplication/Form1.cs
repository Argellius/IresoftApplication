using IresoftApplication.UserControls;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace IresoftApplication
{
    public partial class Form1 : Form
    {
        private OperationsManager operationManager;

        public object CilovaCesta { get; private set; }

        public Form1()
        {
            InitializeComponent();     
            this.operationManager = new OperationsManager(this);
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Naètení souboru";
            openFileDialog.FileName = String.Empty;
            saveFileDialog.Filter = "Textové soubory (*.txt)|*.txt|CSV soubory (*.csv)|*.csv|Všechny soubory (*.*)|*.*";
            // Pøidání obslužné metody k události v objektu Operation
            operationManager.ValueChanged += uC_ProgressBar.HandleValueChanged;
        }

        private void UserControl_SignalReceived(object sender, EventArgs e)
        {

        }

        private void button_load_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.operationManager.StartLoadFile(openFileDialog.FileName.ToString());
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
            this.operationManager.DeleteDiacticInText();
        }

        private void button_copy_Click(object sender, EventArgs e)
        {
            this.operationManager.CopyOperation(this.saveFileDialog.FileName);
        }

        private void button_blank_lines_Click(object sender, EventArgs e)
        {
            this.operationManager.DeleteBlankLines();

        }

        private void button_white_punc_Click(object sender, EventArgs e)
        {
            this.operationManager.DeleteWhiteLinesPuncChars();
        }

        internal void StopProcess()
        {
            this.operationManager.StopProcessingFile();
        }

        internal void SetProgressBar(CancellationToken cts)
        {

        }

        internal void ShowProgressBar(bool v)
        {
            uC_ProgressBar.Visible = v;
        }

        internal void setPocetVet(string v)
        {
            this.textBox_pocet_vet.Text = v;
        }

        internal void setPocetSlov(string v)
        {
            this.textBox_pocet_slov.Text = v;
        }

        internal void setPocetZnaku(string v)
        {
            this.textBox_pocet_znaku.Text = v;
        }

        internal void setPocetRadku(string v)
        {
            this.textBox_pocet_radku.Text = v;
        }

        internal int getPocetRadku()
        {
            return Convert.ToInt32(this.label_pocet_radku.Text);
        }

        internal int getPocetZnaku()
        {
            return Convert.ToInt32(this.label_pocet_znaku.Text);
        }
    }
}
