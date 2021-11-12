using UnityEngine;
using UnityEngine.UI;

namespace Tofunaut.Bootstrap.UnityModels
{
    public abstract class GraphicModel<T> : RectTransformModel<T> where T : Graphic
    {
        public Color Color;
        public bool RaycastTarget;
        
        public override T Build()
        {
            var t = base.Build();
            t.color = Color;
            t.raycastTarget = RaycastTarget;

            return t;
        }
    }
}