using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.InputSystem;

namespace GameExtensions.Story
{
    public class CutscenePlayer : MonoBehaviour
    {
        [SerializeField]private GameObject skipText;
        [SerializeField] private int cutsceneId;
        private VideoPlayer video;
        private GameObject background;
        private GameObject screen;
        private Player player;
        private PlayerInput pInput;
        private bool tryingToSkip;
        
        private Vector3 triggerPos;
        private float triggerRadius;
        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("Player")) return;
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
            if(!video.isPlaying) return;
            if(!tryingToSkip)
            {
                skipText.SetActive(true);
                tryingToSkip = true;
            }
            else
            {
                video.Stop();
                System.GC.Collect();
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(triggerPos,triggerRadius);
        }

        private void Start()
        {
            video = GetComponent<VideoPlayer>();
            background = GetComponentInChildren<Image>(true).gameObject;
            screen = GetComponentInChildren<RawImage>(true).gameObject;
            Player.PlayerReady += () =>
            {
                player = Player.Instance;
                player.AddInputAction("Skip",Skip,Player.ActionType.Started);
                pInput = player.PInput;
                var trigger = GetComponentInChildren<SphereCollider>();
                triggerRadius = trigger.radius;
                triggerPos = trigger.center;
            };
        }
    }
}