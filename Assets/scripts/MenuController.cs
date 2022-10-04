using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

        [CanBeNull] public MenuScreen ActiveScreen { get; set; }
        [SerializeField]private MenuScreen pause;
        public static MenuController Controller => GetInstance();

        public void OpenPause()
        {
            pause.Open();
        }

        public void CloseActive()
        {
            ActiveScreen!.Close();
        }

        public static MenuController GetInstance()
        {
            MenuController controller = null;
            _ = SceneManager.GetActiveScene().GetRootGameObjects().First(o => o.TryGetComponent(out controller));
            return controller;
        }

    }

