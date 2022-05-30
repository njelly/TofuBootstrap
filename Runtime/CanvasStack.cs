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
    public class CanvasStack
    {
        private readonly Stack<IUIController> _stack;
        private readonly Canvas _canvas;
        private readonly Dictionary<Type, Type> _modelTypeToViewType;
        private readonly Dictionary<Type, AssetReference> _viewTypeToAssetReference;

        public CanvasStack(Canvas canvas)
        {
            _stack = new Stack<IUIController>();
            _modelTypeToViewType = new Dictionary<Type, Type>();
            _viewTypeToAssetReference = new Dictionary<Type, AssetReference>();

            _canvas = canvas;
        }
        
        public CanvasStack(CanvasModel canvasModel)
        {
            _stack = new Stack<IUIController>();
            _modelTypeToViewType = new Dictionary<Type, Type>();
            _viewTypeToAssetReference = new Dictionary<Type, AssetReference>();

            _canvas = canvasModel.Build();
        }

        /// <summary>
        /// Register a CanvasViewController/Model pair, allow with the AssetReference to the CanvasViewController prefab. Must be called
        /// before the CanvasViewController is pushed.
        /// </summary>
        /// <param name="assetReference"></param>
        /// <typeparam name="TUIController"></typeparam>
        /// <typeparam name="TUIModel"></typeparam>
        public void RegisterController<TUIController, TUIModel>(AssetReference assetReference) where TUIController : UIController<TUIModel>
        {
            _modelTypeToViewType.Add(typeof(TUIModel), typeof(TUIController));
            _viewTypeToAssetReference.Add(typeof(TUIController), assetReference);
        }

        /// <summary>
        /// Push the CanvasViewController with the requested type to the stack.
        /// </summary>
        /// <param name="model">The model for the CanvasViewController</param>
        /// <typeparam name="TUIController">The type of CanvasViewController to push to the stack.</typeparam>
        /// <typeparam name="TUIModel">The type of model the CanvasViewController will be initialized with.</typeparam>
        /// <returns>The pushed CanvasViewController.</returns>
        /// <exception cref="UIControllerNotRegisteredException{TViewController}"></exception>
        public async Task<TUIController> Push<TUIController, TUIModel>(TUIModel model) where TUIController : UIController<TUIModel>
        {
            if (!_modelTypeToViewType.TryGetValue(typeof(TUIModel), out var viewType))
                throw new UIControllerNotRegisteredException<TUIController>();

            if (!_viewTypeToAssetReference.TryGetValue(viewType, out var assetReference))
                throw new UIControllerNotRegisteredException<TUIController>();

            var go = await Addressables.InstantiateAsync(assetReference, _canvas.transform).Task;
            var uiController = go.GetComponent<TUIController>();

            if (_stack.Count > 0)
                await _stack.Peek().OnLostFocus();
            
            await uiController.OnPushedToStack(model);
            await uiController.OnGainedFocus();
            
            _stack.Push(uiController);

            return uiController;
        }

        /// <summary>
        /// Pop the current CanvasViewController.
        /// </summary>
        public async Task Pop()
        {
            if (_stack.Count <= 0)
                return;

            var viewController = _stack.Pop();
            await viewController.OnLostFocus();
            await viewController.OnPoppedFromStack();

            Addressables.ReleaseInstance(viewController.GameObject);

            if (_stack.Count <= 0)
                return;

            await _stack.Peek().OnGainedFocus();
        }

        /// <summary>
        /// Pop the stack until the requested type is reached.
        /// </summary>
        /// <param name="alsoPopRequested">If true, the view controller with the requested type will also be popped.</param>
        /// <typeparam name="TCanvasViewController"></typeparam>
        public async Task PopUntil<TCanvasViewController>(bool alsoPopRequested) where TCanvasViewController : IUIController
        {
            if (!_stack.Any(x => x is TCanvasViewController))
            {
                Debug.LogError($"No CanvasViewController of type {typeof(TCanvasViewController)} exists ont the stack.");
                return;
            }

            while (!(_stack.Peek() is TCanvasViewController))
                await Pop();

            if (alsoPopRequested)
                await Pop();
        }

        /// <summary>
        /// Pop the stack until view controller is reached.
        /// </summary>
        /// <param name="canvasViewController">The instance of the view controller to pop to.</param>
        /// <param name="alsoPopRequested">If true, the view controller with the requested type will also be popped.</param>
        public async Task PopUntil(IUIController canvasViewController, bool alsoPopRequested)
        {
            if (_stack.All(x => x != canvasViewController))
            {
                Debug.LogError($"The requested view controller is not on the stack.");
                return;
            }

            while (_stack.Peek() != canvasViewController)
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

        public class UIControllerNotRegisteredException<UIController> : Exception where UIController : IUIController
        {
            public UIControllerNotRegisteredException() : base(
                $"the view controller {typeof(UIController)} has not been registered") { }
        }
    }
}