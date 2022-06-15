using UnityEngine;
using GameExtensions;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class Menu : MonoBehaviour
{

    //public GameObject loading;  //the loading screen, not needed yet
    private VisualElement uiRoot;
    private TemplateContainer noSaveError;



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
            noSaveError.RemoveFromClassList("hidden");

        }
        else
        {
            System.GC.Collect();
            SceneManager.LoadScene(1);
        }
    }


    //shows the options menu
    public void Options()
    {
            
    }


    //presses ok and closes the error window
    // ReSharper disable once InconsistentNaming
    public void ok()
    {
       noSaveError.AddToClassList("hidden");
    }


    private void Start()
    {
        PlayerPrefs.DeleteAll(); //This deletes the player's progress, for testing only!
        uiRoot = GetComponent<UIDocument>().rootVisualElement;
        noSaveError = uiRoot.Q<TemplateContainer>("NoSaveError");
        uiRoot.Q<Button>("continue").clicked += Continue;
        uiRoot.Q<Button>("ok").clicked += ok;
        uiRoot.Q<Button>("newgame").clicked += NewGame;
        uiRoot.Q<Button>("options").clicked += Options;
        uiRoot.Q<Button>("exit").clicked += GameHelper.Quit;
    }

}
