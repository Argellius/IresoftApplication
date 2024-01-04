using System.Globalization;
using System.IO;
using System.Text;

namespace IresoftApplication
{
    public partial class Form1 : Form
    {
        private OpenFileDialog opd;
        private string path;
        private string mainString;
        private CancellationTokenSource cts;

        public object CilovaCesta { get; private set; }

        public Form1()
        {
            InitializeComponent();
            cts = new CancellationTokenSource();
            mainString = "";
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Naètení souboru";
            openFileDialog.FileName = String.Empty;
            saveFileDialog.Filter = "Textové soubory (*.txt)|*.txt|CSV soubory (*.csv)|*.csv|Všechny soubory (*.*)|*.*";

        }

        private void button_load_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog.FileName.ToString();
                _ = Start();
            }


        }

        private async Task Start()
        {
            if (File.Exists(path))
            {
                await LoadFileAsync();
                Calculate();
            }
            else
            {
                MessageBox.Show("Soubor neexistuje.");
            }
        }

        private async Task LoadFileAsync()
        {
            mainString = "";
            Char[] buffer;

            try
            {
                Cursor = Cursors.WaitCursor;
                setEnableUI(false);

                //Reader
                using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
                {
                    buffer = new Char[(int)reader.BaseStream.Length];

                    // Asynchronnì naète obsah souboru
                    await reader.ReadAsync(buffer, cts.Token);
                }
            }
            catch (Exception ex)
            {
                // Zde mùžete zpracovat chyby, které mohou nastat pøi ètení souboru
                Console.WriteLine("Chyba pøi naèítání souboru: " + ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
                setEnableUI(true);

            }
        }

        private void Calculate()
        {
            textBox_pocet_vet.Text = CalculateCountSentences().ToString();
            textBox_pocet_slov.Text = CalculateCountWords().ToString();
            textBox_pocet_znaku.Text = CalculateCountLetter().ToString();
            textBox_pocet_radku.Text = CalculateCountRadek().ToString();
        }


        private void setEnableUI(bool v)
        {
            this.button_blank_line.Enabled = v;
            this.button_load.Enabled = v;
            this.button_save.Enabled = v;
            this.button_copy.Enabled = v;
            this.button_diacritic.Enabled = v;
            this.button_blank_line.Enabled = v;
            this.button_white_punc.Enabled = v;


        }

        private int CalculateCountSentences()
        {
            if (string.IsNullOrEmpty(mainString)) return 0;

            // Znak interpunkce použitý k oddìlení vìt
            char[] sentenceSeparators = { '.', ',', '!', '?', ':' };

            // Získání poètu vìt pomocí znaku interpunkce
            int sentenceCount = mainString.Count(c => sentenceSeparators.Contains(c));

            return sentenceCount;
        }

        private int CalculateCountWords()
        {
            if (string.IsNullOrEmpty(mainString)) return 0;

            // Získání poètu vìt pomocí znaku interpunkce
            int Count = mainString.Count(c => c == ' ');

            return Count;
        }

        private int CalculateCountLetter()
        {
            return mainString.ToCharArray().Count();
        }

        private int CalculateCountRadek()
        {
            if (string.IsNullOrEmpty(mainString)) return 0;

            int radek = mainString.Split('\n').Length;

            return radek;
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
            OdstranDiakritiku();
        }

        private void button_copy_Click(object sender, EventArgs e)
        {
            if (!saveFileDialog.CheckFileExists)
            {
                MessageBox.Show("File does not exist");
                return;
            }

            if (!saveFileDialog.CheckPathExists)
            {
                MessageBox.Show("Path of file does not exist");
                return;
            }

            // Get the selected file name and create a StreamWriter
            string fileName = saveFileDialog.FileName;

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                // Write the content of StringBuilder to the file
                sw.Write(mainString.ToString());
            }
        }

        private void button_blank_lines_Click(object sender, EventArgs e)
        {
            OdstranPrazdneRadky();
            Calculate();
        }

        private void button_white_punc_Click(object sender, EventArgs e)
        {
            OdstranMezeryInterpunkcniZnamenka();
        }


        //METHODS FOR OPERATION B.

        private void OdstranDiakritiku()
        {
            if (string.IsNullOrEmpty(mainString.ToString()))
                return;

            // Pøevedení øetìzce na Normalization Form D (NFD)
            string normalizedString = mainString.ToString().Normalize(NormalizationForm.FormD);

            // Odstranìní diakritiky (znakù s diakritikou)
            StringBuilder result = new StringBuilder();

            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    result.Append(c);
            }

            mainString = String.Empty;
            mainString = result.ToString();
        }



        private void OdstranPrazdneRadky()
        {
            var lines = mainString.ToString().Replace("\r", "").Split('\n')
                            .Where(x => !string.IsNullOrEmpty(x))
                            .ToArray();

            mainString = String.Empty;
            mainString = string.Join("\n", lines);
        }

        private void OdstranMezeryInterpunkcniZnamenka()
        {
            StringBuilder sb_temp = new StringBuilder();

            foreach (char ch in mainString.ToString())
            {
                if (!char.IsWhiteSpace(ch) && !char.IsPunctuation(ch))
                {
                    sb_temp.Append(ch);
                }
            }

            mainString = String.Empty;
            mainString = sb_temp.ToString();

        }

        internal void StopProcess()
        {
            // Zrušte asynchronní naèítání souboru, pokud je aktivní
            if (cts != null)
            {
                cts.Cancel();
            }
        }
    }
}
