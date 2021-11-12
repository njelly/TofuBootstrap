using System.Threading.Tasks;

namespace Tofunaut.Bootstrap.Interfaces
{
    public interface IViewController
    {
        Task OnPushedToStack(object model);
        Task OnGainedFocus();
        Task OnLostFocus();
        Task OnPoppedFromStack();
    }
}