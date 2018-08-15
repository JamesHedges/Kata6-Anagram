using System;
using System.Linq;
using Xunit;
using Shouldly;

namespace Kata6.Test
{
    public class CombinatorTests
    {
        [Fact]
        public void TestIt()
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
}
