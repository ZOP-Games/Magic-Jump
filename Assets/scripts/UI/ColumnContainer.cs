using GameExtensions.Debug;
using System.Linq;
using UnityEngine;

namespace GameExtensions.UI
{
    public class ColumnContainer : Container
    {
        private void Start()
        {
            elements = GetComponentsInChildren<RectTransform>().Where(t => t.parent == transform).ToArray();
            BuildLayout();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            var pos = transform.position;
            var endPos = new Vector3(pos.x + 50, pos.y, pos.z);
            Gizmos.DrawLine(pos, endPos);
        }

        protected override void BuildLayout()
        {
            if (elements is null || elements.Length < 1)
            {
                DebugConsole.LogError("The tab layout could not be built: There are no elements to organise.");
                return;
            }

            DebugConsole.Log("Building layout");
            var totalY = (elements.Sum(r => r.sizeDelta.y) + (elements.Length + 1) * padding) / 2;
            for (var i = 0; i < elements.Length; i++)
            {
                var rtf = elements[i];
                var pos = rtf.anchoredPosition;
                var delta = rtf.sizeDelta;
                pos.y = totalY - delta.y / 2;
                pos.y -= (i + 1) * padding;
                totalY -= delta.y;
                pos.x = 0;
                rtf.anchoredPosition = pos;
            }
        }
    }
}