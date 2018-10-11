using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasRoding.UnityContainer {

    public class PoolContainer<U, T> : Container<U, T> where U : ContainerItem<T> {

        [SerializeField] private int initialPoolSize = 20;
        [SerializeField] private int maxPoolSize;

        private Stack<U> pool = new Stack<U>();

        protected override void Awake() {
            base.Awake();

            for(int i = 0; i < initialPoolSize; i++) {
                U item = Instantiate(containerItemTemplate);
                item.transform.SetParent(containerItemTemplate.transform.parent, false);
                item.gameObject.SetActive(false);
                pool.Push(item);
            }
        }

        protected override U InstantiateTemplate(T data) {
            if(pool.Count > 0) {
                return pool.Pop();
            } else {
                return Instantiate(containerItemTemplate);
            }
        }

        protected override void DestroyGameObject(U item) {
            item.gameObject.SetActive(false);
            pool.Push(item);
        }
    }

}

