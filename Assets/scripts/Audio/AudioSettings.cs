using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using GameExtensions.Debug;

namespace GameExtensions.Audio
{
    public class AudioSettings : MonoBehaviour
    {
        public static AudioSettings Instance { get; private set; }

        public bool EnableSubtitles { get; set; } = true;
        public AudioSpeakerMode SpeakerMode { get; private set; }
        public float MasterVolume { get; set; }

        public float BgVolume { get; set; }

        public float SpeechVolume { get; set; }

        public float SfxVolume { get; set; }

        private AudioPlayer player;
        private AudioConfiguration conf;
        [SerializeField] private AudioMixer masterMixer;


        public void ChangeMasterVolume(float amount)
        {
            if (amount == 0) MasterVolume = -80;
            MasterVolume = Mathf.Log10(amount) *  20;
            masterMixer.SetFloat("MasterVolume", MasterVolume);
        }

        public void ChangeBackgroundVolume(float amount)
        {
            BgVolume = Mathf.Log10(amount) *  20;
            masterMixer.SetFloat("BgVolume", BgVolume);
        }

        public void ChangeSfxVolume(float amount)
        {
            SfxVolume = Mathf.Log10(amount) *  20;
            masterMixer.SetFloat("SFXVolume", SfxVolume);
        }

        public void ChangeSpeechVolume(float amount)
        {
            SpeechVolume = Mathf.Log10(amount) *  20;
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
            ApplySpeakerMode((AudioSpeakerMode)modeNumber);
        }

        private void ApplySpeakerMode(AudioSpeakerMode speakerMode)
        {
            //player.PauseAll();
            SpeakerMode = speakerMode;
            conf.speakerMode = SpeakerMode;
            var worked = UnityEngine.AudioSettings.Reset(conf);
            UnityEngine.Debug.Assert(worked, "Couldn't change audio config");
            DebugConsole.Log("Changed speaker mode to: " + SpeakerMode);
            player.PlayAll();
        }

        private void Start()
        {
            if (Instance is not null) Destroy(this);
            Instance = this;
            player = FindObjectOfType<AudioPlayer>();
            conf = UnityEngine.AudioSettings.GetConfiguration();
            ApplySpeakerMode(UnityEngine.AudioSettings.driverCapabilities);
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}