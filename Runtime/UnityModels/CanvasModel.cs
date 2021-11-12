using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tofunaut.Bootstrap.UnityModels
{
    public class CanvasModel : UnityModel<Canvas>
    {
        public RenderMode RenderMode = RenderMode.ScreenSpaceOverlay;
        public bool PixelPerfect = false;
        public int SortingOrder = 0;
        public int TargetDisplay = 0;
        public AdditionalCanvasShaderChannels AdditionalCanvasShaderChannels = AdditionalCanvasShaderChannels.None;
        public CanvasScalerModel CanvasScaler = new CanvasScalerModel();
        public GraphicRaycasterModel GraphicRaycaster = new GraphicRaycasterModel();

        public class CanvasScalerModel
        {
            public CanvasScaler.ScaleMode UiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ConstantPixelSize;
            public float ScaleFactor = 1f;
            public float ReferencePixelsPerUnit = 100f;
            public Vector2 ReferenceResolution = new Vector2(800, 600);
            public float MatchWidthOrHeight = 0f;
        }

        public class GraphicRaycasterModel
        {
            public bool IgnoreReversedGraphics = true;
            public GraphicRaycaster.BlockingObjects BlockingObjects =
                UnityEngine.UI.GraphicRaycaster.BlockingObjects.None;
            public LayerMask BlockingMask = ~0;
        }

        public override Canvas Build()
        {
            var go = BuildGameObject(typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            
            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode;
            canvas.pixelPerfect = PixelPerfect;
            canvas.sortingOrder = SortingOrder;
            canvas.targetDisplay = TargetDisplay;
            canvas.additionalShaderChannels = AdditionalCanvasShaderChannels;

            var canvasScaler = canvas.GetComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.UiScaleMode;
            canvasScaler.scaleFactor = CanvasScaler.ScaleFactor;
            canvasScaler.referencePixelsPerUnit = CanvasScaler.ReferencePixelsPerUnit;
            canvasScaler.referenceResolution = CanvasScaler.ReferenceResolution;
            canvasScaler.matchWidthOrHeight = CanvasScaler.MatchWidthOrHeight;

            var graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
            graphicRaycaster.ignoreReversedGraphics = GraphicRaycaster.IgnoreReversedGraphics;
            graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects;
            graphicRaycaster.blockingMask = GraphicRaycaster.BlockingMask;

            return canvas;
        }
    }
}