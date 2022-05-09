using System;
using Tofunaut.Bootstrap.Interfaces;

namespace Tofunaut.Bootstrap
{
    public class MonoBehaviourUpdateService : IUpdateService
    {
        public event Action Updated;

        private void Update()
        {
            Updated?.Invoke();
        }
    }
}