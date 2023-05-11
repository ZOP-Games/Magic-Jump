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
    private List<(int width,int height)> resolutions;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown refreshDropdown;
    private readonly Color debugColor = new (0,0.91f,0.8f);
    private readonly List<string> refreshRates = new (){"60","120","144"};

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
        var newRes = resolutions.ElementAt(resNumber);
        if(CurrentResolution.width == newRes.width) return;
        CurrentResolution = new Resolution
        {
            width = newRes.width,
            height = newRes.height,
            refreshRate = CurrentRefreshRate
        };
        DebugConsole.Log("Setting resolution to " + newRes.width+ "x" + newRes.height + ", it is no. " + resolutions.ToList().IndexOf(newRes),debugColor);
        Screen.SetResolution(CurrentResolution.width,CurrentResolution.height,ScreenMode,CurrentRefreshRate);
    }

    public void ChangeRefreshRate(int rateNumber)
    {
        var newRate = int.Parse(refreshRates[rateNumber]);
        if(CurrentRefreshRate == newRate) return;
        CurrentRefreshRate = newRate;
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
        resolutions = Screen.resolutions.GroupBy(r => (r.width,r.height)).Select(r => r.Key).ToList();
        resolutionDropdown.AddOptions(resolutions.Select(r => r.width + " x " + r.height).ToList());
        refreshDropdown.AddOptions(refreshRates);
        var displayInfo = Screen.currentResolution;
        CurrentResolution = displayInfo;
        CurrentRefreshRate = displayInfo.refreshRate;
        resolutionDropdown.value = resolutions.ToList().IndexOf((CurrentResolution.width,CurrentResolution.height));
        refreshDropdown.value = refreshRates.IndexOf(CurrentRefreshRate.ToString());
    }
}
