using GameExtensions.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GameExtensions.Debug.DebugMenu
{
    public class DebugOptions : ScreenLayout
    {

        [SerializeField] private Toggle dodgeToggle;
        [SerializeField] private Toggle invincibilityToggle;
        [SerializeField] private Toggle consoleToggle;
        [SerializeField] private Toggle lineToggle;

        public void ChangeSuperDodge(bool newValue)
        {
            DebugManager.IsSuperDodging = newValue;
        }
        public void ChangeInvincibility(bool newValue)
        {
            DebugManager.IsInvincible = newValue;
        }
        public void ChangeConsole(bool _)
        {
            DebugConsole.ToggleConsole();
        }

        public void ChangeForceLine(bool newValue)
        {
            DebugManager.DrawForceLine = newValue;
        }

        protected new void Start()
        {
            dodgeToggle.SetIsOnWithoutNotify(DebugManager.IsSuperDodging);
            invincibilityToggle.SetIsOnWithoutNotify(DebugManager.IsInvincible);
            consoleToggle.SetIsOnWithoutNotify(true);
            lineToggle.SetIsOnWithoutNotify(DebugManager.DrawForceLine);
            base.Start();
        }

    }
}

