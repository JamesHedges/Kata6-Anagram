using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Autofac;
using MediatR;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Kata6
{
    class Program
    {
        static void Main()
        {
            List<string> words;
            List<string> wordTest = new List<string>
            {
                "ABC",
                "paste",
                "kinship",
                "kinships",
                "enlist",
                "boaster",
                "fresher",
                "sinks",
                "knits",
                "rots",
                "crepitus",
                "punctilio",
                "sunders",
                "MA's",
                "epee",
                "aardvark"
            };

            words = LoadWords().Result;
            var writer = new WrappingWriter(Console.Out);
            var mediator = BuildMediator(writer, words);

            RunAnagrammer(writer, words, wordTest);

            //RunCombinator(writer, words, wordTest, mediator);

            //RunAnagram(writer, words, wordTest, mediator);

            //RunAnagram2A(writer, words, wordTest);
            RunAnagram2B(writer, words, wordTest);

            Console.ReadKey();
        }

        private static void RunAnagram(WrappingWriter writer, List<string> words, List<string> wordTest, IMediator mediator)
        {
            Stopwatch stopwatch = new Stopwatch();
            Anagram anagram = new Anagram(writer, mediator);
            GenericCombinator<char> combinations = new GenericCombinator<char>();

            writer.WriteLine($"{Environment.NewLine}Running Anagram");
            stopwatch.Start();
            foreach (var word in wordTest)
            {
                writer.Write($"{word}:");
                anagram.FindAll(word);
                writer.WriteLine();
            }
            stopwatch.Stop();
            writer.WriteLine($"{Environment.NewLine}Done in {stopwatch.ElapsedMilliseconds}ms!");
        }

        private static void RunAnagram2A(WrappingWriter writer, List<string> words, List<string> wordTest)
        {
            Stopwatch stopwatch = new Stopwatch();
            ICombinator<char> combinator = new GenericCombinator<char>();
            Anagram2 anagram = new Anagram2(combinator, words);

            writer.WriteLine($"{Environment.NewLine}Running Anagram2A");
            stopwatch.Start();
            foreach (var word in wordTest)
            {
                writer.Write($"{word}:");
                var results = anagram.FindAll(word);
                foreach(var result in results)
                {
                    writer.Write($@" {result}");
                }
                writer.WriteLine();
            }
            stopwatch.Stop();
            writer.WriteLine($"{Environment.NewLine}Done in {stopwatch.ElapsedMilliseconds}ms!");
        }

        private static void RunAnagram2B(WrappingWriter writer, List<string> words, List<string> wordTest)
        {
            Stopwatch swSearch = new Stopwatch();
            Stopwatch swElapsed = new Stopwatch();
            ICombinator<char> combinator = new GenericCombinator<char>();
            Anagram2 anagram = new Anagram2(combinator, words);

            writer.WriteLine($"{Environment.NewLine}Running Anagram2B");
            swElapsed.Start();
            swSearch.Start();
            var anagrams = anagram.FindAll(wordTest);
            swSearch.Stop();
            foreach (var pair in anagrams)
            {
                writer.Write($"{pair.Key}:");
                foreach(var result in pair.Value)
                {
                    writer.Write($@" {result}");
                }
                writer.WriteLine();
            }
            swElapsed.Stop();

            writer.WriteLine($"{Environment.NewLine}Anagram2B completed! {Environment.NewLine}\tSearchTime:   {swSearch.Elapsed}{Environment.NewLine}\tElapsed Time: {swElapsed.Elapsed}");
            writer.WriteLine($"Number of words: {words.Count}");
            writer.WriteLine($"Number of words to test: {wordTest.Count}");
            writer.WriteLine($"Number of anagram pairs: {anagrams.Count}");
        }

        private static void RunCombinator(WrappingWriter writer, List<string> wordTest, IMediator mediator)
        {
            Stopwatch stopwatch = new Stopwatch();
            GenericCombinator<char> combinations = new GenericCombinator<char>();

            writer.WriteLine($"{Environment.NewLine}Running Combinator");
            stopwatch.Start();
            foreach (var word in wordTest)
            {
                writer.Write($"{word}:");
                var combos = combinations.FindCombinations(word.ToCharArray());
                foreach(var combo in combos)
                {
                    mediator.Publish(new ComboFound { Combo = new string(combo) }).Wait();
                }
                writer.WriteLine();
            }
            stopwatch.Stop();
            writer.WriteLine($"{Environment.NewLine}Done in {stopwatch.ElapsedMilliseconds}ms!");
        }

        private static void RunAnagrammer(WrappingWriter writer, List<string> words, List<string> wordTest)
        {
            Stopwatch swSearch = new Stopwatch();
            Stopwatch swElapsed = new Stopwatch();
            Anagrammer anagrammer = new Anagrammer(writer);

            writer.WriteLine($"{Environment.NewLine}Starting Anagrammer");

            swElapsed.Start();
            swSearch.Start();
            var resultSort = anagrammer.ReturnAnagram(words, wordTest);
            swSearch.Stop();

            resultSort.ForEach(g => writer.WriteLine(String.Join(" ", g)));
            swElapsed.Stop();


            writer.WriteLine($"{Environment.NewLine}Anagram2B completed! {Environment.NewLine}\tSearchTime:   {swSearch.Elapsed}{Environment.NewLine}\tElapsed Time: {swElapsed.Elapsed}");
            writer.WriteLine($"Number of words: {words.Count}");
            writer.WriteLine($"Number of words to test: {wordTest.Count}");
            writer.WriteLine($"Number of anagram pairs: {resultSort.Count}");
        }

        private static IMediator BuildMediator(WrappingWriter writer, List<string> words)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var mediatorOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>)
            };

            foreach (var mediatorOpenType in mediatorOpenTypes)
            {
                builder
                    .RegisterAssemblyTypes(typeof(Anagram).GetTypeInfo().Assembly)
                    .AsClosedTypesOf(mediatorOpenType)
                    .AsImplementedInterfaces();
            }

            builder.RegisterInstance(writer).As<TextWriter>();
            builder.RegisterInstance(words).As <List<string>>();

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            var container = builder.Build();

            return container.Resolve<IMediator>();
        }

        private static async Task<List<string>> LoadWords()
        {
            HttpClient client;
            string path = "data/wordlist.txt";
            Dictionary<string, string> parameters = new Dictionary<string, string> { { "text", path } };
            List<string> words = new List<string>();
            client = new HttpClient()
            {
                BaseAddress = new Uri("http://codekata.com")
            };

            var response = await client.GetStreamAsync(path);
            using (var reader = new StreamReader(response))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    words.Add(line);
                }
            }
            words.Sort();
            return words;
        }
    }
}
