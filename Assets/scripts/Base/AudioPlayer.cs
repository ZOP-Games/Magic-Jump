using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameExtensions
{
    public class AudioPlayer : MonoBehaviour
    {
        private AudioSource[] sauces;

        public bool ReadyToPlay => sauces is not null;

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
            PlayAll();
        }

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
    }
}