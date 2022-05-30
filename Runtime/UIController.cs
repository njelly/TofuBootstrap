using System.Threading.Tasks;
using Tofunaut.Bootstrap.Interfaces;
using UnityEngine;

namespace Tofunaut.Bootstrap
{
    public abstract class UIController<TUIModel> : MonoBehaviour, IUIController
    {
        public GameObject GameObject => gameObject;
        public abstract Task OnPushedToStack(TUIModel model);

        public Task OnPushedToStack(object model) => OnPushedToStack((TUIModel)model);

        public virtual Task OnGainedFocus() => Task.CompletedTask;
        public virtual Task OnLostFocus() => Task.CompletedTask;
        public virtual Task OnPoppedFromStack() => Task.CompletedTask;
    }
}