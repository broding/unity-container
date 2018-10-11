using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ContainerTests {

    [TearDown]
    public void DestroyExistingContainer() {
        TestContainer testContainer = GameObject.FindObjectOfType<TestContainer>();
        if(testContainer != null) {
            GameObject.DestroyImmediate(testContainer.gameObject);
        }
    }

    private TestContainer CreateTestContainer() {
        GameObject containerGo = new GameObject();
        GameObject itemGo = new GameObject();
        itemGo.transform.SetParent(containerGo.transform);
        TestContainer container = containerGo.AddComponent<TestContainer>();
        TestItem item = itemGo.AddComponent<TestItem>();
        container.ContainerItemTemplate = item;
        container.Initialize();

        return container;
    }

    private List<Weapon> CreateWeaponsList(int amount) {
        List<Weapon> weapons = new List<Weapon>();

        for(int i = 0; i < amount; i++) {
            weapons.Add(new Weapon());
        }

        return weapons;
    }

    [Test]
    public void UpdateContainer_IsItemCountCorrect() {
        TestContainer container = CreateTestContainer();
        IEnumerable<Weapon> weapons = CreateWeaponsList(5);
        container.UpdateContainer(weapons);

        Assert.AreEqual(container.Items.Count(), 5);
    }

    [Test]
    public void UpdateContainer_AreGameObjectsCorrectlySpawned() {
        TestContainer container = CreateTestContainer();
        IEnumerable<Weapon> weapons = CreateWeaponsList(5);
        container.UpdateContainer(weapons);

        Assert.AreEqual(5, GameObject.FindObjectsOfType<TestItem>().Length);
    }

    [Test]
    public void DestroyItem_IsItemDestroyed() {
        TestContainer container = CreateTestContainer();
        IEnumerable<Weapon> weapons = CreateWeaponsList(5);
        container.UpdateContainer(weapons);
        container.DestroyItem(weapons.First());

        Assert.AreEqual(container.Items.Count(), 4);
    }

    [Test]
    public void DestroyItem_IsItemsEmpty() {
        TestContainer container = CreateTestContainer();
        List<Weapon> weapons = CreateWeaponsList(5);
        container.UpdateContainer(weapons);

        for(int i = 0; i < 5; i++) {
            Weapon weaponToDelete = weapons.First();
            container.DestroyItem(weaponToDelete);
            weapons.Remove(weaponToDelete);
        }

        Assert.AreEqual(0, container.Items.Count());
    }

    [UnityTest]
    public IEnumerator DestroyItem_AreItemGameObjectsDestroyed() {
        TestContainer container = CreateTestContainer();
        List<Weapon> weapons = CreateWeaponsList(5);
        container.UpdateContainer(weapons);

        for (int i = 0; i < 5; i++) {
            Weapon weaponToDelete = weapons.First();
            container.DestroyItem(weaponToDelete);
            weapons.Remove(weaponToDelete);
        }

        yield return new WaitForEndOfFrame();

        Assert.AreEqual(0, GameObject.FindObjectsOfType<TestItem>().Length);
    }

    [Test]
    public void Clear_IsItemsEmpty() {
        TestContainer container = CreateTestContainer();
        IEnumerable<Weapon> weapons = CreateWeaponsList(5);
        container.UpdateContainer(weapons);
        container.Clear();

        Assert.AreEqual(0, container.Items.Count());
    }

    [Test]
    public void CreateItem_IsItemsCountIncreased() {
        TestContainer container = CreateTestContainer();
        IEnumerable<Weapon> weapons = CreateWeaponsList(5);
        container.UpdateContainer(weapons);
        Weapon weapon = new Weapon();
        container.CreateItem(weapon);

        Assert.AreEqual(6, container.Items.Count());
    }

    [UnityTest]
    public IEnumerator Clear_AreGameObjectsDestroyed() {
        TestContainer container = CreateTestContainer();
        IEnumerable<Weapon> weapons = CreateWeaponsList(5);
        container.UpdateContainer(weapons);
        container.Clear();

        yield return new WaitForEndOfFrame();

        Assert.AreEqual(0, GameObject.FindObjectsOfType<TestItem>().Length);
    }
}
