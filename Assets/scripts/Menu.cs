using UnityEngine;
using GameExtensions;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class Menu : MonoBehaviour
{

    public GameObject loading;  //the loading screen
    private VisualElement UIroot;

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
        loading.SetActive(true);    //show loading screen

    }

    public void Continue()
    {
        //checking for save file, showing error if there isn't one, greying out the button would be a better solution though
        if (!PlayerPrefs.HasKey("game_progress"))
        {
            

        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }


    //shows the options menu
    public void Options(GameObject settingsPanel)
        {
            settingsPanel.SetActive(true);
        }


    //presses ok the whatever it is assigned to
    // ReSharper disable once InconsistentNaming
    public void ok(GameObject panel)
    {
        panel.SetActive(false);
    }

    //wrapper for GameHelper.Quit()
    public void Exit()
    {
        GameHelper.Quit();
    }

    private void Start()
    {
        PlayerPrefs.DeleteAll(); //This deletes the player's progress, for testing only!
        UIroot = GetComponent<UIDocument>().rootVisualElement;
        UIroot.Q<Button>("continue").clicked += Continue;
    }

}
