using System.Collections.Generic;
using System.Collections;
using GameExtensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameExtensions.UI.Menus
{
    public class MainMenu : MenuScreen
    {

        //public GameObject loading;  //the loading screen, not needed yet
        [SerializeField]private Button continueButton;
        [SerializeField]private Button newGameButton;
        // this has all the things for the menu like button events, menu mechanics and more...
        public void NewGame()
        {
            //FadeBlack() so the transition between scenes won't be visible to the player;
            //loading the game
            SaveManager.SaveAll();
            SceneManager.LoadScene(1);
        }
        public void Continue()
        {
            Debug.Log("continue clicked");
            //checking for save file, showing error if there isn't one, greying out the button would be a better solution though
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
            //PlayerPrefs.DeleteAll();
            //Open();
            ES.SetSelectedGameObject(SaveManager.SaveExists ? continueButton.gameObject : newGameButton.gameObject);
            DontDestroyOnLoad(Controller.gameObject);
            if (!SaveManager.SaveExists) continueButton.interactable = false;
        }
    }
}
