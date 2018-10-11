using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasRoding.UnityContainer;

public class TestContainer : PoolContainer<TestItem, Weapon> {

    private void Start() {
        List<Weapon> weapons = new List<Weapon>();
        weapons.Add(new Weapon());
        weapons.Add(new Weapon());
        weapons.Add(new Weapon());
        weapons.Add(new Weapon());
        weapons.Add(new Weapon());

        UpdateContainer(weapons);
    }

}

public class Weapon {

}
