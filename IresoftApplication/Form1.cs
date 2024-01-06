using IresoftApplication.UserControls;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace IresoftApplication
{
    //UI
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
            this.uC_ProgressBar.StopProcessing += this.operationManager.StopProcessing;
        }

        private void UserControl_SignalReceived(object sender, EventArgs e)
        {

        }

        private void button_load_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.operationManager.LoadFileAsync(openFileDialog.FileName.ToString());
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
        private async void button_diacritic_Click(object sender, EventArgs e)
        {
            await this.operationManager.RemoveDiacriticsTextAsync();
        }

        private async void button_copy_Click(object sender, EventArgs e)
        {
            await this.operationManager.SaveStringToFileAsync(this.saveFileDialog.FileName);
        }

        private async void button_blank_lines_Click(object sender, EventArgs e)
        {
            await this.operationManager.RemoveEmptyLinesAsync();

        }

        private async void button_white_punc_Click(object sender, EventArgs e)
        {
            await this.operationManager.RemoveSpacesAndPunctuationAsync();
        }

        internal void StopProcess()
        {
            this.operationManager.StopProcessing();
        }

        internal void SetProgressBar(CancellationToken cts)
        {

        }

        internal void ShowProgressBar(bool v)
        {
            uC_ProgressBar.Visible = v;
        }

        internal void SetCountSentences(string v)
        {
            this.textBox_pocet_vet.Text = v;
        }

        internal void SetCountWords(string v)
        {
            this.textBox_pocet_slov.Text = v;
        }

        internal void SetCountCharacters(string v)
        {
            this.textBox_pocet_znaku.Text = v;
        }

        internal void SetCountLines(string v)
        {
            this.textBox_pocet_radku.Text = v;
        }

        internal int GetCountLines()
        {
            return Convert.ToInt32(this.textBox_pocet_radku.Text);
        }

        internal int GetCountCharacters()
        {
            return Convert.ToInt32(this.textBox_pocet_znaku.Text);
        }

        private void uC_ProgressBar_Load(object sender, EventArgs e)
        {

        }
    }
}
