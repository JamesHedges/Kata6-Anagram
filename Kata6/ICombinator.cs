using System.Collections.Generic;

namespace Kata6
{
    public interface ICombinator<T>
    {
        List<T[]> FindCombinations(T[] source);
    }
}