using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IresoftApplication
{
    public class Operations
    {
        // Define a delegate and event for the signal
        public delegate void SignalEventHandler(object sender, int value, bool maxValue);

        public event SignalEventHandler SignalEvent;

        // Method to initiate the signal
        private void SendSignalToProgressBar(int value, bool maxValue)
        {
            // Check if there are subscribers to the event
            SignalEvent?.Invoke(this, value, maxValue);
        }

        public static int CalculateCountSentences(string mainString)
        {
            if (string.IsNullOrEmpty(mainString)) return 0;

            char[] sentenceSeparators = { '.', ',', '!', '?', ':' };

            int sentenceCount = mainString.Count(c => sentenceSeparators.Contains(c));

            return sentenceCount;
        }

        public static int CalculateCountWords(string mainString)
        {
            if (string.IsNullOrEmpty(mainString)) return 0;

            int Count = mainString.Count(c => c == ' ');

            return Count;
        }

        public static int CalculateCountLetter(string mainString)
        {
            return mainString.ToCharArray().Count();
        }

        public static int CalculateCountRadek(string mainString)
        {
            if (string.IsNullOrEmpty(mainString)) return 0;

            int radek = mainString.Split('\n').Length;

            return radek;
        }

        //METHODS FOR OPERATION B.

        public async Task<string> OdstranDiakritiku(int pocetZnaku, string mainString, CancellationToken cts)
        {
            int pocetZnakuTemp = pocetZnaku;
            char[] buffer = new char[1];
            StringBuilder stringBuilder = new StringBuilder();
            SendSignalToProgressBar(pocetZnaku, true);
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            int read;

            using (StringReader stringReader = new StringReader(mainString))
            {
                while ((read = await stringReader.ReadAsync(buffer, cts)
                        .ConfigureAwait(false)) > 0)
                {
                    char character = buffer[0];

                    if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
                    {
                        stringBuilder.Append(character);
                        SendSignalToProgressBar(pocetZnaku - (--pocetZnakuTemp), false);
                    }
                }
            }

            tcs.SetResult(stringBuilder.ToString());

            return await tcs.Task;
        }



        public async Task<string> OdstranPrazdneRadky(int pocetRadku, string mainString, CancellationToken cts)
        {
            int pocetRadkuTemp = pocetRadku;
            StringBuilder stringBuilder = new StringBuilder();
            SendSignalToProgressBar(pocetRadku, true);
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

            using (StringReader stringReader = new StringReader(mainString))
            {
                string? line;
                while ((line = await stringReader.ReadLineAsync(cts)) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line.Trim()))
                    {
                        stringBuilder.Append(line);
                        SendSignalToProgressBar(pocetRadku - (--pocetRadkuTemp), true);
                    }
                }
            }

            tcs.SetResult(stringBuilder.ToString());

            return await tcs.Task;
        }

        public async Task<string> OdstranMezeryInterpunkcniZnamenka(int pocetZnaku, string mainString, CancellationToken cts)
        {
            int pocetZnakuTemp = pocetZnaku;
            char[] buffer = new char[1];
            StringBuilder stringBuilder = new StringBuilder();
            SendSignalToProgressBar(pocetZnaku, true);
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
                        SendSignalToProgressBar(pocetZnaku - (--pocetZnakuTemp), false);
                    }
                }
            }

            tcs.SetResult(stringBuilder.ToString());

            return await tcs.Task;
        }

        internal async Task<string> LoadFileAsync(string path, CancellationToken cts)
        {
            char[] buffer = new char[4096];
            int read;
            var resultBuilder = new StringBuilder();

            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

            try
            {
                // Reader
                using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
                {
                    SendSignalToProgressBar(Convert.ToInt32(reader.BaseStream.Length), true);
                    //Dokumentace (3.příklad)
                    //Bude číst po bufferech dokud nebude 0
                    while ((read = await reader.ReadAsync(buffer, cts)
                        .ConfigureAwait(false)) > 0)
                    {
                        resultBuilder.Append(buffer, 0, read);
                        // Raise the event
                        SendSignalToProgressBar(read, false);
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

            return await tcs.Task;
        }

        internal async Task saveFile(string path, StringBuilder mainString, CancellationToken cts)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
                {
                    SendSignalToProgressBar(1, true);

                    await writer.WriteLineAsync(mainString, cts);

                    SendSignalToProgressBar(1, false);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error write file: {ex.Message}");
            }

            return;
        }
    }
}
