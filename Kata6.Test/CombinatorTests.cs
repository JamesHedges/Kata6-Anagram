using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Shouldly;

namespace Kata6.Test
{
    public class CombinatorTests
    {
        [Fact]
        public void ListOfLetters_FindCombinations_AllFound()
        {
            char[] source = { 'A', 'B', 'C' };
            long expectedCount = GetFactorial(source.Length);

            var sut = new GenericCombinator<char>();

            var result = sut.FindCombinations("ABC".ToCharArray());

            ((long)result.Count).ShouldBe(expectedCount, "Incorrect combination count returned");
            ((long)result.Distinct().Count()).ShouldBe(expectedCount, "Combos have non-unique members");
        }

        private static long GetFactorial(long number)
        {
            if (number == 0)
            {
                return 1;
            }

            return number * GetFactorial(number - 1);
        }
    }

    public class Anagram2Tests
    {
        [Fact]
        public void Anagrams_FromWordList_AllFound()
        {
            List<string> wordSelections = new List<string>{"ABC", "god", "pierce"};
            int expectedAnagrams = 3;

            Anagram2 sut = CreateAnagram2();
            var result = sut.FindAll(wordSelections);

            result.Count().ShouldBe<int>(expectedAnagrams);
        }

        [Fact]
        public void Anagrams_FromWordList_NotAllFound()
        {
            List<string> wordSelections = new List<string>{"ABC", "god", "pierce", "found"};
            int expectedAnagrams = 3;

            Anagram2 sut = CreateAnagram2();
            var result = sut.FindAll(wordSelections);

            result.Count().ShouldBe<int>(expectedAnagrams);

        }

        [Fact]
        public void Anagrams_FromWordWithRepeatedLetter_Found()
        {
            string wordSelection = "pierce";
            int expectedAnagrams = 1;

            Anagram2 sut = CreateAnagram2();
            var result = sut.FindAll(wordSelection);

            result.Count().ShouldBe<int>(expectedAnagrams);
            result.Contains("recipe").ShouldBeTrue();

        }

        [Fact]
        public void Anagrams_FromWordWithNotInList_NotFound()
        {
            string wordSelection = "duck";
            int expectedAnagrams = 0;

            Anagram2 sut = CreateAnagram2();
            var result = sut.FindAll(wordSelection);

            result.Count().ShouldBe<int>(expectedAnagrams);
            result.Contains(wordSelection).ShouldBeFalse();

        }

        [Fact]
        public void Anagrams_FromWordWithPunctuation_NotFound()
        {
            string wordSelection = "MA's";
            int expectedAnagrams = 1;

            Anagram2 sut = CreateAnagram2();
            var result = sut.FindAll(wordSelection);

            result.Count().ShouldBe<int>(expectedAnagrams);
            result.Contains("AM's").ShouldBeTrue();

        }

        private List<string> LoadWordChoices()
        {
            var words =  new List<string>
            {
                "ABC",
                "CAB",
                "dog",
                "god",
                "recipe",
                "pierce",
                "undress",
                "sunders",
                "MA's",
                "AM's",
                "paste",
                "tapes",
                "pates",
                "peats",
                "septa",
                "spate",
                "tepas",
                "kinship",
                "pinkish",
                "knits",
                "skint",
                "stink",
                "tinks",
                "duck"
            };
            return words.OrderBy(w => w).ToList();
        }

        private Anagram2 CreateAnagram2()
        {
            ICombinator<char> combinator = new GenericCombinator<char>();
            List<string> wordChoices = LoadWordChoices();

            return new Anagram2(combinator, wordChoices);

        }
    }
    public class AnagrammerTests
    {
        [Fact]
        public void Anagrams_FromWordList_AllFound()
        {
            List<string> wordSelections = new List<string>{"ABC", "god", "pierce"};
            int expectedAnagrams = 3;

            var sut = CreateAnagramProvider();
            var result = sut.FindAll(wordSelections);

            result.Count().ShouldBe<int>(expectedAnagrams);
        }

        [Fact]
        public void Anagrams_FromWordList_NotAllFound()
        {
            List<string> wordSelections = new List<string> { "ABC", "god", "pierce", "found" };
            int expectedAnagrams = 3;

            var sut = CreateAnagramProvider();
            var result = sut.FindAll(wordSelections);

            result.Count().ShouldBe<int>(expectedAnagrams);

        }

        [Fact]
        public void Anagrams_FromWordWithRepeatedLetter_Found()
        {
            string wordSelection = "pierce";
            int expectedAnagrams = 1;

            var sut = CreateAnagramProvider();
            var result = sut.FindAll(wordSelection);

            result.Count().ShouldBe<int>(expectedAnagrams);
            result.Any(g => g.Contains("recipe")).ShouldBeTrue();

        }

        [Fact]
        public void Anagrams_FromWordWithNotInList_NotFound()
        {
            string wordSelection = "duck";
            int expectedAnagrams = 0;

            var sut = CreateAnagramProvider();
            var result = sut.FindAll(wordSelection);

            result.Count().ShouldBe<int>(expectedAnagrams);
            result.Any(g => g.Contains(wordSelection)).ShouldBeFalse();

        }

        [Fact]
        public void Anagrams_FromWordWithPunctuation_NotFound()
        {
            string wordSelection = "MA's";
            int expectedAnagrams = 1;

            var sut = CreateAnagramProvider();
            var result = sut.FindAll(wordSelection);

            result.Count().ShouldBe<int>(expectedAnagrams);
            result.Any(g => g.Contains("AM's")).ShouldBeTrue();

        }

        private List<string> LoadWordChoices()
        {
            var words =  new List<string>
            {
                "ABC",
                "CAB",
                "dog",
                "god",
                "recipe",
                "pierce",
                "undress",
                "sunders",
                "MA's",
                "AM's",
                "paste",
                "tapes",
                "pates",
                "peats",
                "septa",
                "spate",
                "tepas",
                "kinship",
                "pinkish",
                "knits",
                "skint",
                "stink",
                "tinks",
                "duck"
            };
            return words.OrderBy(w => w).ToList();
        }

        private Anagrammer CreateAnagramProvider()
        {
            List<string> wordChoices = LoadWordChoices();

            return new Anagrammer(wordChoices);
        }
    }
}
