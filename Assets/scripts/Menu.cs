using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;


public class Menu : MonoBehaviour
{
    public AssetReference scene;
    public  GameObject continueButton;
    public GameObject loading;

    // this has all the things for the menu like button events, menu mechanics and more...
    public void NewGame()
    {

      PlayerPrefs.SetInt("game_progress", 1);
      //FadeBlack();
      scene.LoadSceneAsync().Completed += LoadScene;
      void LoadScene(AsyncOperationHandle<SceneInstance> doneHandle)
      {
          if (doneHandle.IsDone)
          {
              Debug.Log("Scene loading is done");
          }
      }

      loading.SetActive(true);

    }

    public void Continue(GameObject noSaveError)
    {
        
        Debug.Log("Continue script is running FeelsWeirdMan");
        if (!PlayerPrefs.HasKey("game_progress"))
        {
            noSaveError.SetActive(true);

        }
        else
        {
            scene.LoadSceneAsync().Completed += LoadScene;

            void LoadScene(AsyncOperationHandle<SceneInstance> doneHandle)
            {
                if (doneHandle.IsDone)
                {
                    Debug.Log("Scene loading is done");
                }
            }
        }
    }



    public void Options(GameObject settingsPanel)
        {
            settingsPanel.SetActive(true);
        }
public void Exit()
    {
        Debug.Log("Exit pressed");
       Application.Quit();
    }

    public void ok(GameObject panel)
    {
        panel.SetActive(false);
    }

    private void Start()
    {
        PlayerPrefs.DeleteAll(); //This deletes the player's progress, for testing only!

        
    }

}
