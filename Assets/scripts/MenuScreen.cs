using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class MenuScreen : MonoBehaviour
{
   //script for managing menus
   private GameObject gObj;
   private UIDocument doc;

   //menu logic: freeze the game and fixes InputSystem so it will still work
   public void Pause()
   {
       gObj = gObj != null ? gObj : gameObject;
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

   private void Awake()
   {
       doc = GetComponent<UIDocument>();
       var playerName = doc.rootVisualElement.Q<Label>("status-name-content");
       playerName.text = "hello";
   }
}
