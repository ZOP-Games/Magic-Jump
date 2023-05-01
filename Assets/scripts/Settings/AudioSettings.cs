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

        public bool EnableSubtitles { get; set; } = true;
        public AudioSpeakerMode SpeakerMode { get; private set; }
        public float MasterVolume { get; set; } = 0;

        public float BgVolume { get; set; } = 0;

        public float SpeechVolume { get; set; } = 0;

        public float SfxVolume { get; set; } = 0;

        [SerializeField]private AudioMixer masterMixer;
        

        public void ChangeMasterVolume(float amount)
        {
            MasterVolume += amount;
            masterMixer.SetFloat("MasterVolume", MasterVolume);
        }

        public void ChangeBackgroundVolume(float amount)
        {
            BgVolume += amount; 
            masterMixer.SetFloat("BgVolume", BgVolume);
        }

        public void ChangeSfxVolume(float amount)
        {
            SfxVolume += amount; 
            masterMixer.SetFloat("SFXVolume", SfxVolume);
        }

        public void ChangeSpeechVolume(float amount)
        {
            SpeechVolume += amount;
            masterMixer.SetFloat("SpeechVolume", SpeechVolume);
        }

        public void ChangeSpeakerMode(int modeNumber)
        {
            if(modeNumber > 7) DebugConsole.Log("The specified speaker mode does not exist.");
            SpeakerMode = (AudioSpeakerMode)modeNumber;
            UnityEngine.AudioSettings.speakerMode = SpeakerMode;
        }

        private void Start()
        {
            if(Instance is not null) Destroy(this);
        }
    }
}