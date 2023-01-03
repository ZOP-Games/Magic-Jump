namespace GameExtensions.Nonplayer
{
    public interface IInteractable
    {
        public string OwnName { get; }
        public void Interact();
    }
}