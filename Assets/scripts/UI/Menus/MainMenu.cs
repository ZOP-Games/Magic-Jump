using GameExtensions.Debug;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace GameExtensions.UI.Menus
{
    public class MainMenu : OptionsParentScreen
    {
        //public GameObject loading;  //the loading screen, not needed yet
        [SerializeField] private Button continueButton;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private AssetReference hubSceneReference;


        private void Start()
        {
            Open();
        }

        public override void Open()
        {
            ES.SetSelectedGameObject(SaveManager.SaveExists ? continueButton.gameObject : newGameButton.gameObject);
            if (!SaveManager.SaveExists) continueButton.interactable = false;
            Controller.ActiveScreen = this;
        }

        public override void Close()
        {
            if (exitButton is not null) ES.SetSelectedGameObject(exitButton.gameObject);    //just to avoid errors when pausing
        }

        public void NewGame()
        {
            //FadeBlack() so the transition between scenes won't be visible to the player
            //loading the game
            SaveManager.SaveAll();
            hubSceneReference.LoadSceneAsync().Completed += op =>
            {
                op.Result.ActivateAsync();
            };
        }

        public void Continue()
        {
            DebugConsole.Log("continue clicked");
            if (!SaveManager.SaveExists) return;
            hubSceneReference.LoadSceneAsync().Completed += op =>
            {
                op.Result.ActivateAsync();
            };
            Player.PlayerReady += SaveManager.LoadAll;
        }

        public void Quit()
        {
            GameHelper.Quit();
        }
    }
}