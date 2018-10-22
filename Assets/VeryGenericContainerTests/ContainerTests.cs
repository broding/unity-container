using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class ContainerTests {

    [TearDown]
    public void DestroyExistingContainer() {
        DummyContainer testContainer = GameObject.FindObjectOfType<DummyContainer>();
        if(testContainer != null) {
            GameObject.DestroyImmediate(testContainer.gameObject);
        }
    }

    private DummyContainer CreateTestContainer() {
        GameObject containerGo = new GameObject();
        GameObject itemGo = new GameObject();
        itemGo.transform.SetParent(containerGo.transform);
        DummyContainer container = containerGo.AddComponent<DummyContainer>();
        DummyItem item = itemGo.AddComponent<DummyItem>();
        container.ItemTemplate = item;
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
        DummyContainer container = CreateTestContainer();
        IEnumerable<Weapon> weapons = CreateWeaponsList(5);
        container.UpdateContainer(weapons);

        Assert.AreEqual(container.Items.Count(), 5);
    }

    [Test]
    public void UpdateContainer_AreGameObjectsCorrectlySpawned() {
        DummyContainer container = CreateTestContainer();
        IEnumerable<Weapon> weapons = CreateWeaponsList(5);
        container.UpdateContainer(weapons);

        Assert.AreEqual(5, GameObject.FindObjectsOfType<DummyItem>().Length);
    }

    [UnityTest]
    public IEnumerator UpdateContainer_AreOldItemsDestroyed() {
        DummyContainer container = CreateTestContainer();
        IEnumerable<Weapon> oldWeapons = CreateWeaponsList(5);
        container.UpdateContainer(oldWeapons);
        
        IEnumerable<Weapon> newWeapons = CreateWeaponsList(5);
        container.UpdateContainer(newWeapons);

        yield return new WaitForEndOfFrame();
        
        Assert.AreEqual(5, GameObject.FindObjectsOfType<DummyItem>().Length);
    }

    [Test]
    public void UpdateContainer_IsItemCountCorrectAfterNewUpdateContainer() {
        DummyContainer container = CreateTestContainer();
        IEnumerable<Weapon> oldWeapons = CreateWeaponsList(5);
        container.UpdateContainer(oldWeapons);

        IEnumerable<Weapon> newWeapons = CreateWeaponsList(5);
        container.UpdateContainer(newWeapons);

        Assert.AreEqual(5, container.Items.Count());
    }

    [Test]
    public void UpdateContainer_AreOldItemsNotFoundAfterNewUpdateContainer() {
        DummyContainer container = CreateTestContainer();
        IEnumerable<Weapon> oldWeapons = CreateWeaponsList(5);
        container.UpdateContainer(oldWeapons);

        IEnumerable<Weapon> newWeapons = CreateWeaponsList(5);
        container.UpdateContainer(newWeapons);

        LogAssert.ignoreFailingMessages = true;
        container.GetItem(oldWeapons.First());
        LogAssert.Expect(LogType.Error, new Regex(".*"));
    }

    [Test]
    public void DestroyItem_IsItemDestroyed() {
        DummyContainer container = CreateTestContainer();
        IEnumerable<Weapon> weapons = CreateWeaponsList(5);
        container.UpdateContainer(weapons);
        container.DestroyItem(weapons.First());

        Assert.AreEqual(container.Items.Count(), 4);
    }

    [Test]
    public void DestroyItem_IsItemsEmpty() {
        DummyContainer container = CreateTestContainer();
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
        DummyContainer container = CreateTestContainer();
        List<Weapon> weapons = CreateWeaponsList(5);
        container.UpdateContainer(weapons);

        for (int i = 0; i < 5; i++) {
            Weapon weaponToDelete = weapons.First();
            container.DestroyItem(weaponToDelete);
            weapons.Remove(weaponToDelete);
        }

        yield return new WaitForEndOfFrame();

        Assert.AreEqual(0, GameObject.FindObjectsOfType<DummyItem>().Length);
    }

    [Test]
    public void Clear_IsItemsEmpty() {
        DummyContainer container = CreateTestContainer();
        IEnumerable<Weapon> weapons = CreateWeaponsList(5);
        container.UpdateContainer(weapons);
        container.Clear();

        Assert.AreEqual(0, container.Items.Count());
    }

    [Test]
    public void CreateItem_IsItemsCountIncreased() {
        DummyContainer container = CreateTestContainer();
        IEnumerable<Weapon> weapons = CreateWeaponsList(5);
        container.UpdateContainer(weapons);
        Weapon weapon = new Weapon();
        container.CreateItem(weapon);

        Assert.AreEqual(6, container.Items.Count());
    }

    [UnityTest]
    public IEnumerator Clear_AreGameObjectsDestroyed() {
        DummyContainer container = CreateTestContainer();
        IEnumerable<Weapon> weapons = CreateWeaponsList(5);
        container.UpdateContainer(weapons);
        container.Clear();

        yield return new WaitForEndOfFrame();

        Assert.AreEqual(0, GameObject.FindObjectsOfType<DummyItem>().Length);
    }

    [Test]
    public void GetItem_IsItemSame() {
        DummyContainer container = CreateTestContainer();
        IEnumerable<Weapon> weapons = CreateWeaponsList(5);
        container.UpdateContainer(weapons);
        Weapon weapon = new Weapon();
        container.CreateItem(weapon);

        DummyItem item = container.GetItem(weapon);

        Assert.AreSame(weapon, item.Data);
    }

    [Test]
    public void GetItem_IsNewItemNotFound() {
        LogAssert.ignoreFailingMessages = true;

        DummyContainer container = CreateTestContainer();
        IEnumerable<Weapon> weapons = CreateWeaponsList(5);
        container.UpdateContainer(weapons);
        Weapon nonAddedWeapon = new Weapon();

        container.GetItem(nonAddedWeapon);
        LogAssert.Expect(LogType.Error, new Regex(".*"));
    }
}
