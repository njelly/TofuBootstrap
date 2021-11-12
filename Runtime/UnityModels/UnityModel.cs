using System;
using UnityEngine;

namespace Tofunaut.Bootstrap.UnityModels
{
    public abstract class UnityModel<T> where T : Component
    {
        public string Name;
        public Transform Parent = null;
        public Vector3 Position = Vector3.zero;
        public Vector3 Scale = Vector3.one;
        public Quaternion Rotation = Quaternion.identity;
        public bool WorldPositionStays = true;

        protected GameObject BuildGameObject(params Type[] types)
        {
            var go = new GameObject(Name, types);

            var t = go.transform;
            t.localPosition = Position;
            t.localScale = Scale;
            t.rotation = Rotation;
            
            if(Parent)
                t.SetParent(Parent, WorldPositionStays);

            return go;
        }

        public abstract T Build();
    }
}