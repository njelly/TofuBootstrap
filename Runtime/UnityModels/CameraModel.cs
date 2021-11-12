using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.Bootstrap.UnityModels
{
    public class CameraModel : UnityModel<Camera>
    {
        public int CullingMask = -1;
        public Color BackgroundColor;
        public bool Orthographic;
        public int OrthographicSize;
        public float NearClipPlane = 0.3f;
        public float FarClipPlane = 1000f;
        public int Depth = -1;
        public bool AllowMssa;
        public bool UseOcclusionCulling;
        public StereoTargetEyeMask TargetEye = StereoTargetEyeMask.None;
        public bool HasAudioListener = true;

        public override Camera Build()
        {
            var typeList = new List<Type>
            {
                typeof(Camera)
            };
            
            if(HasAudioListener)
                typeList.Add(typeof(AudioListener));

            var go = BuildGameObject(typeList.ToArray());
            var camera = go.GetComponent<Camera>();
            camera.cullingMask = CullingMask;
            camera.backgroundColor = BackgroundColor;
            camera.orthographic = Orthographic;
            camera.orthographicSize = OrthographicSize;
            camera.nearClipPlane = NearClipPlane;
            camera.farClipPlane = FarClipPlane;
            camera.depth = Depth;
            camera.allowMSAA = AllowMssa;
            camera.useOcclusionCulling = UseOcclusionCulling;
            camera.stereoTargetEye = TargetEye;

            return camera;
        }
    }
}