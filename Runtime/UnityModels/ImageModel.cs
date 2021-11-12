using UnityEngine;
using UnityEngine.UI;

namespace Tofunaut.Bootstrap.UnityModels
{
    public class ImageModel : GraphicModel<Image>
    {
        public Sprite Sprite;
        
        public override Image Build()
        {
            var image = base.Build();
            image.sprite = Sprite;

            return image;
        }
    }
}