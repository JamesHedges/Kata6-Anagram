using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Kata6
{
    public class Anagram2
    {
        private readonly ICombinator<char> _combinator;
        private List<string> _words;

        public Anagram2(ICombinator<char> combinator, List<string> words)
        {
            _combinator = combinator;
            _words = words;
        }

        public Dictionary<string, IEnumerable<string>> FindAll(List<string> words)
        {
            Dictionary<string, IEnumerable<string>> anagramDictionary = new Dictionary<string, IEnumerable<string>>();
            foreach(string word in words)
            {
                var results = FindAll(word);
                if (results.Any())
                {
                    anagramDictionary.Add(word, results.Where(w => w != word));
                }
            }
            return anagramDictionary;
        }

        public IEnumerable<string> FindAll(string word)
        {
            List<Task> tasks = new List<Task>();
            List<string> anagrams = new List<string>();
            var combos = _combinator.FindCombinations(word.ToCharArray());
            foreach (var combo in combos)
            {
                var comboString = new string(combo);
                if (comboString != word)
                tasks.Add(SearchForWord(anagrams, comboString));
            }

            Task.WaitAll(tasks.ToArray());

            return anagrams.Distinct();
        }

        public Task SearchForWord(List<string> anagrams, string combo)
        {
            var task = new Task(() =>
            {
                int wordIndex = _words.BinarySearch(combo);
                if (wordIndex >= 0)
                {
                    anagrams.Add(combo);
                }
            });
            task.Start();
            return task;
        }
    }
}