using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VeryGenericContainer {

    public class PoolContainer<U, T> : Container<U, T> where U : ContainerItem<T> {

        [SerializeField] public int InitialPoolSize = 20;
        [SerializeField] public int MaxPoolSize = 100;
        [SerializeField] private bool InitializeOnAwake = true;

        private readonly Stack<U> pool = new Stack<U>();

        protected override void Awake() {
            base.Awake();

            if (InitializeOnAwake) {
                Initialize();
            }
        }

        public override void Initialize() {
            base.Initialize();

            for(int i = 0; i < InitialPoolSize; i++) {
                U item = Instantiate(ItemTemplate);
                item.transform.SetParent(ItemTemplate.transform.parent, false);
                item.gameObject.SetActive(false);
                pool.Push(item);
            }
        }

        public override void UpdateContainer(IEnumerable<T> dataCollection) {
            Clear();

            base.UpdateContainer(dataCollection);
        }

        protected override U InstantiateTemplate(T data) {
            if(pool.Count > 0) {
                U item = pool.Pop();
                item.transform.SetAsLastSibling();
                return item;
            } else {
                return Instantiate(ItemTemplate);
            }
        }

        protected override void DestroyGameObject(U item) {
            item.gameObject.SetActive(false);
            pool.Push(item);
        }
    }

}

