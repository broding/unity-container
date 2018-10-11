using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace BasRoding.UnityContainer {

    public abstract class ContainerItem<T> : MonoBehaviour {
        public T Data { get; private set; }

        public virtual void Initialize(T data) {
            Data = data;
        }

        public virtual void UpdateItem(T data) {
            Data = data;
        }

        public virtual void Dispose() {

        }
    }

    public abstract class Container : MonoBehaviour {

    }

    public abstract class Container<U, T> : Container where U : ContainerItem<T> {

        [SerializeField] protected U containerItemTemplate;

        private IContainerCollection<T> internalCollection;
        private List<U> items = new List<U>();

        public IEnumerable<U> Items {
            get {
                return items;
            }
        }

        public void UpdateContainer(IContainerCollection<T> collection) {
            internalCollection = collection;
            UpdateContainer(internalCollection as IEnumerable<T>);

            internalCollection.AddedItem.AddListener(OnItemAdded);
            internalCollection.RemovedItem.AddListener(OnItemRemoved);
        }

        public U CreateItem(T data) {
            U oldItem = items.FirstOrDefault(i => i.Data.Equals(data));
            if (oldItem != null) {
                oldItem.UpdateItem(data);
                return oldItem;
            }

            U item = InstantiateItem(data);

            AddItemToList(item);

            return item;
        }

        public U GetItem(T data) {
            U item = items.FirstOrDefault(v => v.Data.Equals(data));

            if (item == null) {
                Debug.LogError("Could not find item with data " + data);
            }

            return item;
        }

        public void DestroyItem(T data) {
            U item = Items.FirstOrDefault(i => i.Data.Equals(data));

            if (item == null) {
                throw new System.Exception("Can't delete item because it is not in the list");
            }
            DestroyItem(item);
        }

        public void DestroyItem(U item) {
            OnItemDestroyed(item);
            item.Dispose();
            items.Remove(item);
            DestroyGameObject(item);
        }

        public void Clear() {
            foreach (U item in new List<U>(items)) {
                DestroyItem(item);
            }
        }

        public virtual void UpdateContainer(IEnumerable<T> dataCollection) {
            List<U> oldItems = new List<U>(items);
            items.Clear();

            foreach (T data in dataCollection) {
                U item = oldItems.FirstOrDefault(i => i.Data != null && i.Data.Equals(data));
                if (item != null) {
                    item.UpdateItem(data);
                    oldItems.Remove(item);
                    items.Add(item);
                } else {
                    item = InstantiateItem(data);
                    AddItemToList(item);
                }
            }

            DestroyItems(oldItems);
        }

        protected virtual void Awake() {
            if (containerItemTemplate == null) {
                throw new System.Exception("Container item template is null at " + gameObject.name);
            }

            containerItemTemplate.gameObject.SetActive(false);
        }

        protected virtual void OnDestroy() {
            if (internalCollection != null) {
                internalCollection.AddedItem.RemoveListener(OnItemAdded);
                internalCollection.RemovedItem.RemoveListener(OnItemRemoved);
            }
        }

        protected virtual void DestroyGameObject(U item) {
            GameObject.Destroy(item.gameObject);
        }

        private void OnItemAdded(T data) {
            CreateItem(data);
        }

        private void OnItemRemoved(T data) {
            DestroyItem(data);
        }

        protected virtual U InstantiateTemplate(T data) {
            return Instantiate(containerItemTemplate);
        }

        private U InstantiateItem(T data) {
            U item = InstantiateTemplate(data);
            item.transform.SetParent(containerItemTemplate.transform.parent, false);
            item.gameObject.SetActive(true);
            item.Initialize(data);
            item.UpdateItem(data);

            return item;
        }

        private void AddItemToList(U item) {
            items.Add(item);
            OnItemAdded(item);
        }

        private void DestroyItems(List<U> items) {
            foreach (U item in items) {
                DestroyItem(item);
            }
        }

        protected virtual void OnItemAdded(U item) {

        }

        protected virtual void OnItemDestroyed(U item) {

        }

    }

}