namespace GameExtensions
{
    public interface INotifyable<in T>
    {
        public void Notify(T data);
    }
}