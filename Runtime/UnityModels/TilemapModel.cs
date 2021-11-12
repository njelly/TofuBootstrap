using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tofunaut.Bootstrap.UnityModels
{
    public class TilemapModel : UnityModel<Tilemap>
    {
        public float AnimationFrameRate = 1f;
        public Color Color = Color.white;
        public Tilemap.Orientation Orientation = Tilemap.Orientation.XY;
        public TilemapRendererModel TilemapRenderer = new TilemapRendererModel();

        public class TilemapRendererModel
        {
            public TilemapRenderer.SortOrder SortOrder = UnityEngine.Tilemaps.TilemapRenderer.SortOrder.BottomLeft;
            public TilemapRenderer.Mode Mode = UnityEngine.Tilemaps.TilemapRenderer.Mode.Chunk;
            public TilemapRenderer.DetectChunkCullingBounds DetectChunkCullingBounds =
                UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds.Auto;
            public SpriteMaskInteraction MaskInteraction = SpriteMaskInteraction.None;
            public string SortingLayerName = "Default";
            public int OrderInLayer = 0;
        }
        
        public override Tilemap Build()
        {
            var tilemap = BuildGameObject(typeof(Tilemap), typeof(TilemapRenderer)).GetComponent<Tilemap>();
            tilemap.animationFrameRate = AnimationFrameRate;
            tilemap.color = Color;
            tilemap.orientation = Orientation;

            var tilemapRenderer = tilemap.GetComponent<TilemapRenderer>();
            tilemapRenderer.sortOrder = TilemapRenderer.SortOrder;
            tilemapRenderer.mode = TilemapRenderer.Mode;
            tilemapRenderer.detectChunkCullingBounds = TilemapRenderer.DetectChunkCullingBounds;
            tilemapRenderer.maskInteraction = TilemapRenderer.MaskInteraction;
            tilemapRenderer.sortingLayerName = TilemapRenderer.SortingLayerName;
            tilemapRenderer.sortingOrder = TilemapRenderer.OrderInLayer;

            return tilemap;
        }
    }
}