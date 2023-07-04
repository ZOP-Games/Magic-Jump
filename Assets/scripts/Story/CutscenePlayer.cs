using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GameExtensions.Story
{
    public class CutscenePlayer : MonoBehaviour
    {
        [SerializeField] private GameObject skipText;
        private GameObject background;
        private PlayerInput pInput;
        private Player player;
        private GameObject screen;

        private Vector3 triggerPos;
        private float triggerRadius;

        private bool tryingToSkip;

        //[SerializeField] private int cutsceneId; todo:implement story progression
        private VideoPlayer video;

        private void Start()
        {
            video = GetComponent<VideoPlayer>();
            background = GetComponentInChildren<Image>(true).gameObject;
            screen = GetComponentInChildren<RawImage>(true).gameObject;
            Player.PlayerReady += () =>
            {
                player = Player.Instance;
                player.AddInputAction("Skip", Skip, IInputHandler.ActionType.Started);
                pInput = player.PInput;
                var trigger = GetComponentInChildren<SphereCollider>();
                triggerRadius = trigger.radius;
                triggerPos = trigger.center;
            };
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(triggerPos, triggerRadius);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            video.Prepare();
            background.SetActive(true);
            screen.SetActive(true);
            video.loopPointReached += _ => Close();
            video.prepareCompleted += source =>
            {
                background.SetActive(false);
                source.Play();
                pInput.SwitchCurrentActionMap("Cutscene");
            };
        }

        private void Skip()
        {
            if (!video.isPlaying) return;
            if (!tryingToSkip)
            {
                skipText.SetActive(true);
                tryingToSkip = true;
            }
            else
            {
                video.Stop();
                GC.Collect();
                Close();
            }
        }

        private void Close()
        {
            screen.SetActive(false);
            skipText.SetActive(false);
            tryingToSkip = false;

            pInput.SwitchCurrentActionMap("Player");
        }
    }
}