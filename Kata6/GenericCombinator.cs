using System;
using System.Collections.Generic;
using System.Linq;

namespace Kata6
{
    public class GenericCombinator<T> : ICombinator<T>
    {
        public List<T[]> FindCombinations(T[] source)
        {
            List<T[]> combinations = new List<T[]>();
            T[] searchSource = (T[]) source.Clone();
            Queue<T> items = new Queue<T>(source);
            ProcessStack(searchSource, new Stack<T>(), items, combinations);
            return combinations;
        }

        private void ProcessStack(T[] searchSource, Stack<T> prefix, Queue<T> remainder, List<T[]> combinations)
        {
            if (remainder.Any())
            {
                var count = remainder.Count;
                for (var i = 0; i < count; i++)
                {
                    prefix.Push(remainder.Dequeue());
                    ProcessStack(searchSource, prefix, remainder, combinations);
                }
            }
            else
            {
                combinations.Add(prefix.Reverse().ToArray());
            }
            if (prefix.Any())
            {
                remainder.Enqueue(prefix.Pop());
            }
        }
    }
}