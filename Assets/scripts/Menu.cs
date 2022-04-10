using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using GameExtensions;


public class Menu : MonoBehaviour
{
    public AssetReference scene;
    public AssetReference defaultMaterial;
    public AssetReference terrainMaterial;
    public GameObject loading;

    // this has all the things for the menu like button events, menu mechanics and more...
    public void NewGame()
    {

      PlayerPrefs.SetInt("game_progress", 1);
      //FadeBlack();
      System.GC.Collect();
      defaultMaterial.LoadAssetAsync<Material>().Completed += handle =>
      {
          if (handle.IsDone)
          {
              Debug.Log("Default material has been loaded");
          }
      };
      terrainMaterial.LoadAssetAsync<Material>().Completed += handle => {
          if (handle.IsDone)
          {
              Debug.Log("Terrain material has been loaded");
          }
      }; ;
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
