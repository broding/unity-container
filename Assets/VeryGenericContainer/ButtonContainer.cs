using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VeryGenericContainer {

    public abstract class ButtonContainer<U, T> : Container<U, T> where U : ButtonContainerItem<T> {

        public abstract void OnItemClicked(T item);

        protected override void OnItemAdded(U item) {
            base.OnItemAdded(item);
            item.ButtonClicked.AddListener(OnItemClicked);
        }

        protected override void OnItemDestroyed(U item) {
            base.OnItemDestroyed(item);
            item.ButtonClicked.RemoveListener(OnItemClicked);
        }
    }

    public abstract class ButtonContainerItem<T> : ContainerItem<T> {

        [SerializeField]
        protected Button button;

        public readonly ButtonEvent<T> ButtonClicked = new ButtonEvent<T>();

        protected virtual void Start() {
            button.onClick.AddListener(OnButtonClicked);
        }

        protected virtual void OnDestroy() {
            button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked() {
            ButtonClicked.Invoke(Data);
        }
    }

    public class ButtonEvent<T> : UnityEvent<T> { }

}