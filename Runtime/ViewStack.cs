using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Register a ViewController/Model pair, allow with the AssetReference to the ViewController prefab. Must be called
        /// before the ViewController is pushed.
        /// </summary>
        /// <param name="assetReference"></param>
        /// <typeparam name="TViewController"></typeparam>
        /// <typeparam name="TViewControllerModel"></typeparam>
        public void RegisterViewController<TViewController, TViewControllerModel>(AssetReference assetReference) where TViewController : ViewController<TViewControllerModel>
        {
            _modelTypeToViewType.Add(typeof(TViewControllerModel), typeof(TViewController));
            _viewTypeToAssetReference.Add(typeof(TViewController), assetReference);
        }

        /// <summary>
        /// Push the ViewController with the requested type to the stack.
        /// </summary>
        /// <param name="model">The model for the ViewController</param>
        /// <typeparam name="TViewController">The type of ViewController to push to the stack.</typeparam>
        /// <typeparam name="TViewControllerModel">The type of model the ViewController will be initialized with.</typeparam>
        /// <returns>The pushed ViewController.</returns>
        /// <exception cref="ViewNotRegisteredException{TViewController}"></exception>
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

        /// <summary>
        /// Pop the current ViewController.
        /// </summary>
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

        /// <summary>
        /// Pop the stack until the requested type is reached.
        /// </summary>
        /// <param name="alsoPopRequested">If true, the view controller with the requested type will also be popped.</param>
        /// <typeparam name="TViewController"></typeparam>
        public async Task PopUntil<TViewController>(bool alsoPopRequested) where TViewController : IViewController
        {
            if (!_stack.Any(x => x is TViewController))
            {
                Debug.LogError($"No ViewController of type {typeof(TViewController)} exists ont the stack.");
                return;
            }

            while (!(_stack.Peek() is TViewController))
                await Pop();

            if (alsoPopRequested)
                await Pop();
        }

        /// <summary>
        /// Pop the stack until view controller is reached.
        /// </summary>
        /// <param name="viewController">The instance of the view controller to pop to.</param>
        /// <param name="alsoPopRequested">If true, the view controller with the requested type will also be popped.</param>
        public async Task PopUntil(IViewController viewController, bool alsoPopRequested)
        {
            if (_stack.All(x => x != viewController))
            {
                Debug.LogError($"The requested view controller is not on the stack.");
                return;
            }

            while (_stack.Peek() != viewController)
                await Pop();

            if (alsoPopRequested)
                await Pop();
        }

        /// <summary>
        /// Pop everything off the view stack.
        /// </summary>
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