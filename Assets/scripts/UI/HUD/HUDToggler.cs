﻿using GameExtensions.Debug;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameExtensions.UI.HUD
{
    public class HUDToggler : MonoBehaviour
    {
        private static bool targetState;
        private static bool isChecking;
        public bool IsHUDVisible => gameObject.activeSelf;

        public static void AskSetHUD(bool state)
        {
            var instance = FindObjectOfType<HUDToggler>(true);

            void Check(Scene scene, Scene scene1)
            {
                AskSetHUD(state);
            }

            if (state != targetState)
            {
                isChecking = false;
                targetState = state;
            }

            if (instance is not null)
            {

                instance.gameObject.SetActive(state);
                SceneManager.activeSceneChanged -= Check;
                isChecking = false;
            }
            else if (!isChecking)
            {
                SceneManager.activeSceneChanged += Check;
                isChecking = true;
            }
        }

        private void Start()
        {
            if (DebugInputHandler.Instance is null) return;
            DebugInputHandler.Instance.AddInputCallback("[Debug] Toggle HUD", () => AskSetHUD(!IsHUDVisible));
        }
    }
}