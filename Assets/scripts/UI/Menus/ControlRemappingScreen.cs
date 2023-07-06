using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using GameExtensions.Debug;

namespace GameExtensions.UI.Menus
{
    public class ControlRemappingScreen : MenuScreen
    {
        public string RebindsJson {
            get  {
                return inputActionAsset.Select(a => a.SaveBindingOverridesAsJson()).Aggregate((c,n) => c + ";" +n);
            }
        }

        [SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private GameObject lineTemplate;
        [SerializeField] private MenuScreen activeRemapScreen;
        private TMP_Dropdown schemesDropdown;
        private byte bindingIndex;
        private UnityAction RedrawBinding;

        private const string ActiveBindText = "Press a new key for ";
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
            var isAGamepadConnected = InputSystem.devices.OfType<Gamepad>().Any();
            if (!isAGamepadConnected)
            {
                deviceNames = deviceNames.Skip(1).ToList();
                bindingIndex++;
            }
            schemesDropdown.AddOptions(deviceNames);
            var filteredActions = inputActionAsset.Where(a => a.actionMap.name != "UI");
            foreach (var action in filteredActions)
            {
                var obj = Instantiate(lineTemplate, transform.position, Quaternion.identity,
                lineTemplate.transform.parent);
                obj.SetActive(true);
                DrawBinding(action, obj.GetComponentsInChildren<TextMeshProUGUI>());
                obj.GetComponentInChildren<Button>().onClick.AddListener(() =>
                {
                    var bindingIndices = action.bindings
                    .Where(b => !b.isComposite)
                    .GroupBy(b => b.effectivePath.Split('/').First())
                    .ElementAt(bindingIndex)
                    .Select(b => action.bindings.ToList().IndexOf(b))
                    .ToArray();
                    StartCoroutine(RebindCoroutine(action, bindingIndices));
                    RedrawBinding = () =>
                    {
                        DrawBinding(action, obj.GetComponentsInChildren<TextMeshProUGUI>());
                    };
                });

            }
        }

        private void DrawBinding(InputAction action, TextMeshProUGUI[] txts)
        {
            txts[0].SetText(action.name);
            txts[1].SetText(action.GetBindingDisplayString().Replace('\u0001', '\u200b')
                .Replace('\u001b', '\u200b'));    //removing bad characters to get rid of warnings
        }

        private IEnumerator RebindCoroutine(InputAction action, int[] bIndices, int i = 0)
        {
            var dun = false;
            DebugConsole.Log(action.name + ", #" + bIndices[i] + ", binding: "
                + action.bindings[bIndices[i]].effectivePath);
            action.Disable();
            action.PerformInteractiveRebinding(bIndices[i])
                .OnMatchWaitForAnother(0.1f)
                .WithCancelingThrough("<Keyboard>/Escape")
                .OnCancel((op) =>
                {
                    action.Enable();
                    DebugConsole.Log("Remapping canceled");
                    activeRemapScreen.Close();
                    op.Dispose();
                    StopCoroutine(RebindCoroutine(action, bIndices, i));
                })
                .OnComplete((op) =>
                {
                    action.Enable();
                    DebugConsole.Log("Remap finished");
                    i++;
                    dun = true;
                    op.Dispose();
                })
                .Start();
            activeRemapScreen.Open();
            activeRemapScreen.GetComponentInChildren<TextMeshProUGUI>().SetText(ActiveBindText + action.name
                + (action.bindings[bIndices[i]].name.Length <= 1
                ? ""
                : " (" + action.bindings[bIndices[i]].name + ")"));
            DebugConsole.Log("Remapping began, press a key...");
            yield return new WaitUntil(() => dun);
            if (i < bIndices.Length) StartCoroutine(RebindCoroutine(action, bIndices, i));
            else
            {
                StopCoroutine(RebindCoroutine(action, bIndices, i));
                activeRemapScreen.Close();
                RedrawBinding.Invoke();
            }
        }
    }
}