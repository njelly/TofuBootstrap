using Tofunaut.Bootstrap.Interfaces;
using UnityEngine;

namespace Tofunaut.Bootstrap
{
    public abstract class ModeledComponent<TModel> : MonoBehaviour, IModeledComponent where TModel : class
    {
        public abstract void Initialize(TModel model);
        public void Initialize(object model) => Initialize((TModel)model);
    }
}
