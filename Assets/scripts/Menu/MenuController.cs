using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

// ReSharper disable UseNullPropagation //needed for working null checks

public class MenuController : MonoBehaviour
{

        [CanBeNull] public MenuScreen ActiveScreen { get; set; }
        private MenuScreen pause;
        public static MenuController Controller => GetInstance();

        public void OpenPause()
        {
            pause.Open();
        }

        public void CloseActive()
        {
            if(ActiveScreen is not null)ActiveScreen.Close();
        }

        private static MenuController GetInstance()
        {
            var controller = FindObjectsOfType<MenuController>().FirstOrDefault();
            if (controller is null) Debug.LogError("u need a menuController u idoit");
            return controller;
        }

        public void Start()
        {
            pause = FindObjectOfType<PauseScreen>(true);
            if(pause is null) Debug.LogError("where is pause??");
        }
}

