using System;
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
        [SerializeField]private TextMeshProUGUI hpNumber;
        
        private void UpdateHealth()
        {
            bar.fillAmount = (float)player.Hp / 100;
            hpNumber.SetText(player.Hp + "/100");
            if(player.Hp > 25) return;
            bar.color = Color.red;
        }

        private void Start()
        {
            bar = transform.GetChild(1).GetComponent<Image>();
            Player.PlayerReady += () =>
            {
                player = Player.Instance;
                player.HealthChanged += UpdateHealth;
            };
        }
    }
}