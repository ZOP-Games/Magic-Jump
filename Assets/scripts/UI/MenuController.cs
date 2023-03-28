using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using GameExtensions.Debug;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

// ReSharper disable UseNullPropagation //needed for working null checks

namespace GameExtensions.UI
{
    public class MenuController : MonoBehaviour
    {
        /// <summary>Item1: XP, Item2: Xp threshold, Item3: Level</summary>
        public (int, int, byte) XpInfo { get; private set; }
        [CanBeNull] public MenuScreen ActiveScreen { get; set; }
        private MenuScreen pause;
        private MenuScreen spells;
        [SerializeField] private InputActionAsset actions;
        public static MenuController Controller {
            get
            {
                var controller = FindObjectsOfType<MenuController>().FirstOrDefault();
                if (controller is null) DebugConsole.LogError("u need a menuController u idoit");
                return controller;
            }
        }

        public void OpenPause()
        {
            var player = Player.Instance;
            XpInfo = (player.Xp, player.XpThreshold, player.Lvl);
            pause.Open();
        }

        public void OpenSpell()
        {
            spells.Open();
        }

        public void CloseActive()
        {
            if (ActiveScreen is not null) ActiveScreen.Close();
        }


        public void Start()
        {
            Player.PlayerReady += () =>
            {
                var player = Player.Instance;
                player.AddInputAction("Change", OpenSpell);
                player.AddInputAction("Pause", OpenPause);
                player.AddInputAction("Exit", CloseActive);
                pause = FindObjectsOfType<MenuScreen>(true).FirstOrDefault(p => p.CompareTag("Pause"));
                spells = FindObjectsOfType<MenuScreen>(true).FirstOrDefault(o => o.CompareTag("Spell menu"));
                UnityEngine.Debug.Assert(pause is not null);
                UnityEngine.Debug.Assert(spells is not null);
                pause!.GetComponentInChildren<Button>().onClick.AddListener(CloseActive);
            };
            actions["Exit"].canceled += _ => CloseActive();
        }

        
    }
}

