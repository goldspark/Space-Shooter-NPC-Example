using Assets.Scripts.Controller;
using Assets.Scripts.Entities;
using Assets.Scripts.UI;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;



public class Inventory
{
    public List<Item> items;
    public Ship owner;

    public UnityAction OnInventoryChanged;

    public Inventory(Ship owner)
    {
        items = new List<Item>(3);
        OnInventoryChanged = delegate { };
        this.owner = owner;
    }


    public void AddItem(Item item)
    {
        if(!item.isMounted)
            items.Add(item);
        else
            items.Insert(0, item);

        OnInventoryChanged.Invoke();     
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        OnInventoryChanged.Invoke();
    }

    public void AddEquippedWeapons(Ship ship)
    {
        for (int i = 0; i < ship.mountedTurrets.Count; i++)
        {
            AddItem(new Item(ship, i));
        }

    }




}
