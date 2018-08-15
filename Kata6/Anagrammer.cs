using System;
using System.Collections.Generic;
using System.Linq;

namespace Kata6
{
    public class Anagrammer
    {
        private readonly List<string> _words;

        public Anagrammer(List<string> words)
        {
            _words = words;
        }

        public List<IGrouping<string, string>> FindAll()
        {
            return GetAll().ToList();
        }

        public List<IGrouping<string, string>> FindAll(List<string> wordTest)
        {
            return GetAll()
                .Where(g => g.Any(wordTest.Contains))
            .ToList();
        }

        public List<IGrouping<string, string>> FindAll(string wordTest)
        {
            return GetAll()
                .Where(g => g.Any(wordTest.Contains))
                .ToList();
        }

        private IEnumerable<IGrouping<string, string>> GetAll()
        {
            return _words.GroupBy(w => String.Concat(w.OrderBy(c => c)))
                .Where(g => g.Count() > 1);
        }
    }
}