using GameExtensions.Debug;
using JetBrains.Annotations;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;

namespace GameExtensions
{
    internal class Rumbler : MonoBehaviour
    {
        public static Rumbler Instance { get; private set; }
        [CanBeNull] private IDualMotorRumble device;
        private bool canRumble;

        public void Rumble(float smallMotor, float largeMotor)
        {
            if (!canRumble) return;
            device?.SetMotorSpeeds(smallMotor, largeMotor);
        }

        public void StopRumbling()
        {
            if (!canRumble) return;
            device?.SetMotorSpeeds(0, 0);
        }

        private void Start()
        {
            if (Instance is not null) Destroy(this);
            else Instance = this;
            device = InputSystem.devices.OfType<IDualMotorRumble>().FirstOrDefault();
            if (device is null)
            {
                DebugConsole.Log(
                                    "No rumble device detected," +
                                    " rumble will be disabled until a rumble device is connected.",
                                    DebugConsole.WarningColor);
                InputSystem.PauseHaptics();

            }
            InputSystem.onDeviceChange += (d, c) =>
            {
                if (d is not IDualMotorRumble rumbleDevice) return;
                switch (c)
                {
                    case InputDeviceChange.Added or InputDeviceChange.Reconnected:
                        device ??= rumbleDevice;
                        break;
                    case InputDeviceChange.Disconnected or InputDeviceChange.Removed:
                        device = null;
                        break;
                }
            };
            Player.PlayerReady += () =>
            {
                var pInput = Player.Instance.PInput;
                void CheckRumble()
                {
                    canRumble = pInput.currentControlScheme == "Controller";
                }
                CheckRumble();
                pInput.controlsChangedEvent.AddListener(_ => CheckRumble());
            };
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}