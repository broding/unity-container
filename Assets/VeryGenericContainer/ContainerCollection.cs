using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace BasRoding.VeryGenericContainer {

    public interface IContainerCollection<T> : IEnumerable<T> {
        ContainerEvent<T> AddedItem { get; }
        ContainerEvent<T> RemovedItem { get; }
    }

    public class ContainerEvent<T> : UnityEvent<T> { }

    public class ContainerCollection<T> : IContainerCollection<T> {

        public ContainerEvent<T> AddedItem {
            get { return addedItem; }
        }

        public ContainerEvent<T> RemovedItem {
            get { return removedItem; }
        }

        private List<T> internalList;
        private ContainerEvent<T> addedItem = new ContainerEvent<T>();
        private ContainerEvent<T> removedItem = new ContainerEvent<T>();

        public ContainerCollection() {
            internalList = new List<T>();
        }

        public bool Contains(T item) {
            return internalList.Contains(item);
        }

        public void Add(T item) {
            internalList.Add(item);
            AddedItem.Invoke(item);
        }

        public void Remove(T item) {
            if (internalList.Remove(item)) {
                RemovedItem.Invoke(item);
            }
        }

        public IEnumerator<T> GetEnumerator() {
            return internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return internalList.GetEnumerator();
        }

        public T this[int index] {
            get { return internalList[index]; }
        }

        public int Count {
            get {
                return internalList.Count;
            }
        }
    }
}

