using System.Threading.Tasks;
using Tofunaut.Bootstrap.Interfaces;
using UnityEngine;

namespace Tofunaut.Bootstrap
{
    public abstract class ViewController<TViewControllerModel> : MonoBehaviour, IViewController
    {
        public abstract Task OnPushedToStack(TViewControllerModel model);

        public Task OnPushedToStack(object model) => OnPushedToStack((TViewControllerModel)model);

        public virtual Task OnGainedFocus() => Task.CompletedTask;
        public virtual Task OnLostFocus() => Task.CompletedTask;
        public virtual Task OnPoppedFromStack() => Task.CompletedTask;
    }
}