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

            RunAnagrammer(writer, words);

            RunAnagram(writer, words, wordTest, mediator);

            Console.ReadKey();
        }

        private static void RunAnagram(WrappingWriter writer, List<string> words, List<string> wordTest, IMediator mediator)
        {
            Stopwatch stopwatch = new Stopwatch();
            Anagram anagram = new Anagram(writer, mediator);

            writer.WriteLine("Running Anagram");
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

        private static void RunAnagrammer(WrappingWriter writer, List<string> words)
        {
            Stopwatch stopwatch = new Stopwatch();
            Anagrammer anagrammer = new Anagrammer(writer);

            writer.WriteLine("Starting Annagramer");
            stopwatch.Start();

            var resultSort = anagrammer.ReturnAnagram(words);
            resultSort.ForEach(g => writer.WriteLine(String.Join(" ", g)));

            stopwatch.Stop();

            writer.WriteLine($"\n\rAction completed! \n\rElapsed Time: {stopwatch.Elapsed}");
            writer.WriteLine($"Number of words: {words.Count}");
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
