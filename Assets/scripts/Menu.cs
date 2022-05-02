using UnityEngine;
using GameExtensions;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{

    public GameObject loading;

    // this has all the things for the menu like button events, menu mechanics and more...
    public void NewGame()
    {

        PlayerPrefs.SetInt("game_progress", 1);
      //FadeBlack();
        System.GC.Collect();

        SceneManager.LoadScene(1);
        loading.SetActive(true);

    }

    public void Continue(GameObject noSaveError)
    {
        
        if (!PlayerPrefs.HasKey("game_progress"))
        {
            noSaveError.SetActive(true);

        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }



    public void Options(GameObject settingsPanel)
        {
            settingsPanel.SetActive(true);
        }


    public void ok(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void Exit()
    {
        GameHelper.Quit();
    }

    private void Start()
    {
        PlayerPrefs.DeleteAll(); //This deletes the player's progress, for testing only!

        
    }

}
