using System;
using Tofunaut.Bootstrap.Interfaces;
using UnityEngine;

namespace Tofunaut.Bootstrap
{
    public class MonoBehaviourUpdateService : MonoBehaviour, IUpdateService
    {
        public event Action Updated;
        public event Action UpdatedLate;
        public event Action UpdatedFixed;

        private void Update()
        {
            Updated?.Invoke();
        }

        private void LateUpdate()
        {
            UpdatedLate?.Invoke();
        }

        private void FixedUpdate()
        {
            UpdatedFixed?.Invoke();
        }
    }
}