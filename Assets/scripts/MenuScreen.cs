using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using GameExtensions;
public class MenuScreen : MonoBehaviour
{
   //script for managing menus
   //private GameObject gObj;
   private VisualElement uiRoot;
   [SerializeField]private PlayerInput pInput;
   //menu logic: freeze the game and fixes InputSystem so it will still work
   public void Pause()
   {
       //gObj = gObj != null ? gObj : gameObject;
       Debug.Log("pauseScreen is up");
       //gObj.SetActive(true);
       uiRoot.RemoveFromClassList("hidden");
       Debug.Log("focused element: " + uiRoot.focusController.focusedElement);
       uiRoot.Q<Button>("menu-resume").Focus();
       InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
       pInput.SwitchCurrentActionMap("UI");
       Time.timeScale = 0;
    }


   //unpause logic: basically Pause() but backwards
   public void UnPause()
   {
       //if (!gObj.activeSelf) return;
       Time.timeScale = 1;
       //gObj.SetActive(false);
       uiRoot.AddToClassList("hidden");
       InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
       pInput.SwitchCurrentActionMap("Player");
   }


   private void Awake()
   {
       uiRoot = GetComponent<UIDocument>().rootVisualElement.Children().First();
       uiRoot.Q<Button>("menu-resume").clicked += UnPause;
       //uiRoot.Q<Button>("menu-resume").RegisterCallback<NavigationMoveEvent>(Navigate);
       //uiRoot.Q<Button>("menu-options").clicked += 
       uiRoot.Q<Button>("menu-exit").clicked += GameHelper.Quit;
   }
}
