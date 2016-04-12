using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRS_Demo.Search
{
    public class QueryTerm
    {
        public string Term { get; set; }
        public string Field { get; set; }

        public readonly char[] CharactersToRemove = { '(', ')', '-', ',', '.' };

        private readonly string[] _stopWords = new string[0];

        public bool IsCombinationTerms()
        {
            if (string.IsNullOrEmpty(Term)) { return false; }

            return Terms().Count > 1;
        }

        public List<string> Terms()
        {
            if (string.IsNullOrEmpty(Term)) { return new List<string>(); }

            var splitTerms = Term.Split(',').ToList().Select(x => x.Trim()).ToList();

            return splitTerms;
        }


        public bool ContainsBlankSpace
        {
            get { return Term.Contains(" "); }
        }

        public string[] GetSplitTextRemovingStopWords()
        {
            var split = Term.Split(' ');

            var words = new List<string>();

            foreach (var s in split)
            {
                if (_stopWords.All(sw => sw != s) && !string.IsNullOrEmpty(s))
                    words.Add(s);
            }

            return words.ToArray();
        }
    }
}