using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VeryGenericContainer.Examples {
    
    public class FoodItem : ContainerItem<Food> {

        [SerializeField] private Text labelText;

        public override void Initialize(Food food) {
            base.Initialize(food);

            labelText.text = food.Name;
        }
    }
}
