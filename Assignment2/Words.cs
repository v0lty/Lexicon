using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using static System.Console;

namespace Assignment2
{
    class RandomWord
    {
        private readonly Random random = new Random();
        private readonly string[] wordArray = null;
        private readonly List<int> usedIndexes = new List<int>();

        public string Next()
        {
            if (usedIndexes.Count >= wordArray.Length)               
                usedIndexes.Clear(); // overload -> reset

            int index;
            do  {
                index = random.Next(0, wordArray.Length);
            }
            while (usedIndexes.Contains(index));

            usedIndexes.Add(index);
            return wordArray[index].ToUpper();
        }

        public RandomWord()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "words.txt");

            if (File.Exists(path))
            {
                try {
                    // Comma-separated value
                    wordArray = File.ReadAllText(path).Split(",");
                }
                catch (Exception ex)
                {
                    WriteLine($"An error occured reading from file: {ex.Message}.");
                }
            }
            else /*first run, no words generated yet -> fetch & save*/
            {
                var url = @"https://www.ef.com/wwen/english-resources/english-vocabulary/top-1000-words/";
                var div = "content";
                var tag = "<br>\r\n\t";

                var words = FetchWordsFromHTML(url, div, tag);
                if (words != null)
                {
                    wordArray = words.Where(x => x.Length > 5 && x.Length < 7).ToArray();

                    try {
                        // Comma-separated value
                        string strArray = string.Join(",", wordArray); 
                        File.WriteAllText(path, strArray);
                    }
                    catch (Exception ex)
                    {
                        WriteLine($"An error occured reading from file: {ex.Message}.");
                    }                    
                }
            }
        }

        private string[] FetchWordsFromHTML(string url, string div, string tag)
        {
            var html = string.Empty;

            try {
                using (var client = new WebClient())
                {
                    html = client.DownloadString(url);
                    //WriteLine($"Downloaded {(html.Length * 2) * .001} kb of data..");
                }
            }
            catch (Exception ex)
            {
                WriteLine($"An error occured while fetching webpage for words: {ex.Message}.");
                return null;
            }
            
            int divHead = html.IndexOf($@"<div class=""{div}"">");
            int divTail = html.IndexOf("</div>", Math.Max(divHead, 0));

            if (divHead < 0
             || divTail < divHead) {
                WriteLine($"No div class named '{div}' was fount at url '{url}'.");
                return null;
            }
            
            return html[divHead..divTail].Split(tag);
        }
    }
}