using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseScreen : MonoBehaviour
{
   //base class for pause screens
   public GameObject gObj;

   //displays the other menu screens
   protected void Show(GameObject prevScreen)
   {
       prevScreen.SetActive(false);
       gObj.SetActive(true);
   }

   //pause logic: freeze the game and fixes InputSystem so it will still work
   public void Pause()
   {
       Debug.Log("pauseScreen is up");
       gObj.SetActive(true);
       InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
       Time.timeScale = 0;
    }


   //unpause logic: basically Pause() but backwards
   public void UnPause()
   {
       Time.timeScale = 1;
       gObj.SetActive(false);
       InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
   }

}
