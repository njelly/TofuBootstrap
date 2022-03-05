using System.Threading.Tasks;
using Tofunaut.Bootstrap.Interfaces;
using UnityEngine;

namespace Tofunaut.Bootstrap
{
    public abstract class CanvasViewController<TCanvasViewModel> : MonoBehaviour, ICanvasViewController
    {
        public GameObject GameObject => gameObject;
        public abstract Task OnPushedToStack(TCanvasViewModel model);

        public Task OnPushedToStack(object model) => OnPushedToStack((TCanvasViewModel)model);

        public virtual Task OnGainedFocus() => Task.CompletedTask;
        public virtual Task OnLostFocus() => Task.CompletedTask;
        public virtual Task OnPoppedFromStack() => Task.CompletedTask;
    }
}