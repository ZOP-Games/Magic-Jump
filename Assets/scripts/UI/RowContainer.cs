using System;
using System.Linq;
using GameExtensions.Debug;
using UnityEngine;

namespace GameExtensions.UI
{
    public class RowContainer : Container
    {
        protected override void BuildLayout()
        {
            if (elements is null || elements.Length < 1)
            {
                DebugConsole.LogError("The tab layout could not be built: There are no elements to organize.");
                return;
            }
            DebugConsole.Log("Building layout");
            var totalX = (elements.Sum(r => r.sizeDelta.x)+(elements.Length+1)*padding)/-2;
            for (var i = 0; i < elements.Length; i++)
            {
                var rtf = elements[i];
                var pos = rtf.anchoredPosition;
                var delta = rtf.sizeDelta;
                pos.x = totalX + delta.x/2;
                pos.x += (i+1) * padding;
                totalX += delta.x;
                pos.y = delta.y / 2;
                rtf.anchoredPosition = pos;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            var pos = transform.position;
            var endPos = new Vector3(pos.x, pos.y + 50, pos.z);
            Gizmos.DrawLine(pos,endPos);
        }

        private void Start()
        {
            elements = GetComponentsInChildren<UIComponent>().Where(c =>  c.gameObject != gameObject)
                .Select(c => c.GetComponent<RectTransform>()).ToArray();
            BuildLayout();
        }
    }
}