using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace GameExtensions.Debug
{
    public class DebugMessageWriter : MonoBehaviour
    {
        private const byte MaxLogs = 5;
        private readonly Queue<string> textBuffer = new();
        private TextMeshProUGUI debugText;
        internal static DebugMessageWriter Instance { get; private set; }


        // Start is called before the first frame update
        private void Start()
        {
            if (Instance is null) Instance = this;
            else Destroy(this);
            debugText = GetComponent<TextMeshProUGUI>();
            DontDestroyOnLoad(transform.parent.gameObject);
        }

        internal void WriteLine(string text, Color fontColor, byte fontSize)
        {
            debugText.enabled = true;
            debugText.color = fontColor;
            debugText.fontSize = fontSize;
            textBuffer.Enqueue(text);
            if (textBuffer.Count > MaxLogs) textBuffer.Dequeue();
            var allText = textBuffer.Aggregate((s, next) => s + '\n' + next);
            debugText.SetText(allText);
        }

        internal void ClearText()
        {
            debugText.SetText("");
            textBuffer.Clear();
        }

        #region Styling

        internal void AddStyle(TextStyle style)
        {
            var format = style switch
            {
                TextStyle.Bold => FontStyles.Bold,
                TextStyle.Italic => FontStyles.Italic,
                TextStyle.Underline => FontStyles.Underline,
                TextStyle.Strikethrough => FontStyles.Strikethrough,
                TextStyle.Highlight => FontStyles.Highlight,
                _ => FontStyles.Normal
            };
            debugText.fontStyle |= format;
        }

        internal void ClearStyles()
        {
            debugText.fontStyle = FontStyles.Normal;
        }

        internal enum TextStyle
        {
            None,
            Bold,
            Italic,
            Underline,
            Strikethrough,
            Highlight
        }

        #endregion
    }
}