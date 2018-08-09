using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MediatR;

namespace Kata6
{
    public class Anagram
    {
        private List<string> _combinations;
        private TextWriter _writer;
        private readonly IMediator _mediator;

        public Anagram(WrappingWriter writer, IMediator mediator, string letters)
        {
            _writer = writer;
            _mediator = mediator;
            Letters = new Queue<char>(letters.ToCharArray());
        }

        private List<string> Words => _combinations ?? (_combinations = new List<string>());

        public Queue<char> Letters { get; private set; }

        public long ExpectedCombos => GetFactorial(Letters.Count);
        public long WordCount => Words.Count;

        public List<string> FindAll()
        {
            Stopwatch stopwatch = new Stopwatch();

            Words.Clear();
            stopwatch.Start();
            ProcessStack(new Stack<char>(), Letters);
            stopwatch.Stop();
            _writer.WriteLine($"{stopwatch.ElapsedMilliseconds/1000} seconds to get all combinations.");
            return Words;
        }

        private void ProcessStack(Stack<char> prefix, Queue<char> remainder)
        {
            if (remainder.Any())
            {
                var count = remainder.Count;
                for (var i = 0; i < count; i++)
                {
                    prefix.Push(remainder.Dequeue());
                    ProcessStack(prefix, remainder);
                }
            }
            else
            {
                var word = string.Join(",", prefix.Reverse());
                Words.Add(word);
                _mediator.Send(new ComboFound {Combo = word});
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