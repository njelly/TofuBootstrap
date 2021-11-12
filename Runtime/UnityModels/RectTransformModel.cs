using System;
using System.Linq;
using UnityEngine;

namespace Tofunaut.Bootstrap.UnityModels
{
    public abstract class RectTransformModel<T> : UnityModel<T> where T : Component
    {
        public Vector2 Pivot;
        public Vector2 AnchorMin;
        public Vector2 AnchorMax;
        public Vector2 SizeDelta;
        public Vector2 AnchoredPosition;
        
        public override T Build()
        {
            var rectTransform = BuildGameObject(typeof(RectTransform), typeof(T)).GetComponent<RectTransform>();
            rectTransform.pivot = Pivot;
            rectTransform.anchorMin = AnchorMin;
            rectTransform.anchorMax = AnchorMax;
            rectTransform.sizeDelta = SizeDelta;
            rectTransform.anchoredPosition = AnchoredPosition;

            return rectTransform.GetComponent<T>();
        }
    }
}