using UnityEngine;
namespace AtoLib {
    public abstract class ViewDisplayer<TModel> : MonoBehaviour {
        public TModel Model { get; private set; }

        public abstract void Show();

        public ViewDisplayer<TModel> SetModel(TModel model) {
            Model = model;
            return this;
        }
    }
}