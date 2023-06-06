using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using GameExtensions.Debug;
using TMPro;

namespace GameExtensions.UI.Menus
{
    [Serializable]
    public class AudioSettings : ScreenLayout, ISaveable, IPassiveStart
    {
        public static AudioSettings Instance { get; private set; }

        [field: SerializeField] public bool EnableSubtitles { get; set; } = true;
        [field: SerializeField] public AudioSpeakerMode SpeakerMode { get; set; }
        [field: SerializeField] public float MasterVolume { get; set; }
        [field: SerializeField] public float BgVolume { get; set; }
        [field: SerializeField] public float SpeechVolume { get; set; }
        [field: SerializeField] public float SfxVolume { get; set; }

        private Slider masterVolumeSlider;
        private Slider musicVolumeSlider;
        private Slider speechVolumeSlider;
        private Slider sfxVolumeSlider;
        private TMP_Dropdown speakerModeDropdown;
        private Toggle subtitlesToggle;

        private AudioPlayer player;
        private AudioConfiguration conf;
        [SerializeField] private AudioMixer masterMixer;
        byte ISaveable.Id { get; set; }

        public void ChangeMasterVolume(float amount)
        {
            if (amount < 0.01) MasterVolume = -80;
            else MasterVolume = LinearToLog(amount);
            masterMixer.SetFloat("MasterVolume", MasterVolume);
        }

        public void ChangeBackgroundVolume(float amount)
        {
            BgVolume = LinearToLog(amount);
            masterMixer.SetFloat("BgVolume", BgVolume);
        }

        public void ChangeSfxVolume(float amount)
        {
            SfxVolume = LinearToLog(amount);
            masterMixer.SetFloat("SFXVolume", SfxVolume);
        }

        public void ChangeSpeechVolume(float amount)
        {
            SpeechVolume = LinearToLog(amount);
            masterMixer.SetFloat("SpeechVolume", SpeechVolume);
        }

        public void ChangeSpeakerMode(int modeNumber)
        {
            modeNumber++;
            if (modeNumber > 7)
            {
                DebugConsole.Log("The specified speaker mode does not exist.");
                return;
            }

            modeNumber = Mathf.Clamp(modeNumber, 1, 7);
            ApplySpeakerMode((AudioSpeakerMode)modeNumber);
        }

        private void ApplySpeakerMode(AudioSpeakerMode speakerMode)
        {
            if (speakerMode is 0) speakerMode = AudioSpeakerMode.Mono;
            SpeakerMode = speakerMode;
            conf.speakerMode = SpeakerMode;
            var worked = UnityEngine.AudioSettings.Reset(conf);
            UnityEngine.Debug.Assert(worked, "Couldn't change audio config");
            if(player is not null && player.ReadyToPlay) player.PlayAll();
        }

        private float LinearToLog(float value)
        {
            return Mathf.Log10(value) * 20;
        }

        private float LogToLinear(float value)
        {
            return Mathf.Pow(10, value / 20);
        }

        public void PassiveStart()
        {
            player = FindObjectOfType<AudioPlayer>();
            conf = UnityEngine.AudioSettings.GetConfiguration();
            masterMixer.SetFloat("MasterVolume", MasterVolume);
            masterMixer.SetFloat("BgVolume", BgVolume);
            masterMixer.SetFloat("SFXVolume", SfxVolume);
            masterMixer.SetFloat("SpeechVolume", SpeechVolume);
            ApplySpeakerMode(SpeakerMode);
        }

        private new void Start()
        {
            if (Instance is not null) Destroy(this);
            Instance = this;
            var options = GetComponentsInChildren<Selectable>();
            try
            {
                masterVolumeSlider = (Slider)options[0];
                musicVolumeSlider = (Slider)options[1];
                speechVolumeSlider = (Slider)options[3];
                sfxVolumeSlider = (Slider)options[2];
                speakerModeDropdown = (TMP_Dropdown)options[5];
                subtitlesToggle = (Toggle)options[4];
            }
            catch
            {
                DebugConsole.LogError("The order of the options is incorrect." +
                                      " Please change it either in the script or in the Editor");
            }

            firstObj = masterVolumeSlider.gameObject;
            masterVolumeSlider.value = LogToLinear(MasterVolume);
            musicVolumeSlider.value = LogToLinear(BgVolume);
            sfxVolumeSlider.value = LogToLinear(SfxVolume);
            speechVolumeSlider.value = LogToLinear(SpeechVolume);
            speakerModeDropdown.value = (int)SpeakerMode - 1;
            subtitlesToggle.SetIsOnWithoutNotify(EnableSubtitles);
            base.Start();
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}