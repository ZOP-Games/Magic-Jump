using System.Collections.Generic;
using System.Linq;
using GameExtensions.Debug;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// ReSharper disable UseNullPropagation //needed for working null checks

namespace GameExtensions.UI
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private InputActionAsset actions;
        private MenuScreen pause;
        private MenuScreen spells;

        /// <summary>Item1: XP, Item2: Xp threshold, Item3: Level</summary>
        public (int, int, byte) XpInfo { get; private set; }

        [CanBeNull] public MenuScreen ActiveScreen { get; set; }
        public static MenuController Controller { get; private set; }


        public void Start()
        {
            if (Controller is not null) Destroy(this);
            Controller = this;
            DontDestroyOnLoad(gameObject);
            Player.PlayerReady += () =>
            {
                var player = Player.Instance;
                player.AddInputAction("Change", OpenSpell);
                player.AddInputAction("Pause", OpenPause, IInputHandler.ActionType.Canceled);
                pause = FindObjectsOfType<MenuScreen>(true).FirstOrDefault(p => p.CompareTag("Pause"));
                spells = FindObjectsOfType<MenuScreen>(true).FirstOrDefault(o => o.CompareTag("Spell menu"));
                UnityEngine.Debug.Assert(pause is not null, "pause is not assigned in MenuController.");
                UnityEngine.Debug.Assert(spells is not null);
                pause.GetComponentInChildren<Button>().onClick.AddListener(CloseActive);
            };
            actions["Exit"].canceled += _ => CloseActive();
        }

        private void OnDestroy()
        {
            Controller = null;
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
    }
}