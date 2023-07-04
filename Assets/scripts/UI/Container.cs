using UnityEngine;

namespace GameExtensions.UI
{
    public abstract class Container : UIComponent
    {
        protected float padding = 8f;
        protected RectTransform[] elements;
        protected abstract void BuildLayout();
    }
}