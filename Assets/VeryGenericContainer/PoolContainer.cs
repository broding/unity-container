using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasRoding.VeryGenericContainer {

    public class PoolContainer<U, T> : Container<U, T> where U : ContainerItem<T> {

        [SerializeField] public int InitialPoolSize = 20;
        [SerializeField] public int MaxPoolSize = 100;

        private readonly Stack<U> pool = new Stack<U>();

        public override void Initialize() {
            base.Initialize();

            for(int i = 0; i < InitialPoolSize; i++) {
                U item = Instantiate(ContainerItemTemplate);
                item.transform.SetParent(ContainerItemTemplate.transform.parent, false);
                item.gameObject.SetActive(false);
                pool.Push(item);
            }
        }

        protected override U InstantiateTemplate(T data) {
            if(pool.Count > 0) {
                return pool.Pop();
            } else {
                return Instantiate(ContainerItemTemplate);
            }
        }

        protected override void DestroyGameObject(U item) {
            item.gameObject.SetActive(false);
            pool.Push(item);
        }
    }

}

