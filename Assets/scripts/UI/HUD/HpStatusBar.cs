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
        private Player player;

        private void Start()
        {
            bar.color = Color.green;
            Player.PlayerReady += () =>
            {
                player = Player.Instance;
                player.HealthChanged += UpdateHealth;
            };
        }

        private void UpdateHealth()
        {
            bar.fillAmount = (float)player.Hp / 100;
            hpNumber.SetText(player.Hp + "/100");
            if (player.Hp > 25) return;
            bar.color = Color.red;
        }
    }
}