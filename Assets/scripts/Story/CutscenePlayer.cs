using GameExtensions.Debug;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;
using GameExtensions.UI.HUD;
using UnityEngine.AddressableAssets;

namespace GameExtensions.Story
{
    [System.Serializable]
    internal class VideoClipReference : AssetReferenceT<VideoClip>
    {
        public VideoClipReference(string guid) : base(guid)
        {
        }
    }

    public class CutscenePlayer : StoryEvent
    {
        [SerializeField] private GameObject skipText;
        [SerializeField] private string targetTag;
        [SerializeField] private VideoClipReference video;
        [SerializeField] private VideoClipReference linuxVideo;
        private PlayerInput pInput;
        private Player player;

        private Vector3 triggerPos;
        private float triggerRadius;

        private bool tryingToSkip;
        private bool isUsingWebm;

        //todo:implement story progression
        private VideoPlayer videoPlayer;

        private void Start()
        {
            videoPlayer = GetComponent<VideoPlayer>();
            videoPlayer.source = VideoSource.VideoClip;
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
            if (!other.CompareTag(targetTag)) return;
            base.OnTriggerEnter(other);
        }

        private void Skip()
        {
            if (!videoPlayer.isPlaying) return;
            if (!tryingToSkip)
            {
                skipText.SetActive(true);
                tryingToSkip = true;
            }
            else
            {
                videoPlayer.Stop();
                System.GC.Collect();
                Close();
            }
        }

        private void Close()
        {
            skipText.SetActive(false);
            tryingToSkip = false;
            HUDToggler.AskSetHUD(true);
            if(isUsingWebm) linuxVideo?.ReleaseAsset();
            else video.ReleaseAsset();
            pInput.SwitchCurrentActionMap("Player");
        }

        protected override void DoEvent()
        {
#if UNITY_LINUX
            linuxVideo.LoadAssetAsync().Complete += op =>  videoplayer.clip = op.Result;
            isUsingWebm = true;
#else
            video.LoadAssetAsync().Completed += op => videoPlayer.clip = op.Result;
#endif    
            videoPlayer.Prepare();
            DebugConsole.Log("Loading video..");
            videoPlayer.loopPointReached += _ => Close();
            videoPlayer.prepareCompleted += source =>
            {
                HUDToggler.AskSetHUD(false);
                source.Play();
                pInput.SwitchCurrentActionMap("Cutscene");
            };
        }
    }
}