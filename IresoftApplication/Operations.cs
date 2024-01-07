using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace IresoftApplication
{
    //Třída, kde se budou počitat všechny operace
    public class Operations
    {
        //Komunikace s progressBarem
        public delegate void ValueChangedHandler(int value, bool max);
        public event ValueChangedHandler ValueChanged;
        private int periodNumber = 0;
        private int periodNumberCurrent = 0;


        #region ProgressBar
        public void SendSignalToProgressBar(int value, bool max)
        {
            //Resetování progressBaru
            if (value == -1)
            {
                ValueChanged?.Invoke(-1, false);
                periodNumber = 0;
                periodNumberCurrent = 0;
                return;
            }

            //Ukončení průběhu operace - odeslání zbytkových hodnot do progressBaru
            if (value == -2)
            {
                ValueChanged?.Invoke(periodNumberCurrent, false);
                periodNumberCurrent = 0;
            }

            //Schování PB
            if (value == -3)
                ValueChanged?.Invoke(-3, false);

            //Odeslání maximální hodnoty do PB
            if (max)
            {
                // Zajistění, aby byla událost volána v případě, že je nějaký posluchač připojen
                ValueChanged?.Invoke(value, max);
            }
            //Odeslání aktuální hodnoty do PB
            else
            {

                periodNumberCurrent += value;
                //Odeslání do PB hodnotu pouze tehdy, pokud přesahuje 1 z celkového množství
                if (periodNumberCurrent >= periodNumber)
                {
                    ValueChanged?.Invoke(periodNumberCurrent, false);
                    periodNumberCurrent = 0;
                }
            }
        }

        //Resetování progressBaru
        internal void ResetProgressBar()
        {
            SendSignalToProgressBar(-1, false);
        }

        //Nastavení progressBaru
        private void setttingNotificationProgressBar(int pocetZnaku)
        {
            //Interval po kterých se bude aktualizovat progressBar
            periodNumber = Convert.ToInt32(Math.Round((double)pocetZnaku / 100, 0, MidpointRounding.ToPositiveInfinity));
            //Poslání maximální hodnoty do PB
            SendSignalToProgressBar(pocetZnaku, true);
        }
        #endregion

        #region Labely
        //Obsluha labelů
        public static int CalculateCountSentences(string mainString)
        {
            if (string.IsNullOrEmpty(mainString)) return 0;

            char[] sentenceSeparators = { '.', ',', '!', '?', ':' };

            int sentenceCount = mainString.Count(c => sentenceSeparators.Contains(c));

            return sentenceCount;
        }

        public static int CalculateCountWords(string mainString)
        {
            if (mainString.Length == 0) return 0;

            string[] words = mainString.Split(new char[] { ' ', '\t', '\n', '\r', '.' }, StringSplitOptions.RemoveEmptyEntries);

            return words.Length;
        }

        public static int CalculateCountLetter(string mainString)
        {
            return mainString.ToCharArray().Count();
        }

        public static int CalculateCountLines(string mainString)
        {
            var count = 0;

            using (System.IO.StringReader reader = new System.IO.StringReader(mainString))
            {
                while (reader.ReadLine() != null)
                {
                    count++;
                }
            }

            return count;

        }
        //Konec obsluhy labelů
        #endregion

        #region Metody
        //Definice metod pro obsluhu tlačítek - diakritika, mezery, atd.

        public async Task<string> RemoveDiacritics(int pocetZnaku, string mainString, CancellationToken cts)
        {
            var stringBuilder = new StringBuilder();
            char[] buffer = new char[1];

            setttingNotificationProgressBar(pocetZnaku);
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            int read;

            using (StringReader stringReader = new StringReader(mainString.Normalize(NormalizationForm.FormD)))
            {
                while ((read = await stringReader.ReadAsync(buffer, cts)
                        .ConfigureAwait(false)) > 0)
                {
                    char character = buffer[0];

                    var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(character);
                    if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    {
                        stringBuilder.Append(character);
                        SendSignalToProgressBar(1, false);
                    }
                }

                SendSignalToProgressBar(-2, false);
            }

            tcs.SetResult(stringBuilder.ToString().Normalize(NormalizationForm.FormC));
            return await tcs.Task;
        }

        public async Task<string> RemoveEmptyLinesAsync(int pocetRadku, string mainString, CancellationToken cts)
        {
            int pocetRadkuTemp = pocetRadku;
            StringBuilder stringBuilder = new StringBuilder();
            setttingNotificationProgressBar(pocetRadku);
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

            using (StringReader stringReader = new StringReader(mainString))
            {
                string? line;
                while ((line = await stringReader.ReadLineAsync(cts)) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line.Trim()))
                    {
                        stringBuilder.Append(line);
                    }
                    SendSignalToProgressBar(1, false);
                }
                SendSignalToProgressBar(-2, false);
            }

            tcs.SetResult(stringBuilder.ToString());
            return await tcs.Task;
        }

        public async Task<string> RemoveSpacesAndPunctuationAsync(int pocetZnaku, string mainString, CancellationToken cts)
        {
            char[] buffer = new char[1];
            StringBuilder stringBuilder = new StringBuilder();
            setttingNotificationProgressBar(pocetZnaku);
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            int read;

            using (StringReader stringReader = new StringReader(mainString))
            {
                while ((read = await stringReader.ReadAsync(buffer, cts)
                        .ConfigureAwait(false)) > 0)
                {
                    char character = buffer[0];

                    if (!char.IsWhiteSpace(character) && !char.IsPunctuation(character))
                    {
                        stringBuilder.Append(character);
                    }
                    SendSignalToProgressBar(1, false);
                }
                SendSignalToProgressBar(-2, false);
            }
            tcs.SetResult(stringBuilder.ToString());
            return await tcs.Task;
        }

        internal async Task<string> LoadFileAsync(string path, CancellationToken cts)
        {
            char[] buffer = new char[1024];
            int read;
            var resultBuilder = new StringBuilder();
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

            try
            {
                setttingNotificationProgressBar(1);

                // Reader
                using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
                {
                    //Dokumentace (3.příklad)
                    //Bude číst po bufferech dokud nebude 0
                    while ((read = await reader.ReadAsync(buffer, cts)
                        .ConfigureAwait(false)) > 0)
                    {
                        resultBuilder.Append(buffer, 0, read);
                    }
                }
                tcs.SetResult(resultBuilder.ToString());
            }
            catch (Exception ex)
            {
                // Handle errors that may occur during file reading
                Console.WriteLine("Error while reading the file: " + ex.Message);
                tcs.SetException(ex);
            }
            finally
            {
                SendSignalToProgressBar(1, false);
            }

            return await tcs.Task;
        }

        internal async Task SaveStringToFileAsync(string path, StringBuilder mainString, CancellationToken cts)
        {
            try
            {
                setttingNotificationProgressBar(1);

                using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
                {
                    await writer.WriteLineAsync(mainString, cts).ConfigureAwait(false);
                    SendSignalToProgressBar(1, false);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error write file: {ex.Message}");
            }

            return;
        }

        //Konec metod
        #endregion
    }
}
