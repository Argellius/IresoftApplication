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
        private void PrepareBeforeMethod(bool prepare)
        {
            parent.Cursor = prepare ? Cursors.WaitCursor : Cursors.Default;
            parent.setEnableUI(!prepare);
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


        private void PrepareForStartMethod()
        {
            this.ResetProgressBar();
            this.PrepareBeforeMethod(true);
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

            this.PrepareForStartMethod();

            try
            {
                this.PrepareBeforeMethod(true);
                string result = await Task.Run(() => operation.LoadFileAsync(path, cts.Token));
                this.mainString.Clear().Append(result);                
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation canceled.");
            }
            finally
            {
                this.PrepareBeforeMethod(false);
                this.Calculate();
            }
        }

        internal async Task SaveStringToFileAsync(string path)
        {

            this.PrepareForStartMethod(); 

            try
            {
                await Task.Run(() => this.operation.SaveStringToFileAsync(path, mainString, cts.Token));
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation canceled.");
            }
            finally
            {
                this.PrepareBeforeMethod(false);
                this.Calculate();
            }

        }

        internal async Task RemoveEmptyLinesAsync()
        {
            this.PrepareForStartMethod();
            try
            {
                string result = await Task.Run(() => this.operation.RemoveEmptyLinesAsync(this.GetCountLines(), this.mainString.ToString(), cts.Token));
                this.mainString.Clear().Append(result);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation canceled.");
            }
            finally
            {
                this.PrepareBeforeMethod(false);
                this.Calculate();
            }

        }

        internal async Task RemoveDiacriticsTextAsync()
        {
            this.PrepareForStartMethod();

            try
            {
                string result = await Task.Run(() => this.operation.RemoveDiacritics(this.GetCountCharacters(), this.mainString.ToString(), cts.Token));
                this.mainString.Clear().Append(result);
                this.SetCountCharacters();
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation canceled.");
            }
            finally
            {
                this.PrepareBeforeMethod(false);
                this.Calculate();
            }
        }



        internal async Task RemoveSpacesAndPunctuationAsync()
        {
            this.PrepareForStartMethod();

            try
            {
                string result = await Task.Run(() => this.operation.RemoveSpacesAndPunctuationAsync(this.GetCountCharacters(), this.mainString.ToString(), cts.Token));
                this.mainString.Clear().Append(result);
                this.Calculate();
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation canceled.");
            }
            finally
            {
                this.PrepareBeforeMethod(false);
                this.Calculate();
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
