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
            var resultSort = words.GroupBy(w => String.Concat(w.OrderBy(c => c)))
                .Where(g => g.Count() > 1)
                .ToList();

            return resultSort;
        }
    }
}