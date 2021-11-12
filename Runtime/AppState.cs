using System.Threading.Tasks;
using Tofunaut.Bootstrap.Interfaces;

namespace Tofunaut.Bootstrap
{
    public abstract class AppState<TAppStateRequest> : IAppState
    {
        public abstract Task OnEnter(TAppStateRequest request);

        public Task OnEnter(object request) => OnEnter((TAppStateRequest) request);

        public virtual Task OnExit() => Task.CompletedTask;
    }
}