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
        public Form1 form1;
        private string mainString;
        private CancellationToken cts;

        public OperationsManager()
        {
            this.operation = new Operations();
            this.cts = new CancellationToken();
            this.operation.SignalEvent += (sender, value, maxValue) => OnSignalReceived(value, maxValue);

        }

        private void OnSignalReceived(int value, bool maxValue)
        {
            form1.sendValueToProgressBar(value, maxValue);
        }

        internal async void StartLoadFile(string path)
        {
            if (!this.TestValidPath(path))
                return;

            this.PrepareFinishLoadFile(true);
            this.mainString = await operation.LoadFileAsync(path, cts);
            this.PrepareFinishLoadFile(false);
            this.Calculate();
        }

        //Prefore/Finish before/after load file
        //true = before
        //false = after
        private void PrepareFinishLoadFile(bool prepare)
        {
            form1.Cursor = prepare ? Cursors.WaitCursor : Cursors.Default;
            form1.setEnableUI(true);
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
            this.form1.setPocetVet(Operations.CalculateCountSentences(this.mainString).ToString());
        }

        private void setPocetSlov()
        {
            this.form1.setPocetSlov(Operations.CalculateCountWords(this.mainString).ToString());
        }

        private void setPocetZnaku()
        {
            this.form1.setPocetZnaku(Operations.CalculateCountLetter(this.mainString).ToString());
        }

        private void setPocetRadku()
        {
            this.form1.setPocetRadku(Operations.CalculateCountRadek(this.mainString).ToString());

        }

        internal void CopyOperatin()
        {

            // Get the selected file name and create a StreamWriter
            string fileName = saveFileDialog.FileName;

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                // Write the content of StringBuilder to the file
                sw.Write(mainString.ToString());
            }
        }

        internal async Task DeleteBlackLines()
        {
           this.mainString = await this.operation.OdstranPrazdneRadky(this.form1.getPocetRadku(), this.mainString);
            this.setPocetRadku();
        }

        internal void DeleteDiacticInText()
        {
            CalculateCountSentences();
        }

        internal void DeleteWhiteLinesPuncChars()
        {
            OdstranMezeryInterpunkcniZnamenka();
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
