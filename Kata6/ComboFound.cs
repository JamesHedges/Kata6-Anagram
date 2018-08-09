using MediatR;

namespace Kata6
{
    public class ComboFound : IRequest<FoundWord>
    {
        public string Combo { get; set; }
    }
}