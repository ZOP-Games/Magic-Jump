using UnityEngine.VFX;

namespace GameExtensions
{
    public interface ISpellwithVFX
    {
        public VisualEffect VFX { get; protected set; }
    }
}