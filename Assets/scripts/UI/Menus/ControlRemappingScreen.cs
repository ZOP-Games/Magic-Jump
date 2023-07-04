using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using GameExtensions.Debug;
namespace GameExtensions.UI.Menus
{
    public class ControlRemappingScreen : MenuScreen
    {
        private static List<InputAction> actions;
        [SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private GameObject lineTemplate;

        private void Awake()
        {
            actions = inputActionAsset.ToList();
            foreach (var action in actions)
            {
                var obj = Instantiate(lineTemplate, transform.position, Quaternion.identity,
                    lineTemplate.transform.parent);
                var txts = obj.GetComponentsInChildren<TextMeshProUGUI>();
                txts[0].SetText(action.name);
                txts[1].SetText(action.GetBindingDisplayString().Replace('\u0001', '\u200b')
                    .Replace('\u001b', '\u200b'));    //removing bad characters to get rid of warnings
                obj.GetComponentInChildren<Button>().onClick.AddListener(() => 
                {
                    action.Disable();
                    var rebind = action.PerformInteractiveRebinding();
                    rebind.Start();
                    DebugConsole.Log("Rebinding is happening...");
                    rebind.OnComplete((op) => {
                        action.Enable();
                    txts[1].SetText(action.GetBindingDisplayString().Replace('\u0001', '\u200b')
                        .Replace('\u001b', '\u200b'));
                    });
                });
            }
        }
    }
}