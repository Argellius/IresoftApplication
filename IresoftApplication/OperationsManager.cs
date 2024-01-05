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

        internal async void StartLoadFile(string path)
        {
            if (!this.TestValidPath(path))
                return;

            Task<string> resultTask = operation.LoadFileAsync(path, cts.Token);
            try
            {
                this.ResetProgressBar();
                this.PrepareFinishLoadFile(true);
                this.ResetProgressBar();
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

        private void Calculate()
        {
            setPocetVet();
            setPocetSlov();
            setPocetZnaku();
            setPocetRadku();
        }

        private void setPocetVet()
        {
            this.parent.setPocetVet(Operations.CalculateCountSentences(this.mainString.ToString()).ToString());
        }

        private void setPocetSlov()
        {
            this.parent.setPocetSlov(Operations.CalculateCountWords(this.mainString.ToString()).ToString());
        }

        private void setPocetZnaku()
        {
            this.parent.setPocetZnaku(Operations.CalculateCountLetter(this.mainString.ToString()).ToString());
        }

        private void setPocetRadku()
        {
            this.parent.setPocetRadku(Operations.CalculateCountRadek(this.mainString.ToString()).ToString());

        }

        private int getPocetRadku()
        {
            return this.parent.getPocetRadku();

        }

        private int getPocetZnaku()
        {
            return this.parent.getPocetZnaku();
        }

        internal async void CopyOperation(string path)
        {

            // Get the selected file name and create a StreamWriter

            await this.operation.saveFile(path, mainString, cts.Token);

        }

        internal async Task DeleteBlankLines()
        {
            //KONTROLY

            Task<string> resultTask = this.operation.OdstranPrazdneRadky(this.getPocetRadku(), this.mainString.ToString(), cts.Token);
            try
            {
                this.ResetProgressBar();
                string result = await resultTask;
                this.mainString.Clear().Append(result);
                this.Calculate();
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation canceled.");
            }

        }

        internal async Task DeleteDiacticInText()
        {
            Task<string> resultTask = this.operation.OdstranDiakritiku(this.getPocetZnaku(), this.mainString.ToString(), cts.Token);
            try
            {
                this.ResetProgressBar();
                string result = await resultTask;
                this.mainString.Clear().Append(result);
                this.setPocetZnaku();
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation canceled.");
            }



        }

        private async void ResetProgressBar()
        {
            await this.operation.ResetProgressBar();
        }

        internal async Task DeleteWhiteLinesPuncChars()
        {
            Task<string> resultTask = this.operation.OdstranMezeryInterpunkcniZnamenka(this.getPocetZnaku(), this.mainString.ToString(), cts.Token);
            try
            {
                this.ResetProgressBar();
                string result = await resultTask;
                this.mainString.Clear().Append(result);
                this.Calculate();
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation canceled.");
            }
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

        internal void StopProcessingFile()
        {
            // Zrušte asynchronní načítání souboru, pokud je aktivní
            if (cts != null)
            {
                cts.Cancel();
            }
        }
    }
}
