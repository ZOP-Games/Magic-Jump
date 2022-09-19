using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using GameExtensions;
public abstract class MenuScreen : MonoBehaviour
{
    protected abstract PlayerInput PInput { get; }

    protected void Quit()
    {
        GameHelper.Quit();
    }
    public void Pause()
    {
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
        PInput.SwitchCurrentActionMap("UI");
        Time.timeScale = 0;
    }
    public void UnPause()
    {
        Time.timeScale = 1;
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
        PInput.SwitchCurrentActionMap("Player");
    }
}
