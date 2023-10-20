namespace GameExtensions
{
    public interface IMediator
    {
        public void Mediate<T>(NotifyableState<T> state, T value);
    }
}