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
        [SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private GameObject lineTemplate;
        private TMP_Dropdown schemesDropdown;
        private byte bindingIndex;
        public void SetBindingIndex(int scheme)
        {
            bindingIndex = (byte)scheme;
            DebugConsole.Log("scheme is " + scheme);
        }

        private void Awake()
        {
            schemesDropdown = GetComponentInChildren<TMP_Dropdown>();
            if (inputActionAsset.controlSchemes.Count == 0)
            {
                DebugConsole.LogError("No control schemes detected!");
                return;
            }
            var deviceNames = inputActionAsset.controlSchemes.Select(s => s.name).ToList();
            schemesDropdown.AddOptions(deviceNames);
            var filteredActions = inputActionAsset.Where(a => a.actionMap.name != "UI");
            foreach (var action in filteredActions)
            {
                /*DebugConsole.Log("Name: " + action.name
                    + ", Is Composite?: " + action.bindings[bindingIndex].isComposite + ", Is part of composite?: " 
                    + action.bindings[bindingIndex].isPartOfComposite);*/
                var obj = Instantiate(lineTemplate, transform.position, Quaternion.identity,
                lineTemplate.transform.parent);
                obj.SetActive(true);
                DrawBinding(action, obj.GetComponentsInChildren<TextMeshProUGUI>());
                var bindingIndices = action.bindings.Where(b => b.effectivePath.Contains('/')).GroupBy(b => b.effectivePath.Split('/')[0]).ElementAt(bindingIndex)
                    .Select(b => action.bindings.ToList().IndexOf(b)).ToArray();
                obj.GetComponentInChildren<Button>().onClick.AddListener(() =>
                {
                    RebindComposite(action, bindingIndices,obj.GetComponentsInChildren<TextMeshProUGUI>());
                });

            }
        }

        private void DrawBinding(InputAction action, TextMeshProUGUI[] txts)
        {
            txts[0].SetText(action.name);
            txts[1].SetText(action.GetBindingDisplayString().Replace('\u0001', '\u200b')
                .Replace('\u001b', '\u200b'));    //removing bad characters to get rid of warnings
        }

        private void RebindComposite(InputAction action, int[] bIndices,TextMeshProUGUI[] textBoxes)
        {
            var i = 0;
            DebugConsole.Log(action.name + ", #" + bIndices[i] + ", binding: " + action.bindings[bIndices[i]].effectivePath);
            Rebind();
            void Rebind()
            {
                action.Disable();
                action.PerformInteractiveRebinding(bIndices[i])
                    .OnCancel((op) =>
                    {
                        action.Enable();
                        DebugConsole.Log("Remapping canceled");
                        op.Dispose();
                    })
                    .OnComplete((op) =>
                    {
                        action.Enable();
                        DebugConsole.Log("Remap finished");
                        i++;
                        if(i < bIndices.Length) Rebind();
                        else DrawBinding(action,textBoxes);
                        op.Dispose();
                    })
                    .OnMatchWaitForAnother(0.1f)
                    .WithCancelingThrough("<Keyboard>/Escape")
                    .Start();
                    DebugConsole.Log("Remapping began, press a key...");
            }
        }
    }
}