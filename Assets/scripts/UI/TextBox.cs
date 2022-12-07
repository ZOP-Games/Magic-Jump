using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameExtensions.UI
{ 
    /// <summary>
    /// Class representing gameplay text boxes (used for e.g. NPC-s)
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI),typeof(Image))]
    public class TextBox : MenuScreen,IUIComponent
    {
        public Image BackgroundImage => GetComponent<Image>();
        public RectTransform Transform => GetComponent<RectTransform>();
        private string text;
        private TextMeshProUGUI TMPBox => GetComponent<TextMeshProUGUI>();

        private void Start()
        {
            IUIComponent.Transform.anchoredPosition = new Vector2(50, 20);
        }
    }
}