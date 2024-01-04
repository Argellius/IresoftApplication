﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
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

        private void OdstranDiakritiku()
        {
            if (string.IsNullOrEmpty(mainString.ToString()))
                return;

            // Převedení řetězce na Normalization Form D (NFD)
            string normalizedString = mainString.ToString().Normalize(NormalizationForm.FormD);

            // Odstranění diakritiky (znaků s diakritikou)
            StringBuilder result = new StringBuilder();

            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    result.Append(c);
            }

            mainString = String.Empty;
            mainString = result.ToString();
        }



        public async Task<string> OdstranPrazdneRadky(int pocetRadku, string mainString)
        {
            int pocetRadkuTemp = pocetRadku;
            StringBuilder stringBuilder = new StringBuilder();
            SendSignalToProgressBar(pocetRadku, true);
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

            using (StringReader stringReader = new StringReader(mainString))
            {                
                string line;
                while ((line = await stringReader.ReadLineAsync()) != null)
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

    }



}
