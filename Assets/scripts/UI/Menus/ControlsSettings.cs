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

        [SerializeField] private InputSettings inputSettings;
        [SerializeField] private Slider sensitivitySlider;
        [SerializeField] private Slider deadzoneSlider;

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
                (CinemachineCore.Instance.GetVirtualCamera(0) as CinemachineVirtualCamera)
                    .GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = Sensitivity;
                (CinemachineCore.Instance.GetVirtualCamera(0) as CinemachineVirtualCamera)
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
            inputSettings.defaultDeadzoneMin = amount;
        }

        public void PassiveStart()
        {
            sensitivitySlider.value = Sensitivity;
        }

        private new void Start()
        {
            remapMenu = GetComponentInChildren<MenuScreen>(true);
            GetComponentInChildren<Button>().onClick.AddListener(() => remapMenu.Open());
            base.Start();
        }
    }
}