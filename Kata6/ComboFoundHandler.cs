using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Kata6
{
    public class ComboFoundHandler : IRequestHandler<ComboFound, FoundWord>
    {
        private readonly TextWriter _writer;

        public ComboFoundHandler(TextWriter writer)
        {
            _writer = writer;
        }

        public async Task<FoundWord> Handle(ComboFound request, CancellationToken cancellationToken)
        {
            await _writer.WriteLineAsync($"Handling ComboFound: {request.Combo}");
            return new FoundWord {Word = request.Combo};
        }
    }
}