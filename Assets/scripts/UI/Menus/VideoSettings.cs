using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using GameExtensions.Debug;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ReSharper disable Unity.RedundantHideInInspectorAttribute

namespace GameExtensions.UI.Menus
{
    [Serializable]
    public class VideoSettings : ScreenLayout, ISaveable, IAwakeStart, ISerializationCallbackReceiver
    {
        [field: SerializeField]
        [field: HideInInspector]
        public FullScreenMode ScreenMode { get; private set; }

        [field: SerializeField]
        [field: HideInInspector]
        public float RenderScale { get; private set; }

        [field: SerializeField]
        [field: HideInInspector]
        public int CurrentRefreshRate { get; private set; }

        [field: SerializeField]
        [field: HideInInspector]
        public bool IsSsaoEnabled { get; private set; }

        [field: SerializeField]
        [field: HideInInspector]
        public bool IsVSyncEnabled { get; private set; }

        [field: SerializeField]
        [field: HideInInspector]
        public AntialiasingMode AntiAliasing { get; private set; }

        [field: SerializeField]
        [field: HideInInspector]
        public bool IsUsingAnisoFiltering { get; private set; }

        [field: SerializeField]
        [field: HideInInspector]
        public byte WorldQualityLevel { get; private set; }

        [field: SerializeField]
        [field: HideInInspector]
        public byte ModelQualityLevel { get; private set; }

        [field: SerializeField]
        [field: HideInInspector]
        public byte ShadowQuality { get; private set; }

        [field: SerializeField]
        [field: HideInInspector]
        public float Brightness { get; private set; }

        [SerializeField] [HideInInspector] private int currentHeight;
        [SerializeField] [HideInInspector] private int currentWidth;
        [SerializeField] private UniversalRenderPipelineAsset urpAsset;
        [SerializeField] private ScriptableRendererFeature ssao;

        [SerializeField] private TMP_Dropdown screenDropdown;
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private Slider scaleSlider;
        [SerializeField] private TMP_Dropdown refreshDropdown;
        [SerializeField] private Toggle ssaoToggle;
        [SerializeField] private Toggle vSyncToggle;
        [SerializeField] private TMP_Dropdown aaDropdown;
        [SerializeField] private TMP_Dropdown worldDropdown;
        [SerializeField] private TMP_Dropdown modelDropdown;
        [SerializeField] private Toggle filterToggle;
        [SerializeField] private TMP_Dropdown shadowDropdown;
        [SerializeField] private TMP_Dropdown vfxDropdown;
        [SerializeField] private Slider brightnessSlider;

        private readonly LightingQuality[] lightingQualities =
        {
            new(0, 50, 4), //low
            new(2, 50, 4), //medium
            new(4, 75, 2), //high
            new(6, 100, 0) //ultra
        };

        // ReSharper disable once InconsistentNaming
        private readonly (float bias, int max)[] LODSettings =
        {
            (0.75f, 1), //low
            (1, 1), //medium
            (1, 0), //high
            (2, 0) //ultra
        };

        private readonly List<string> refreshRates = new() { "60", "120", "144" };

        private readonly WorldQuality[] worldQualities =
        {
            new(200, 2, 512), //low
            new(1000, 2, 512), //medium
            new(2500, 1, 1024), //high
            new(5000, 0, 2048) //ultra
        };

        private Camera cam;
        private UniversalAdditionalCameraData cameraData;

        private List<(int width, int height)> resolutions;
        public static VideoSettings Instance { get; private set; }

        public Resolution CurrentResolution { get; private set; }

        // ReSharper disable once Unity.RedundantSerializeFieldAttribute
        [field: SerializeField]
        [field: HideInInspector]
        public byte VFXQuality
        {
            get => throw new NotImplementedException();
            private set => throw new NotImplementedException();
        }

