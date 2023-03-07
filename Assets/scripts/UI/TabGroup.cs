using System;
using UnityEngine;

namespace GameExtensions.UI
{
    public class TabGroup : MonoBehaviour
    {
        private Tab[] tabs;
        private Tab activeTab;

        private void NextTab()
        {
            activeTab.Deactivate();
            if (activeTab == tabs[^1]) activeTab = tabs[0];
            activeTab.Activate();
        }
    }
}