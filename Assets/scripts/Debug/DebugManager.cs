namespace GameExtensions.Debug
{

    public static class DebugManager
    {
        public static bool IsDebugEnabled { get; private set; }
        public static bool IsSuperDodging { get; set; }
        public static bool IsInvincible { get; set; }
        public static bool DrawForceRays { get; set; }

        public static void ToggleDebug()
        {
            IsDebugEnabled = !IsDebugEnabled;
            if (IsDebugEnabled) DebugInputHandler.Instance.EnableDebugActions();
            else DebugInputHandler.Instance.DisableDebugActions();
        }
    }
}