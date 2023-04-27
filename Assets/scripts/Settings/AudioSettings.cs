using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using GameExtensions.Debug;

namespace GameExtensions.Settings
{
    public class AudioSettings : MonoBehaviour
    {
        public static AudioSettings Instance
        {
            get
            {
                var controller = FindObjectOfType<AudioSettings>();
                if (controller is null) DebugConsole.Log("No Audio Settings detected, no audio adjustments will be possible.");
                return controller;
            }
        }
        [SerializeField] private AudioMixer masterMixer;

        public void ChangeMasterVolume(float amount)
        {
            masterMixer.SetFloat("MasterVolume", amount);
        }

        public void ChangeBackgroundVolume(float amount)
        {
            masterMixer.SetFloat("BgVolume", amount);
        }

        public void ChangeSfxVolume(float amount)
        {
            masterMixer.SetFloat("SFXVolume", amount);
        }

        public void ChangeSpeechVolume(float amount)
        {
            masterMixer.SetFloat("SpeechVolume", amount);
        }

        private void Start()
        {
            if(Instance is not null) Destroy(this);
        }
    }
}