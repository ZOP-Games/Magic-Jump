using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameExtensions.Debug
{
    public static class DebugConsole
    {
        private const byte FontSize = 20;
        private const byte ErrorFontSize = 36;
        private static bool _writtenError;
        private static readonly Color FontColor = Color.black;
        public static Color TestColor => new(0, 0.91f, 0.8f);
        public static Color WarningColor => Color.yellow;
        public static Color ErrorColor => Color.red;
        public static Color SuccessColor => Color.green;
        public static Color MissingColor => Color.magenta;
        private static bool IsWriterAvalaible => Writer is not null;
        private static DebugMessageWriter Writer => DebugMessageWriter.Instance;

        public static void Log(string text, Color color, byte fontSize)
        {
            if (!IsWriterAvalaible && !_writtenError)
            {
                UnityEngine.Debug.LogWarning(
                    "The message writer is not available. Debug console messages will be written to Unity console only");
                _writtenError = true;
            }

            if (!IsWriterAvalaible)
            {
                UnityEngine.Debug.Log(text);
                return;
            }

            if (color == FontColor) color = SceneManager.GetActiveScene().buildIndex is 0 ? Color.white : FontColor;
            Writer.ClearStyles();
#if UNITY_EDITOR
            UnityEngine.Debug.Log(text);
#endif
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
            if (!IsWriterAvalaible)
            {
                Log(text, Color.red, ErrorFontSize);
                return;
            }

            Writer.ClearText();
            Log(text, Color.red, ErrorFontSize);
            Writer.AddStyle(DebugMessageWriter.TextStyle.Bold);
            Writer.AddStyle(DebugMessageWriter.TextStyle.Underline);
        }
    }
}