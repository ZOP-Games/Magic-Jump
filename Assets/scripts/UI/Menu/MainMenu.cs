using System.Collections.Generic;
using System.Collections;
using GameExtensions;
using GameExtensions.Debug;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameExtensions.UI.Menus
{
    public class MainMenu : OptionsParentScreen
    {

        //public GameObject loading;  //the loading screen, not needed yet
        [SerializeField] private Button continueButton;
        [SerializeField] private Button newGameButton;
        // this has all the things for the menu like button events, menu mechanics and more...

        public override void Open()
        {
        }

        public override void Close()
        {
            //todo:highlight exit button
        }

        public void NewGame()
        {
            //FadeBlack() so the transition between scenes won't be visible to the player;
            //loading the game
            SaveManager.SaveAll();
            SceneManager.LoadScene(1);
        }
        public void Continue()
        {
            DebugConsole.Log("continue clicked");
            if (!SaveManager.SaveExists) return;
            SceneManager.LoadScene(1);
            Player.PlayerReady += SaveManager.LoadAll;
        }

        public void Quit()
        {
            GameHelper.Quit();
        }


        private void Start()
        {
            ES.SetSelectedGameObject(SaveManager.SaveExists ? continueButton.gameObject : newGameButton.gameObject);
            if (!SaveManager.SaveExists) continueButton.interactable = false;
        }
    }
}
