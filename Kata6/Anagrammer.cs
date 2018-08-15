using System;
using System.Collections.Generic;
using System.Linq;

namespace Kata6
{
    public class Anagrammer
    {

        public Anagrammer(WrappingWriter writer)
        {
        }

        public List<IGrouping<string, string>> ReturnAnagram(List<string> words)
        {
            return GetAll(words).ToList();
        }
        public List<IGrouping<string, string>> ReturnAnagram(List<string> words, List<string> wordTest)
        {
            return GetAll(words)
                .Where(g => g.Any(x => wordTest.Contains(x)))
                .ToList();
        }

        private IEnumerable<IGrouping<string, string>> GetAll(List<string> words)
        {
            return words.GroupBy(w => String.Concat(w.OrderBy(c => c)))
                .Where(g => g.Count() > 1);
        }
    }
}