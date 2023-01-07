using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GameExtensions
{
    public class CutscenePlayer : MonoBehaviour
    {
        private VideoPlayer video;
        private GameObject background;
        private GameObject screen;
        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("Player")) return;
            Debug.Log("player in the player xd");
            video.Prepare();
            background.SetActive(true);
            screen.SetActive(true);
            video.loopPointReached += _ => screen.SetActive(false);
            video.prepareCompleted += source =>
            {
                background.SetActive(false);
                source.Play();
            };
        }

        private void Start()
        {
            video = GetComponent<VideoPlayer>();
            background = GetComponentInChildren<Image>(true).gameObject;
            screen = GetComponentInChildren<RawImage>(true).gameObject;
        }
    }
}