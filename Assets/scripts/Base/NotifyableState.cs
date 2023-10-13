namespace GameExtensions
{
    public abstract class NotifyableState<T> : State, INotifyable<T>
    {
        protected NotifyableState(StateManager context) : base(context)
        {
        }

        public abstract void Notify(T data);
    }
}