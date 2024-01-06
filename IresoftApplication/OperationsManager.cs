using IresoftApplication.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IresoftApplication
{
    //Controller - mezivrstva, který propojuje Form(UI) a třídu Operations(Výpočet operací). 
    public class OperationsManager
    {
        private Operations operation;
        public Form1 parent;
        private StringBuilder mainString;
        private CancellationTokenSource cts;

        public OperationsManager(Form1 form)
        {
            this.parent = form;
            this.mainString = new StringBuilder();
            this.operation = new Operations();
            this.cts = new CancellationTokenSource();
            this.operation.ValueChanged += this.parent.uC_ProgressBar.HandleValueChanged;
        }       

        #region Pomocné operace pro načtení souboru
        //Prefore/Finish before/after load file
        //true = before
        //false = after
        private void PrepareFinishLoadFile(bool prepare)
        {
            parent.Cursor = prepare ? Cursors.WaitCursor : Cursors.Default;
            parent.setEnableUI(true);
        }

        private bool TestValidPath(string path)
        {
            if (!File.Exists(path)) return false;
            if (!TryReadFile(path)) return false;
            return true;
        }

        private bool TryReadFile(string path)
        {
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    // Soubor je čitelný
                    return true;
                }
            }
            catch (Exception)
            {
                // Soubor nelze číst
                return false;
            }
        }

        #endregion

        #region Metody pro statistiky
        //Metody pro obsluhu labelů - statistiky
        private void Calculate()
        {
            SetCountSentences();
            SetCountWords();
            SetCountCharacters();
            SetCountLines();
        }

        private void SetCountSentences()
        {
            this.parent.SetCountSentences(Operations.CalculateCountSentences(this.mainString.ToString()).ToString());
        }

        private void SetCountWords()
        {
            this.parent.SetCountWords(Operations.CalculateCountWords(this.mainString.ToString()).ToString());
        }

        private void SetCountCharacters()
        {
            this.parent.SetCountCharacters(Operations.CalculateCountLetter(this.mainString.ToString()).ToString());
        }

        private void SetCountLines()
        {
            this.parent.SetCountLines(Operations.CalculateCountLines(this.mainString.ToString()).ToString());

        }

        private int GetCountLines()
        {
            return this.parent.GetCountLines();

        }

        private int GetCountCharacters()
        {
            return this.parent.GetCountCharacters();
        }

        #endregion

        #region Metody pro obsluhu tlačítek
        internal async void LoadFileAsync(string path)
        {
            if (!this.TestValidPath(path))
            {
                MessageBox.Show("Nevalidní soubor - nelze číst");
                return;
            }

            this.ResetProgressBar();
            Task<string> resultTask = operation.LoadFileAsync(path, cts.Token);
            try
            {
                this.PrepareFinishLoadFile(true);
                string result = await resultTask;
                this.mainString.Clear().Append(result);
                this.PrepareFinishLoadFile(false);
                this.Calculate();
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation canceled.");
            }
        }

        internal async Task SaveStringToFileAsync(string path)
        {

            // Get the selected file name and create a StreamWriter
            this.ResetProgressBar();
            Task resultTask = this.operation.SaveStringToFileAsync(path, mainString, cts.Token);
            try
            {
                await resultTask;
                this.Calculate();
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation canceled.");
            }

        }

        internal async Task RemoveEmptyLinesAsync()
        {
            //KONTROLY
            this.ResetProgressBar();
            Task<string> resultTask = this.operation.RemoveEmptyLinesAsync(this.GetCountLines(), this.mainString.ToString(), cts.Token);
            try
            {
                string result = await resultTask;
                this.mainString.Clear().Append(result);
                this.Calculate();
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation canceled.");
            }

        }

        internal async Task RemoveDiacriticsTextAsync()
        {
            this.ResetProgressBar();
            Task<string> resultTask = this.operation.RemoveDiacritics(this.GetCountCharacters(), this.mainString.ToString(), cts.Token);
            try
            {
                string result = await resultTask;
                this.mainString.Clear().Append(result);
                this.SetCountCharacters();
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation canceled.");
            }
        }



        internal async Task RemoveSpacesAndPunctuationAsync()
        {
            this.ResetProgressBar();
            Task<string> resultTask = this.operation.RemoveSpacesAndPunctuationAsync(this.GetCountCharacters(), this.mainString.ToString(), cts.Token);
            try
            {
                string result = await resultTask;
                this.mainString.Clear().Append(result);
                this.Calculate();
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation canceled.");
            }
        }

        #endregion

        #region ProgressBar
        private void ResetProgressBar()
        {
            this.operation.ResetProgressBar();
        }



        internal void StopProcessing()
        {
            // Zrušte asynchronní načítání souboru, pokud je aktivní
            if (cts != null)
            {
                cts.Cancel();
            }
        }

        #endregion
    }
}
