using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GameExtensions.UI.Layouts
{
    public class AudioSettingsLayout : ScreenLayout
    {
       [SerializeField]private AudioMixer masterMixer;

        private new void OnEnable()
        {
            base.OnEnable();
        }

        private new void Start()
        {
            base.Start();
        }

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
    }
}
