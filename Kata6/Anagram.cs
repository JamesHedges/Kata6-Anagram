using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediatR;

namespace Kata6
{
    public class Anagram
    {
        private TextWriter _writer;
        private readonly IMediator _mediator;
        private const string delimiter = "";

        public Anagram(WrappingWriter writer, IMediator mediator)
        {
            _writer = writer;
            _mediator = mediator;
        }

        public Queue<char> Letters { get; private set; }

        public long ExpectedCombos => GetFactorial(Letters.Count);

        public void FindAll(string testWord)
        {
            string searchWord = testWord;
            Letters = new Queue<char>(testWord.ToCharArray());
            ProcessStack(searchWord, new Stack<char>(), Letters);
        }

        private void ProcessStack(string searchWord, Stack<char> prefix, Queue<char> remainder)
        {
            if (remainder.Any())
            {
                var count = remainder.Count;
                for (var i = 0; i < count; i++)
                {
                    prefix.Push(remainder.Dequeue());
                    ProcessStack(searchWord, prefix, remainder);
                }
            }
            else
            {
                var word = string.Join(delimiter, prefix.Reverse());
                if (word != searchWord)
                {
                    _mediator.Publish(new ComboFound { Combo = word });
                }
            }
            if (prefix.Any())
                remainder.Enqueue(prefix.Pop());
        }

        private static long GetFactorial(int number)
        {
            if (number == 0)
            {
                return 1;
            }

            return number * GetFactorial(number - 1);
        }
    }
}