using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameExtensions.Debug
{
    public static class DebugConsole
    {
        private const int FontSize = 36;
        private static readonly Color FontColor = Color.black;
        private static DebugMessageWriter Writer => DebugMessageWriter.Instance;

        public static void LogFor(string text, float seconds, Color color, byte fontSize)
        {
            Writer.StartCoroutine(Writer.WriteText(text, color, fontSize, seconds));
        }

        public static void LogFor(string text, float seconds, Color color)
        {
            Writer.StartCoroutine(Writer.WriteText(text, color, FontSize, seconds));
        }

        public static void LogFor(string text, float seconds, byte fontSize)
        {
            Writer.StartCoroutine(Writer.WriteText(text, FontColor, fontSize, seconds));
        }

        public static void LogFor(string text, float seconds)
        {
            Writer.StartCoroutine(Writer.WriteText(text, FontColor, FontSize, seconds));
        }
        
        public static void Log(string text, Color color, byte fontSize)
        {
            Writer.WriteTextPermanent(text,color,fontSize);
        }

        public static void Log(string text, Color color)
        {
            Writer.WriteTextPermanent(text,color,FontSize);
        }

        public static void Log(string text, byte fontSize)
        {
            Writer.WriteTextPermanent(text,FontColor,fontSize);
        }

        public static void Log(string text)
        {
            Writer.WriteTextPermanent(text,FontColor,FontSize);
        }


    }
}

