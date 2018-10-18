using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VeryGenericContainer.Examples {
    
    public class FoodItem : ContainerItem<Food> {

        [SerializeField] private Text labelText;

        public override void OnSetup(Food data) {
            base.Setup(data);

            labelText.text = data.Name;
        }

        public override void OnDispose() {
        }
    }
}
