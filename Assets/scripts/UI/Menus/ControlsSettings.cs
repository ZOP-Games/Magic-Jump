using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using GameExtensions.Debug;
using Cinemachine;

namespace GameExtensions.UI.Menus
{
    public class ControlsSettings : ScreenLayout, ISaveable, IPassiveStart
    {

        [field: SerializeField, HideInInspector] public float Sensitivity { get; private set; }
        [field: SerializeField, HideInInspector] public float Deadzone { get; private set; }
        [field: SerializeField, HideInInspector] public bool IsRumbleEnabled { get; private set; }
        [field: SerializeField, HideInInspector] public bool IsCameraYInverted { get; private set; }

        private Button remapButton;
        [SerializeField] private InputSettings inputSettings;
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private Slider sensitivitySlider;
        [SerializeField] private Slider deadzoneSlider;
        [SerializeField] private Toggle rumbleToggle;
        [SerializeField] private Toggle invertToggle;
        [SerializeField, HideInInspector] private string remapsJson;

        private MenuScreen remapMenu;

        byte ISaveable.Id { get; set; }

        public void OpenRemap()
        {
            if (!isActiveAndEnabled) return;
            remapMenu.Open();
        }

        public void ChangeSensitivity(float amount)
        {
            Sensitivity = amount;
            void ApplySensitivity()
            {
                ((CinemachineVirtualCamera)CinemachineCore.Instance.GetVirtualCamera(0))
                    .GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = Sensitivity;
                ((CinemachineVirtualCamera)CinemachineCore.Instance.GetVirtualCamera(0))
                    .GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = Sensitivity;
            }
            if (CinemachineCore.Instance.VirtualCameraCount < 1)
            {
                Player.PlayerReady -= ApplySensitivity; //remove any previous subscription so it will only be added once
                Player.PlayerReady += ApplySensitivity;
            }
            else
            {
                ApplySensitivity();
            }
        }

        public void ChangeDeadzone(float amount)
        {
            Deadzone = amount;
            inputSettings.defaultDeadzoneMin = Deadzone;
        }

        public void ChangeRumble(bool value)
        {
            IsRumbleEnabled = value;
            if (!IsRumbleEnabled) InputSystem.ResetHaptics();
            else
            {
                InputSystem.ResumeHaptics();
            }
            InputSystem.GetDevice<Gamepad>()?.SetMotorSpeeds(0.5f, 0.5f);
            InputSystem.GetDevice<Gamepad>()?.SetMotorSpeeds(0, 0);
        }

        public void ChangeInvertCamera(bool value)
        {
            IsCameraYInverted = value;
            void ApplyInvert()
            {
                ((CinemachineVirtualCamera)CinemachineCore.Instance.GetVirtualCamera(0))
                    .GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_InvertInput = IsCameraYInverted;
            }
            if (CinemachineCore.Instance.VirtualCameraCount < 1)
            {
                Player.PlayerReady -= ApplyInvert; //remove any previous subscription so it will only be added once
                Player.PlayerReady += ApplyInvert;
            }
            else
            {
                ApplyInvert();
            }

        }

        public void PassiveStart()
        {
            ChangeSensitivity(Sensitivity);
            inputSettings.defaultDeadzoneMin = Deadzone;
            ChangeRumble(IsRumbleEnabled);
            ChangeInvertCamera(IsCameraYInverted);
            var splitMaps = remapsJson.Split(';');
            var iActionsArr = inputActions.ToArray();
            for (var i = 0; i < iActionsArr.Length; i++)
            {
                iActionsArr[i].LoadBindingOverridesFromJson(splitMaps[i]);
            }
        }

        private void OnDisable()
        {
            remapsJson = GetComponentInChildren<ControlRemappingScreen>(true).RebindsJson;
        }

        private new void Start()
        {
            remapButton = firstObj.GetComponent<Button>();
            remapMenu = GetComponentInChildren<MenuScreen>(true);
            remapButton.onClick.AddListener(() => remapMenu.Open());
            sensitivitySlider.value = Sensitivity;
            deadzoneSlider.value = Deadzone;
            rumbleToggle.SetIsOnWithoutNotify(IsRumbleEnabled);
            invertToggle.SetIsOnWithoutNotify(IsCameraYInverted);
            base.Start();
        }
    }
}