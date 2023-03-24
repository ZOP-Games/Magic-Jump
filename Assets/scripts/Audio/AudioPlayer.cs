using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameExtensions.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += (_, _) =>
            {
                var source = GetComponent<AudioSource>();
                source.Stop();
                source.Play();
            };
        }
    }
}
