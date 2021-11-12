using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tofunaut.Bootstrap.Interfaces;
using Tofunaut.Bootstrap.UnityModels;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Tofunaut.Bootstrap
{
    
    public class ViewStack
    {
        private readonly Stack<IViewController> _stack;
        private readonly Canvas _canvas;
        private readonly Dictionary<Type, Type> _modelTypeToViewType;
        private readonly Dictionary<Type, AssetReference> _viewTypeToAssetReference;

        public ViewStack(Canvas canvas)
        {
            _stack = new Stack<IViewController>();
            _modelTypeToViewType = new Dictionary<Type, Type>();
            _viewTypeToAssetReference = new Dictionary<Type, AssetReference>();

            _canvas = canvas;
        }
        
        public ViewStack(CanvasModel canvasModel)
        {
            _stack = new Stack<IViewController>();
            _modelTypeToViewType = new Dictionary<Type, Type>();
            _viewTypeToAssetReference = new Dictionary<Type, AssetReference>();

            _canvas = canvasModel.Build();
        }

        public void RegisterViewController<TViewController, TViewControllerModel>(AssetReference assetReference) where TViewController : ViewController<TViewControllerModel>
        {
            _modelTypeToViewType.Add(typeof(TViewControllerModel), typeof(TViewController));
            _viewTypeToAssetReference.Add(typeof(TViewController), assetReference);
        }

        public async Task<TViewController> Push<TViewController, TViewControllerModel>(TViewControllerModel model) where TViewController : ViewController<TViewControllerModel>
        {
            if (!_modelTypeToViewType.TryGetValue(typeof(TViewControllerModel), out var viewType))
                throw new ViewNotRegisteredException<TViewController>();

            var assetReference = _viewTypeToAssetReference[viewType];
            var go = await Addressables.InstantiateAsync(assetReference, _canvas.transform, false).Task;
            var viewController = go.GetComponent<TViewController>();

            if (_stack.Count > 0)
                await _stack.Peek().OnLostFocus();
            
            await viewController.OnPushedToStack(model);
            await viewController.OnGainedFocus();
            
            _stack.Push(viewController);

            return viewController;
        }

        public async Task Pop()
        {
            if (_stack.Count <= 0)
                return;

            var viewController = _stack.Pop();
            await viewController.OnLostFocus();
            await viewController.OnPoppedFromStack();
            
            UnityEngine.Object.Destroy(((MonoBehaviour)viewController).gameObject);

            if (_stack.Count <= 0)
                return;

            await _stack.Peek().OnGainedFocus();
        }

        public async Task<bool> PopIf<TViewController>()
        {
            if (!(_stack.Peek() is TViewController))
                return false;

            await Pop();

            return true;
        }

        public async Task PopAll()
        {
            while (_stack.Count > 0)
                await Pop();
        }

        public class ViewNotRegisteredException<TViewController> : Exception where TViewController : IViewController
        {
            public ViewNotRegisteredException() : base(
                $"the view controller {typeof(TViewController)} has not been registered") { }
        }
    }
}