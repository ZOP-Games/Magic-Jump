using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;
using GameExtensions.UI.HUD;

namespace GameExtensions.Story
{
    public class CutscenePlayer : StoryEvent
    {
        [SerializeField] private GameObject skipText;
        [SerializeField] private string targetTag;
        private PlayerInput pInput;
        private Player player;

        private Vector3 triggerPos;
        private float triggerRadius;

        private bool tryingToSkip;

        //todo:implement story progression
        private VideoPlayer video;

        private void Start()
        {
            video = GetComponent<VideoPlayer>();
            /*video = gameObject.AddComponent<VideoPlayer>();
            video.playOnAwake = false;*/
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

        private new void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
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
            skipText.SetActive(false);
            tryingToSkip = false;

            pInput.SwitchCurrentActionMap("Player");
        }

        protected override void DoEvent()
        {
            video.Prepare();
            video.loopPointReached += _ => Close();
            video.prepareCompleted += source =>
            {
                HUDToggler.AskSetHUD(false);
                source.Play();
                pInput.SwitchCurrentActionMap("Cutscene");
            };
        }
    }
}