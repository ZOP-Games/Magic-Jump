using System.Collections;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

namespace GameExtensions.Debug
{
    public class DebugMessageWriter : MonoBehaviour
    {
        internal static DebugMessageWriter Instance { get; private set; }
        private TextMeshProUGUI debugText;

        internal IEnumerator WriteText(string text, Color fontColor, byte fontSize,float seconds)
        {
            debugText.enabled = true;
            debugText.color = fontColor;
            debugText.fontSize = fontSize;
            debugText.SetText(text);
            yield return new WaitForSeconds(seconds);
            ClearText();
        }

        internal void WriteTextPermanent(string text, Color fontColor, byte fontSize)
        {
            debugText.enabled = true;
            debugText.color = fontColor;
            debugText.fontSize = fontSize;
            debugText.SetText(text);
        }

        internal void ClearText()
        {
            debugText.enabled = false;
        }
        // Start is called before the first frame update
        private void Start()
        {
            if (Instance is null) Instance = this;
            else Destroy(this);
            debugText = GetComponent<TextMeshProUGUI>();
        }
    }
}

