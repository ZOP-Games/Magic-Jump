using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
   //base class for pause screens
   public GameObject gObj;

   protected void Show(GameObject prevScreen)
   {
       prevScreen.SetActive(false);
       gObj.SetActive(true);
   }

   public void Pause()
   {
       Debug.Log("pauseScreen is up");
       gObj.SetActive(true);
       InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
       Time.timeScale = 0;
    }


   public void UnPause()
   {
       Time.timeScale = 1;
       gObj.SetActive(false);
       InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
   }

}
