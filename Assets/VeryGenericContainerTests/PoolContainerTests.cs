using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolContainerTests : MonoBehaviour {

    [TearDown]
    public void DestroyExistingContainer() {
        DummyContainer testContainer = GameObject.FindObjectOfType<DummyContainer>();
        if (testContainer != null) {
            GameObject.DestroyImmediate(testContainer.gameObject);
        }
    }

    private DummyPoolContainer CreateTestContainer() {
        GameObject containerGo = new GameObject();
        GameObject itemGo = new GameObject();
        itemGo.transform.SetParent(containerGo.transform);
        DummyPoolContainer container = containerGo.AddComponent<DummyPoolContainer>();
        DummyItem item = itemGo.AddComponent<DummyItem>();
        container.ContainerItemTemplate = item;
        container.Initialize();

        return container;
    }

    private List<Weapon> CreateWeaponsList(int amount) {
        List<Weapon> weapons = new List<Weapon>();

        for (int i = 0; i < amount; i++) {
            weapons.Add(new Weapon());
        }

        return weapons;
    }

    [Test]
    public void IsInitializedWithInitialPoolSize() {
        DummyPoolContainer container = CreateTestContainer();

        Assert.AreEqual(container.transform.childCount, 21);
    }

    [Test]
    public void IsInitialPoolSizeAllDeactivated() {
        DummyPoolContainer container = CreateTestContainer();

        foreach(Transform child in container.transform) {
            Assert.IsFalse(child.gameObject.activeSelf);
        }
    }

    [Test]
    public void UpdateContainer_IsItemCountCorrect() {
        DummyPoolContainer container = CreateTestContainer();
        IEnumerable<Weapon> weapons = CreateWeaponsList(5);
        container.UpdateContainer(weapons);

        Assert.AreEqual(container.Items.Count(), 5);
    }

    [Test]
    public void UpdateContainer_AreNewItemsNotInstanstiated() {
        DummyPoolContainer container = CreateTestContainer();
        List<Weapon> weapons = CreateWeaponsList(1);

        Assert.AreEqual(container.transform.childCount, 21);

        container.UpdateContainer(weapons);

        Assert.AreEqual(container.transform.childCount, 21);
    }

    [Test]
    public void UpdateContainer_IsItemActivated() {
        DummyPoolContainer container = CreateTestContainer();
        List<Weapon> weapons = CreateWeaponsList(1);

        container.UpdateContainer(weapons);

        Assert.AreEqual(container.GetComponentsInChildren<DummyItem>().Length, 1);
    }

    [Test]
    public void Clear_ArePoolItemsNotDestroyed() {
        DummyPoolContainer container = CreateTestContainer();
        List<Weapon> weapons = CreateWeaponsList(1);

        container.UpdateContainer(weapons);
        container.Clear();

        Assert.AreEqual(container.transform.childCount, 21);
    }
}
