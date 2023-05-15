using System.Linq;
using GameExtensions.Debug;
using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace GameExtensions.UI
{
    public class TabGroup : RowContainer
    {
        private Tab[] tabs;
        private Tab activeTab;
        private byte activeTabIndex;

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
                DebugConsole.LogError($"Can't select active tab because the ID provided ({id}) is invalid.");
                return;
            }
            activeTab.Deactivate();
            activeTabIndex = id;
            activeTab = tabs[activeTabIndex];
            activeTab.Activate();
            //RefreshLayout();
        }

        internal void RefreshLayout()
        {
            BuildLayout();
        }

        private void OnEnable()
        {
            tabs = GetComponentsInChildren<Tab>();
            if(tabs.Length < 1) DebugConsole.Log("There are no elements to organise.",Color.yellow);
            for (var i = 0; i < tabs.Length; i++)
            {
                tabs[i].id = (byte)i;
                tabs[i].Deactivate();
            }
            elements = tabs.Select(t => t.GetComponent<RectTransform>()).ToArray();
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
        }

       
    }
}