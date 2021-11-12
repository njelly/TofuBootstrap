using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tofunaut.Bootstrap.Interfaces;

namespace Tofunaut.Bootstrap
{
    public class AppStateMachine
    {
        private Dictionary<Type, IAppState> _requestTypeToAppState;
        private object _currentStateRequest;

        public AppStateMachine()
        {
            _requestTypeToAppState = new Dictionary<Type, IAppState>();
        }
        
        public void RegisterState<TAppState, TAppStateRequest>(TAppState appState) where TAppState : AppState<TAppStateRequest>
        {
            _requestTypeToAppState.Add(typeof(TAppStateRequest), appState);
        }
        
        public async Task EnterState<TAppStateRequest>(TAppStateRequest request)
        {
            if (_currentStateRequest != null)
                await _requestTypeToAppState[_currentStateRequest.GetType()].OnExit();

            _currentStateRequest = request;
            await _requestTypeToAppState[_currentStateRequest.GetType()].OnEnter(_currentStateRequest);
        }
    }
}