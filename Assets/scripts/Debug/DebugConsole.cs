using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameExtensions.Debug
{
    public static class DebugConsole
    {
        private const byte FontSize = 20;
        private const byte ErrorFontSize = 36; 
        private static readonly Color FontColor = Color.black;
        private static DebugMessageWriter Writer
        {
            get
            {
                UnityEngine.Debug.Assert(DebugMessageWriter.Instance is not null);
                return DebugMessageWriter.Instance;
            }
        }

        public static void Log(string text, Color color, byte fontSize)
        {
            if(color == FontColor) color = SceneManager.GetActiveScene().buildIndex is 0 ? Color.white : FontColor;
            Writer.ClearStyles();
            #if UNITY_EDITOR 
                UnityEngine.Debug.Log(text);
            #endif
            Writer.WriteLine(text,color,fontSize);
        }

        public static void Log(string text, Color color)
        {
            Log(text,color,FontSize);
        }

        public static void Log(string text, byte fontSize)
        {
           Log(text,FontColor,fontSize);
        }

        public static void Log(string text)
        {
            Log(text,FontColor,FontSize);
        }

        public static void LogError(string text)
        {
            Writer.ClearText();
            Log(text,Color.red,ErrorFontSize);
            Writer.AddStyle(DebugMessageWriter.TextStyle.Bold);
            Writer.AddStyle(DebugMessageWriter.TextStyle.Underline);
        }

    }
}

