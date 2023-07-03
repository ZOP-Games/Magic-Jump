using System;
using UnityEngine;
using GameExtensions.UI;
// ReSharper disable ConvertIfStatementToNullCoalescingAssignment
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

        private void Start()
        {
            if (options is null) options = FindObjectOfType<OptionsScreen>(true);
        }
    }
}