using UnityEngine;
using GameExtensions.UI;
namespace GameExtensions.UI.Menus
{
    public class OptionsParentScreen : MenuScreen
    {
        [SerializeField] private OptionsScreen options;

        public void OpenOptions()
        {
            options.SetParent(this);
            options.Open();
        }
    }
}