using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GameExtensions.Debug;
using UnityEngine;
using TMPro;

public class VideoSettings : MonoBehaviour
{
    public FullScreenMode ScreenMode { get; private set; }
    public Resolution CurrentResolution { get; private set; }
    public int CurrentRefreshRate { get; private set; } 
    public bool IsVSyncEnabled { get; private set; }
    private Resolution[] resolutions;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown refreshDropdown;
    private readonly Color debugColor = new (0,0.91f,0.8f);

    public void ChangeFullscreenMode(int modeNumber)
    {
        switch (modeNumber)
        {
            case > 2 or < 0:
                DebugConsole.Log("The specified fullscreen mode does not exist.",Color.yellow);
                return;
            case > 1:
                modeNumber = 3;
                break;
        }
        ScreenMode = (FullScreenMode)modeNumber;
        Screen.fullScreenMode = ScreenMode;
    }

    public void ChangeResolution(int resNumber)
    {
        var resString = resolutionDropdown.options[resNumber].text.Split(" x ");
        CurrentResolution = resolutions.FirstOrDefault(r => r.width == int.Parse(resString[0]) &&
                                                   r.height == int.Parse(resString[1]) && 
                                                   r.refreshRate == CurrentRefreshRate);
        Screen.SetResolution(CurrentResolution.width,CurrentResolution.height,ScreenMode,CurrentRefreshRate);
        Debug.Assert(Screen.currentResolution.width != 1080);
        var newRes = Screen.currentResolution;
        DebugConsole.Log("Set resolution to " + newRes.width+ "x" + newRes.height,debugColor);
    }

    public void ChangeRefreshRate(int rateNumber)
    {
        CurrentRefreshRate = int.Parse(refreshDropdown.options[rateNumber].text);
        Application.targetFrameRate = CurrentRefreshRate;
        DebugConsole.Log("Set refresh rate to " + Application.targetFrameRate,debugColor);
    }

    public void ToggleVSync(bool newValue)
    {
        IsVSyncEnabled = newValue;
        QualitySettings.vSyncCount = IsVSyncEnabled ? 1 : 0;
        DebugConsole.Log("Set VSync to " + (QualitySettings.vSyncCount == 1 ? "ON" : "OFF"),debugColor);
    }

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.AddOptions(resolutions.Select(r => r.height + " x " + r.width).Distinct().ToList());
        refreshDropdown.AddOptions(new List<string>{"60","120","144"});
    }
}
