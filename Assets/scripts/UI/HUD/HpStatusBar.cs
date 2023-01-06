using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameExtensions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace GameExtensions.UI.HUD
{
    public class HpStatusBar : MonoBehaviour
    {
        // Start is called before the first frame update
        private Image bar;
        private Player player;
        private float maxWidth;
        [SerializeField]private TextMeshProUGUI hpNumber;
        private void Start()
        {
            bar = transform.GetChild(1).GetComponent<Image>();
            maxWidth = bar.rectTransform.localScale.x;
            Player.PlayerReady += () =>
            {
                player = Player.Instance;
                player.HealthChanged += UpdateHealth;
            };
        }

        private void UpdateHealth()
        {
            var scale = bar.rectTransform.localScale;
            scale.x = maxWidth*((float)player.Hp / 100);
            bar.rectTransform.localScale = scale;
            hpNumber.SetText(player.Hp + "/100");
            if(player.Hp > 25) return;
            bar.color = Color.red;
        }
    }
}