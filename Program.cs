using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace MysteryCompany
{
    class Program
    {
        static void Main()
        {
            string fl = @"c:\fe\ListOfWords.txt";
            List<string> words = new List<string>();

            try
            {
                using (StreamReader sr = File.OpenText(fl))
                {
                    string word = string.Empty;
                    while ((word = sr.ReadLine()) != null)
                    {
                        words.Add(word);
                    }
                }

                WordsClass wc = new WordsClass(words);
                Console.WriteLine("Longest concatenated word in the file: " + wc.LongestWord());
                Console.WriteLine("Second longest word: " + wc.GetWordItem(2));
                Console.WriteLine("Count of words that can be constructed of other words: " + wc.GetWordCount().ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ooopppss!!! Got exception: " + ex.Message);
            }
        }
    }

    class WordsClass
    {
        private List<string> _allwords;
        private List<string> comb = new List<string>();
        int maxlen = 0;

        public WordsClass(List<string> allWords)
        {
            _allwords = allWords;
        }
        public string LongestWord()
        {
            string longestword = String.Empty;
            try
            {
                int len = _allwords.Min(x => x.Length);
                var worditem = _allwords.Where(x => x.Length == len).Select(x => x).ToArray();
                longestword = string.Join("", worditem);
            }
            catch
            {
                throw new Exception("No word to concatenate.");
            }
            return longestword;
        }
        public string GetWordItem(int idx)
        {
            string worditem = String.Empty;
            try
            {
                int len = _allwords.Skip(idx - 1).Max(x => x.Length);
                worditem = _allwords.Where(x=>x.Length < len).Select(x=>x).OrderByDescending(x => x.Length).FirstOrDefault();
            }
            catch
            {
                throw new Exception("Beyond index.");
            }
            return worditem;
        }
        public int GetWordCount()
        {
            int cnt=0;
            List<string> newwords = new List<string>();
            try
            {
                maxlen = _allwords.Max(x => x.Length);
                //newwords = _allwords.Where(x => x.Length == len).Select(x => x).ToList();

                foreach (string s in _allwords)
                {
                    GetWordCombinations(s);
                }

                foreach (string t in comb.Distinct())
                {
                    if (_allwords.Contains(t) == true)
                    {
                        cnt++;
                    }
                }
            }
            catch
            {
                throw new Exception("Not able to find matching words.");
            }
            return cnt;
        }

        public void GetWordCombinations(string str)
        {
            foreach (string s in _allwords)
            {
                if ((str + s).Length <= maxlen)
                {
                    comb.Add(str + s);
                    comb.Add(s + str);
                    GetWordCombinations(str + s);
                    GetWordCombinations(s + str);
                }
            }
        }
        
    }
 

}
