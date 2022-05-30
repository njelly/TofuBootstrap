using System.Threading.Tasks;
using UnityEngine;

namespace Tofunaut.Bootstrap.Interfaces
{
    public interface IUIController
    {
        public GameObject GameObject { get; }
        Task OnPushedToStack(object model);
        Task OnGainedFocus();
        Task OnLostFocus();
        Task OnPoppedFromStack();
    }
}