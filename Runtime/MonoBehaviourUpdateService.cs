using System;
using Tofunaut.Bootstrap.Interfaces;
using UnityEngine;

namespace Tofunaut.Bootstrap
{
    public class MonoBehaviourUpdateService : MonoBehaviour, IUpdateService
    {
        public event Action Updated;

        private void Update()
        {
            Updated?.Invoke();
        }
    }
}