        private new void Start()
        {
            base.Start();
            if (Instance is not null) Destroy(this);
            else Instance = this;
            resolutions = Screen.resolutions.GroupBy(r => (r.width, r.height)).Select(r => r.Key).ToList();
            resolutionDropdown.AddOptions(resolutions.Select(r => r.width + " x " + r.height).ToList());
            refreshDropdown.AddOptions(Screen.resolutions.Select(r => r.refreshRate.ToString()).Distinct().ToList());
            screenDropdown.value = ScreenMode == FullScreenMode.Windowed ? 2 : (int)ScreenMode;
            resolutionDropdown.value = resolutions.IndexOf((CurrentResolution.width, CurrentResolution.height));
            scaleSlider.value = RenderScale;
            refreshDropdown.value = refreshRates.IndexOf(CurrentRefreshRate.ToString());
            ssaoToggle.SetIsOnWithoutNotify(IsSsaoEnabled);
            vSyncToggle.SetIsOnWithoutNotify(IsVSyncEnabled);
            aaDropdown.value = (int)AntiAliasing;
            worldDropdown.value = WorldQualityLevel;
            modelDropdown.value = ModelQualityLevel;
            filterToggle.SetIsOnWithoutNotify(IsUsingAnisoFiltering);
            shadowDropdown.value = ShadowQuality;
            brightnessSlider.value = Brightness;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void AwakeStart()
        {
            cameraData = FindObjectOfType<UniversalAdditionalCameraData>();
            if (cameraData is null)
            {
                DebugConsole.LogError("Universal Additional Camera Data cannot be found.");
                return;
            }

            cam = cameraData.GetComponent<Camera>();
            Screen.fullScreenMode = ScreenMode;
            Screen.SetResolution(CurrentResolution.width, CurrentResolution.height, ScreenMode, CurrentRefreshRate);
            urpAsset.renderScale = RenderScale;
            Application.targetFrameRate = CurrentRefreshRate;
            ssao.SetActive(IsSsaoEnabled);
            QualitySettings.vSyncCount = IsVSyncEnabled ? 1 : 0;
            var quality = worldQualities[WorldQualityLevel];
            QualitySettings.masterTextureLimit = quality.maxTextureSize;
            QualitySettings.streamingMipmapsMemoryBudget = quality.textureMemoryBudget;
            ApplyFarClipping();
            var (bias, max) = LODSettings[ModelQualityLevel];
            QualitySettings.SetLODSettings(bias, max);
            QualitySettings.anisotropicFiltering =
                IsUsingAnisoFiltering ? AnisotropicFiltering.ForceEnable : AnisotropicFiltering.Disable;
            var selectedLevel = lightingQualities[ShadowQuality];
            urpAsset.maxAdditionalLightsCount = selectedLevel.maxLightsCount;
            urpAsset.shadowDistance = selectedLevel.shadowDistance;
            if (selectedLevel.shadowCascades != 0) urpAsset.shadowCascadeCount = selectedLevel.shadowCascades;
            Screen.brightness = Brightness;

            #region crossSceneSetup

            SceneManager.activeSceneChanged += (_, _) =>
            {
                var cd = FindObjectOfType<UniversalAdditionalCameraData>();
                if (cd is null)
                {
                    DebugConsole.Log(
                        "Universal Additional Camera Data couldn't be found in this scene. Anti-aliasing settings won't be applied.",
                        DebugConsole.WarningColor);
                    return;
                }

                cd.antialiasing = AntiAliasing;
                cam = cd.GetComponent<Camera>();
                ApplyFarClipping();
            };

            #endregion
        }

        byte ISaveable.Id { get; set; }

        public void OnBeforeSerialize()
        {
            currentHeight = CurrentResolution.height;
            currentWidth = CurrentResolution.width;
        }

        public void OnAfterDeserialize()
        {
            var res = CurrentResolution;
            res.height = currentHeight;
            res.width = currentWidth;
            res.refreshRate = CurrentRefreshRate;
            CurrentResolution = res;
        }

        public void ChangeFullscreenMode(int modeNumber)
        {
            switch (modeNumber)
            {
                case > 2 or < 0:
                    DebugConsole.Log("The specified fullscreen mode does not exist.", DebugConsole.WarningColor);
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
            if (CurrentResolution.width == width) return;
            CurrentResolution = new Resolution
            {
                width = width,
                height = height,
                refreshRate = CurrentRefreshRate
            };
            Screen.SetResolution(CurrentResolution.width, CurrentResolution.height, ScreenMode, CurrentRefreshRate);
        }

        public void ChangeRenderResolution(float scaleNumber)
        {
            RenderScale = Mathf.Clamp(scaleNumber, 0.1f, 2);
            urpAsset.renderScale = RenderScale;
        }

        public void ChangeRefreshRate(int rateNumber)
        {
            var newRate = int.Parse(refreshRates[rateNumber]);
            if (CurrentRefreshRate == newRate) return;
            CurrentRefreshRate = newRate;
            Application.targetFrameRate = CurrentRefreshRate;
        }

        public void ToggleSsao(bool newValue)
        {
            IsSsaoEnabled = newValue;
            ssao.SetActive(IsSsaoEnabled);
        }

        public void ToggleVSync(bool newValue)
        {
            IsVSyncEnabled = newValue;
            QualitySettings.vSyncCount = IsVSyncEnabled ? 1 : 0;
        }

        public void ChangeAntiAliasing(int setting)
        {
            AntiAliasing = (AntialiasingMode)Mathf.Clamp(setting, 0, 2);
            cameraData.antialiasing = AntiAliasing;
            cameraData.antialiasingQuality = AntialiasingQuality.High;
        }

        public void ChangeWorldQuality(int level)
        {
            WorldQualityLevel = (byte)level;
            var quality = worldQualities[WorldQualityLevel];
            QualitySettings.masterTextureLimit = quality.maxTextureSize;
            QualitySettings.streamingMipmapsMemoryBudget = quality.textureMemoryBudget;
            ApplyFarClipping();
        }

        public void ChangeModelQuality(int level)
        {
            ModelQualityLevel = (byte)Mathf.Clamp(level, 0, 3);
            var (bias, max) = LODSettings[level];
            QualitySettings.SetLODSettings(bias, max);
        }

        public void ChangeTextureFiltering(bool newValue)
        {
            IsUsingAnisoFiltering = newValue;
            QualitySettings.anisotropicFiltering =
                IsUsingAnisoFiltering ? AnisotropicFiltering.ForceEnable : AnisotropicFiltering.Disable;
        }

        public void ChangeVfxQuality(int newValue)
        {
            DebugConsole.Log("This feature is not implemented yet. :(", DebugConsole.MissingColor);
            throw new NotImplementedException();
        }

        public void ChangeShadowQuality(int newValue)
        {
            ShadowQuality = (byte)newValue;
            var selectedLevel = lightingQualities[ShadowQuality];
            urpAsset.maxAdditionalLightsCount = selectedLevel.maxLightsCount;
            urpAsset.shadowDistance = selectedLevel.shadowDistance;
            if (selectedLevel.shadowCascades != 0) urpAsset.shadowCascadeCount = selectedLevel.shadowCascades;
        }

        public void ChangeBrightness(float newBrightness)
        {
            Brightness = Mathf.Clamp(newBrightness, 0, 1);
            Screen.brightness = Brightness;
        }

        private void ApplyFarClipping()
        {
            var fcp = worldQualities[WorldQualityLevel].farClippingPlane;
            if (cam.TryGetComponent<CinemachineVirtualCamera>(out var vCam))
            {
                var lens = vCam.m_Lens;
                lens.FarClipPlane = fcp;
                vCam.m_Lens = lens;
            }
            else
            {
                cam.farClipPlane = fcp;
            }
        }

        private struct WorldQuality
        {
            public readonly int farClippingPlane;
            public readonly int maxTextureSize;
            public readonly int textureMemoryBudget;

            public WorldQuality(int farClippingPlane, int maxTextureSize, int textureMemoryBudget)
            {
                this.maxTextureSize = maxTextureSize;
                this.textureMemoryBudget = textureMemoryBudget;
                this.farClippingPlane = farClippingPlane;
            }
        }

        private struct LightingQuality
        {
            public readonly int maxLightsCount;
            public readonly int shadowDistance;
            public readonly int shadowCascades;

            public LightingQuality(int maxLightsCount, int shadowDistance, int shadowCascades)
            {
                this.maxLightsCount = maxLightsCount;
                this.shadowDistance = shadowDistance;
                this.shadowCascades = shadowCascades;
            }
        }
    }
}