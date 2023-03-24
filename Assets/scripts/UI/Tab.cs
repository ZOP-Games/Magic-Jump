using GameExtensions.Debug;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameExtensions.UI
{
    [RequireComponent(typeof(Image))]
    public class Tab : UIComponent, IPointerDownHandler
    {
        [HideInInspector]public byte id;
        [SerializeField] private Color activeColor;
        [SerializeField] private Color disabledColor;
        [SerializeField] private GameObject menuLayout;
        [SerializeField] private float passiveSizePc;
        private Image background;
        private RectTransform rTf;
        private TabGroup group;

        public void Activate()
        {
            background.color = activeColor;
            rTf.sizeDelta /= passiveSizePc;
            group.RefreshLayout();
            menuLayout.SetActive(true);
        }

        public void Deactivate()
        {
            background.color = disabledColor;
            rTf.sizeDelta *= passiveSizePc;
            menuLayout.SetActive(false);
        }

        private void OnEnable()
        {
            group = transform.parent.GetComponent<TabGroup>();
            UnityEngine.Debug.Assert(group is not null, "Tab (" + name + ") is not part of a TabGroup. " +
                                                        "Make sure the Tab is a TabGroup's child!");
            background = GetBackground();
            rTf = GetComponent<RectTransform>();
        }

        private Image GetBackground()
        {
            return GetComponent<Image>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            group.SetActiveTab(id);
        }
    }
}