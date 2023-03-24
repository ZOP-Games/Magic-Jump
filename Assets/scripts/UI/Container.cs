using System;
using GameExtensions.Debug;
using UnityEngine;

namespace GameExtensions.UI
{
    public abstract class Container : UIComponent
    {
        protected RectTransform[] elements;
        [SerializeField]protected float padding = 8f;
        protected abstract void BuildLayout();
    }
}