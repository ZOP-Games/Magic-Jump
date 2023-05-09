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
    private Resolution[] resolutions;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown refreshDropdown;

    public void ChangeFullscreenMode(int modeNumber)
    {
        switch (modeNumber)
        {
            case > 2:
                DebugConsole.Log("The specified fullscreen mode does not exist.");
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
        CurrentResolution = resolutions.First(r => r.width == int.Parse(resString[0]) &&
                                                   r.height == int.Parse(resString[1]) && 
                                                   r.refreshRate == CurrentRefreshRate);
        ApplyResolution();
    }

    public void ChangeRefreshRate(int rateNumber)
    {
        CurrentRefreshRate = int.Parse(refreshDropdown.options[rateNumber].text);
        ApplyResolution();
    }

    private void ApplyResolution()
    {
        Screen.SetResolution(CurrentResolution.width, CurrentResolution.height, ScreenMode, CurrentRefreshRate);
    }

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.AddOptions(resolutions.Select(r => r.height + " x " + r.width).Distinct().ToList());
        refreshDropdown.AddOptions(resolutions.Select(r => r.refreshRate.ToString()).Distinct().ToList());
    }
}
