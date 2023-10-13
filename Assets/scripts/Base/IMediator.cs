using System.Xml.Serialization;
using UnityEngine.Events;

namespace GameExtensions
{
    public interface IMediator
    {
        public void Mediate<T>(NotifyableState<T> state, T value);
    }
}