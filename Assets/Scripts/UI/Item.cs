using Assets.Scripts.Entities;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public class Item
{

    public static int ID = 0;


    [NonSerialized]public int id;
    public string Name;
    public string hardpointName;
    public bool isMounted = false;
    public bool isTurret = false;
    public string prefabLocation;

    [NonSerialized]public GameObject prefab;

    public Item(Ship ship, int mountedTurretIndex)
    {
        Init(ship, ID++, mountedTurretIndex);
    }

    public Item()
    {
        this.id = ID++;
    }

    private void Init(Ship ship, int id, int mountedTurretIndex)
    {
        prefabLocation = ship.mountedTurrets[mountedTurretIndex].assetPath;
        prefab = ship.mountedTurrets[mountedTurretIndex].gameObject;
        this.id = id;   
        Name = ship.mountedTurrets[mountedTurretIndex].Name;
        isMounted = true;
        isTurret = ship.mountedTurrets[mountedTurretIndex].isTurret;
        if(prefab != null )
        {
            hardpointName = prefab.transform.parent.name;
        }
    }
    
    public override bool Equals(object other)
    {
        Item item = other as Item;
        return id == item.id;
    }

    public override int GetHashCode()
    {
        return id;
    }
}
