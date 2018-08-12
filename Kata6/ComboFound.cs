using MediatR;

namespace Kata6
{
    public class ComboFound : INotification
    {
        public string Combo { get; set; }
    }
}