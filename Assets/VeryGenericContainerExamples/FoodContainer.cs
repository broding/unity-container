using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VeryGenericContainer.Examples {
    
    public class FoodContainer : Container<FoodItem, Food> {

        private void Start() {
            List<Food> foods = new List<Food>();

            foods.Add(new Food("Apple"));
            foods.Add(new Food("Bread"));
            foods.Add(new Food("Pasta"));
            foods.Add(new Food("Chocolate"));
            foods.Add(new Food("Yoghurt"));

            UpdateContainer(foods);
        }

    }
}

