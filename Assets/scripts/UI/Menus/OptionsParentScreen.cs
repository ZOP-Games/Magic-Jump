using UnityEngine;

// ReSharper disable ConvertIfStatementToNullCoalescingAssignment
namespace GameExtensions.UI.Menus
{
    public class OptionsParentScreen : MenuScreen
    {
        [SerializeField] private OptionsScreen options;

        private void Start()
        {
            if (options is null) options = FindObjectOfType<OptionsScreen>(true);
        }

        public void OpenOptions()
        {
            options.SetParent(this);
            options.Open();
        }
    }
}