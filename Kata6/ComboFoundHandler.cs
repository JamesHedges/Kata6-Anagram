using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using System.Linq;

namespace Kata6
{
    public class ComboFoundHandler : INotificationHandler<ComboFound>
    {
        private readonly TextWriter _writer;
        private readonly List<string> _words;

        public ComboFoundHandler(TextWriter writer, List<string> words)
        {
            _writer = writer;
            _words = words;
        }

        public async Task Handle(ComboFound notification, CancellationToken cancellationToken)
        {
            int wordIndex = _words.BinarySearch(notification.Combo);
            if (wordIndex >= 0)
            {
                    _writer.Write($@" {notification.Combo}");
            }
        }
    }
}