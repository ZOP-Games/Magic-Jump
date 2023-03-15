using GameExtensions.Debug;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace GameExtensions.UI
{
    public class TabGroup : UIComponent
    {
        private Tab[] tabs;
        private Tab activeTab;
        private byte activeTabIndex;
        private const float TabPadding = 5;

        public void NextTab()
        {
            var index = activeTab == tabs[^1] ? byte.MinValue : ++activeTabIndex;
            SetActiveTab(index);
        }
        
        public void PreviousTab()
        {
            var index = activeTab == tabs[0] ? (byte) (tabs.Length-1) : --activeTabIndex;
            SetActiveTab(index);
        }

        internal void SetActiveTab(byte id)
        {
            if (id >= tabs.Length)
            {
                DebugConsole.LogError("Can't select active tab because the ID provided is invalid.");
                return;
            }
            activeTab.Deactivate();
            activeTabIndex = id;
            activeTab = tabs[activeTabIndex];
            activeTab.Activate();
            BuildLayout();
        }

        private void OnEnable()
        {
            tabs = GetComponentsInChildren<Tab>();
            for (var i = 0; i < tabs.Length; i++)
            {
                tabs[i].id = (byte)i;
                tabs[i].Deactivate();
            }
            UnityEngine.Debug.Assert(ES.currentInputModule is InputSystemUIInputModule,
                "Current Input Module is not of type InputSystemUIInputModule.");
            var input = ES.currentInputModule as InputSystemUIInputModule;
            input!.actionsAsset["Tab navigation"].Enable();
            input!.actionsAsset["Tab navigation"].performed += context =>
            {
                if(!context.performed) return;
                var val = context.ReadValue<float>();
                if (val < 0) PreviousTab();
                else NextTab();
            };
            activeTab = tabs[0];
            activeTab.Activate();
            BuildLayout();
        }

        internal void BuildLayout()
        {
            if (tabs is null || tabs.Length < 1)
            {
                DebugConsole.LogError("The tab layout could not be built: There are no tabs to organize.");
                return;
            }
            DebugConsole.Log("Building layout");
            for (var i = 0; i < tabs.Length; i++)
            {
                DebugConsole.Log("setting position for: " + tabs[i].name);
                var rtf = tabs[i].GetComponent<RectTransform>();
                var pos = rtf.anchoredPosition;
                var xDelta = rtf.sizeDelta.x;
                pos.x = i*xDelta+xDelta / 2+(i+1)*TabPadding;   //todo:fix layout calculations w/ scaling
                rtf.anchoredPosition = pos;
            }
        }
    }
}