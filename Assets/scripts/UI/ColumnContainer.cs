using System.Linq;
using GameExtensions.Debug;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameExtensions.UI
{
    public class ColumnContainer : Container
    {

        protected override void BuildLayout()
        {
            if (elements is null || elements.Length < 1)
            {
                DebugConsole.LogError("The tab layout could not be built: There are no elements to organize.");
                return;
            }
            DebugConsole.Log("Building layout");
            var totalY = 0f;
            for (var i = 0; i < elements.Length; i++)
            {
                var rtf = elements[i];
                var pos = rtf.anchoredPosition;
                var delta = rtf.sizeDelta;
                pos.y = totalY-delta.y / 2-(i+1)*padding;
                totalY -= delta.y;
                pos.x = delta.x / 2;
                rtf.anchoredPosition = pos;
            }
        }

        private void Start()
        {
            elements = GetComponentsInChildren<RectTransform>().Where(t => t.parent == transform).ToArray();
            BuildLayout();
        }
    }
}