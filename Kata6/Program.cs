using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Autofac;
using MediatR;
using System.Collections.Concurrent;

namespace Kata6
{
    class Program
    {
        static void Main()
        {
            ConcurrentBag<string> words;
            List<string> combos;
            Anagram anagram;

            var writer = new WrappingWriter(Console.Out);
            var mediator = BuildMediator(writer);

            anagram = new Anagram(writer, mediator, "ABCD");
            combos = anagram.FindAll();
            Console.WriteLine($"Found {combos.Count} combinations. Expected {anagram.ExpectedCombos} combinations");
            Console.ReadKey();
        }

        private static IMediator BuildMediator(WrappingWriter writer)
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

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            var container = builder.Build();

            return container.Resolve<IMediator>();
        }
    }
}
