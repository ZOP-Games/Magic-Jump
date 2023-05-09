using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameExtensions.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        private AudioSource[] sauces;
        public void PlayAll()
        {
            foreach (var sauce in sauces) sauce.Play();
        }

        public void PauseAll()
        {
            foreach (var sauce in sauces) sauce.Pause();
        }
        
        public void StopAll()
        {
            foreach (var sauce in sauces) sauce.Stop();
        }
        
        public void ResumeAll()
        {
            foreach (var sauce in sauces) sauce.UnPause();
        }
        // Start is called before the first frame update
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            sauces = GetComponents<AudioSource>();
            SceneManager.activeSceneChanged += (_, _) =>
            {
                StopAll();
                PlayAll();
            };
        }
    }
}
