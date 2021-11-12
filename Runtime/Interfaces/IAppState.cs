using System.Threading.Tasks;

namespace Tofunaut.Bootstrap.Interfaces
{
    public interface IAppState
    {
        Task OnEnter(object request);
        Task OnExit();
    }
}