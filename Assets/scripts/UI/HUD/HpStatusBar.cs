using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameExtensions.UI.HUD
{
    public class HpStatusBar : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private Image bar;
        [SerializeField] private TextMeshProUGUI hpNumber;
        private Animator anim;
        private Player player;

        private readonly int helfPmHash = Animator.StringToHash("helf");

        private void Start()
        {
            Player.PlayerReady += () =>
            {
                player = Player.Instance;
                player.HealthChanged += UpdateHealthBar;
            };
            anim = bar.GetComponent<Animator>();
        }

        private void UpdateHealthBar()
        {
            anim.SetInteger(helfPmHash,Mathf.CeilToInt(player.Hp/20f));
            hpNumber.SetText(player.Hp + "/100");
        }
    }
}