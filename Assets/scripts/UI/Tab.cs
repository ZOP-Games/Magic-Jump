using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameExtensions.UI
{
    [RequireComponent(typeof(Image))]
    public class Tab : UIComponent, IPointerDownHandler
    {
        [HideInInspector] public byte id;
        [SerializeField] private Color activeColor;
        [SerializeField] private Color disabledColor;
        [SerializeField] private GameObject menuLayout;
        [SerializeField] private float passiveSizePc;
        private Vector2 activeSize;
        private Vector2 passiveSize;
        private Image Background => GetComponent<Image>();
        private RectTransform rTf;
        private TabGroup group;

        public void Activate()
        {
            Background.color = activeColor;
            rTf.sizeDelta = activeSize;
            group.RefreshLayout();
            menuLayout.SetActive(true);
        }

        public void Deactivate()
        {
            Background.color = disabledColor;
            rTf.sizeDelta = passiveSize;
            menuLayout.SetActive(false);
        }

        private void Awake()
        {
            group = transform.parent.GetComponent<TabGroup>();
            UnityEngine.Debug.Assert(group is not null, "Tab (" + name + ") is not part of a TabGroup. " +
                                                        "Make sure the Tab is a TabGroup's child!");
            rTf = GetComponent<RectTransform>();
            activeSize = rTf.sizeDelta;
            passiveSize = activeSize * passiveSizePc;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            group.SetActiveTab(id);
        }
    }
}