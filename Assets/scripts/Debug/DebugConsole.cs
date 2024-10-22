using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameExtensions.Debug
{
    public static class DebugConsole
    {
        private const byte FontSize = 20;
        private const byte ErrorFontSize = 36;
        private static bool writtenError;
        private static bool enabled = true;
        private static readonly Color FontColor = Color.black;
        public static Color TestColor => Color.cyan;
        public static Color WarningColor => Color.yellow;
        public static Color ErrorColor => Color.red;
        public static Color SuccessColor => Color.green;
        public static Color MissingColor => Color.magenta;
        public static Color HintColor => new Color32(103, 58, 183, 255);
        private static bool IsWriterAvailable => Writer is not null;
        private static DebugMessageWriter Writer => DebugMessageWriter.Instance;

        // ReSharper disable Unity.PerformanceAnalysis
        [Conditional("DEVELOPMENT_BUILD"),Conditional("UNITY_EDITOR")]
        public static void Log(string text, Color color, byte fontSize)
        {
            if (!IsWriterAvailable && !writtenError)
            {
                UnityEngine.Debug.LogWarning(
                    "The message writer is not available. Debug console messages will be written to Unity console only"
                );
                writtenError = true;
            }

            if (!IsWriterAvailable || !enabled)
            {
                UnityEngine.Debug.Log(text);
                return;
            }

            if (color == FontColor) color = SceneManager.GetActiveScene().buildIndex is 0 ? Color.white : FontColor;
            Writer.ClearStyles();
            UnityEngine.Debug.Log(text);
            Writer.WriteLine(text, color, fontSize);
        }

        public static void Log(string text, Color color)
        {
            Log(text, color, FontSize);
        }

        public static void Log(string text, byte fontSize)
        {
            Log(text, FontColor, fontSize);
        }

        public static void Log(string text)
        {
            Log(text, FontColor, FontSize);
        }

        public static void LogError(string text)
        {
            if (!IsWriterAvailable)
            {
                text = text.Insert(0, "ERROR: ");
                Log(text, ErrorColor, ErrorFontSize);
                return;
            }
            Writer.ClearText();
            Log(text, ErrorColor, ErrorFontSize);
            Writer.AddStyle(DebugMessageWriter.TextStyle.Bold);
            Writer.AddStyle(DebugMessageWriter.TextStyle.Underline);
        }

        public static void ToggleConsole()
        {
            enabled = !enabled;
            Writer.gameObject.SetActive(enabled);
            if (enabled || !IsWriterAvailable) return;
            UnityEngine.Debug.Log(
                "The on-screen debug console is disabled. "
                + "Debug console messages will be written to Unity console only");
        }
    }
}