using UnityEngine;
using GameExtensions.UI;
using GameExtensions.Debug;
namespace GameExtensions.Debug.DebugMenu
{
    public class DebugOptions : ScreenLayout
    {
        public void ChangeSuperDodge(bool newValue)
        {
            DebugManager.IsSuperDodging = newValue;
        }
        public void ChangeInvincibility(bool newValue)
        {
            DebugManager.IsInvincible = newValue;
        }
        public void ChangeConsole(bool newValue)
        {
            DebugConsole.ToggleConsole();
        }

        public void ChangeForceRays(bool newValue){
            DebugManager.DrawForceRays = newValue;
        }
    }
}

