using System.Collections;
using System.Linq;
using UnityEngine;
using GameExtensions;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class MainMenu : MenuScreen
{

    //public GameObject loading;  //the loading screen, not needed yet
    private VisualElement uiRoot;



    // this has all the things for the menu like button events, menu mechanics and more...
    public void NewGame()
    {
        //start a new save file
        PlayerPrefs.SetInt("game_progress", 1);
      //FadeBlack() so the transition between scenes won't be visible to the player;
      //run GC so memory will be ready to load the game
        System.GC.Collect();
        //loading the game
        SceneManager.LoadScene(1);
    }

    public void Continue()
    {
        Debug.Log("continue clicked");
        //checking for save file, showing error if there isn't one, greying out the button would be a better solution though
        if (!PlayerPrefs.HasKey("game_progress"))
        {
            Debug.Log("focused element" + uiRoot.focusController.focusedElement);
        }
        else
        {
            System.GC.Collect();
            SceneManager.LoadScene(1);
        }
    }

    public void Quit()
    {
        GameHelper.Quit();
    }


}
