using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GameExtensions.Debug;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.UI;
using UnityEngine.VFX;

public class VideoSettings : MonoBehaviour
{
    public FullScreenMode ScreenMode { get; private set; }
    public Resolution CurrentResolution { get; private set; }
    public int CurrentRefreshRate { get; private set; } 
    public bool IsVSyncEnabled { get; private set; }
    public AntialiasingMode AntiAliasing { get; private set; }
    public bool IsUsingAnisoFiltering { get; private set; }
    //todo: world quality
    public byte ModelQuality { get; private set; }

    private List<(int width,int height)> resolutions;
    private UniversalAdditionalCameraData cameraData;

    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown refreshDropdown;
    [SerializeField] private Toggle vSyncToggle;
    [SerializeField] private TMP_Dropdown aaDropdown;
    [SerializeField] private TMP_Dropdown worldDropdown;
    [SerializeField] private TMP_Dropdown modelDropdown;
    [SerializeField] private Toggle filterToggle;

    private readonly List<string> refreshRates = new (){"60","120","144"};
    private readonly (float bias, int max) lowLODSettings = (0.75f, 2);
    private readonly (float bias, int max) mediumLODSettings = (1, 2);
    private readonly (float bias, int max) highLODSettings = (1, 1);
    private readonly (float bias, int max) ultraLODSettings = (2, 0);


    public void ChangeFullscreenMode(int modeNumber)
    {
        switch (modeNumber)
        {
            case > 2 or < 0:
                DebugConsole.Log("The specified fullscreen mode does not exist.",DebugConsole.WarningColor);
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
        var (width, height) = resolutions.ElementAt(resNumber);
        if(CurrentResolution.width == width) return;
        CurrentResolution = new Resolution
        {
            width = width,
            height = height,
            refreshRate = CurrentRefreshRate
        };
        Screen.SetResolution(CurrentResolution.width,CurrentResolution.height,ScreenMode,CurrentRefreshRate);
    }

    public void ChangeRefreshRate(int rateNumber)
    {
        var newRate = int.Parse(refreshRates[rateNumber]);
        if(CurrentRefreshRate == newRate) return;
        CurrentRefreshRate = newRate;
        Application.targetFrameRate = CurrentRefreshRate;
    }

    public void ToggleVSync(bool newValue)
    {
        IsVSyncEnabled = newValue;
        QualitySettings.vSyncCount = IsVSyncEnabled ? 1 : 0;
    }

    public void ChangeAntiAliasing(int setting)
    {
        AntiAliasing = (AntialiasingMode) Mathf.Clamp(setting,0,2);
        cameraData.antialiasing = AntiAliasing;
        cameraData.antialiasingQuality = AntialiasingQuality.High;
    }

    public void ChangeWorldQuality(int level)
    {
        DebugConsole.Log("This feature is not implemented yet. :(",DebugConsole.TestColor);
        throw new NotImplementedException();
    }

    public void ChangeModelQuality(int level)
    {
        ModelQuality = (byte)Mathf.Clamp(level, 0, 3);
        switch (ModelQuality)
        {
            case 0:
                QualitySettings.SetLODSettings(lowLODSettings.bias,lowLODSettings.max);
                break;
            case 1:
                QualitySettings.SetLODSettings(mediumLODSettings.bias,mediumLODSettings.max);
                break;
            case 2:
                QualitySettings.SetLODSettings(highLODSettings.bias,highLODSettings.max);
                break;
            case 3:
                QualitySettings.SetLODSettings(ultraLODSettings.bias,ultraLODSettings.max);
                break;

        }
    }

    public void ChangeTextureFiltering(bool newValue)
    {
        IsUsingAnisoFiltering = newValue;
        QualitySettings.anisotropicFiltering =
            IsUsingAnisoFiltering ? AnisotropicFiltering.ForceEnable : AnisotropicFiltering.Disable;
    }

    public void ChangeVfxQuality()
    {
        DebugConsole.Log("This feature is not implemented yet. :(",DebugConsole.TestColor);
        throw new NotImplementedException();
    }

    private void Start()
    {
        cameraData = FindObjectOfType<UniversalAdditionalCameraData>();
        if (cameraData is null)
        {
            DebugConsole.LogError("Universal Additional Camera Data cannot be found.");
            return;
        }
        resolutions = Screen.resolutions.GroupBy(r => (r.width,r.height)).Select(r => r.Key).ToList();
        resolutionDropdown.AddOptions(resolutions.Select(r => r.width + " x " + r.height).ToList());
        refreshDropdown.AddOptions(refreshRates);
        var displayInfo = Screen.currentResolution;
        CurrentResolution = displayInfo;
        CurrentRefreshRate = displayInfo.refreshRate;
        IsVSyncEnabled = QualitySettings.vSyncCount == 1;
        AntiAliasing = cameraData.antialiasing;
        IsUsingAnisoFiltering = QualitySettings.anisotropicFiltering == AnisotropicFiltering.ForceEnable;
        resolutionDropdown.value = resolutions.ToList().IndexOf((CurrentResolution.width,CurrentResolution.height));
        refreshDropdown.value = refreshRates.IndexOf(CurrentRefreshRate.ToString());
        vSyncToggle.SetIsOnWithoutNotify(IsVSyncEnabled);
        aaDropdown.value = (int)AntiAliasing;
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += (_, _) =>
        {
            var cd = FindObjectOfType<UniversalAdditionalCameraData>();
            if (cd is null)
            {
                DebugConsole.Log("Universal Additional Camera Data couldn't be found in this scene. Anti-aliasing settings won't be applied.",DebugConsole.WarningColor);
                return;
            }
            cd.antialiasing = AntiAliasing;
        };
        filterToggle.SetIsOnWithoutNotify(IsUsingAnisoFiltering);
    }
}
