using Assets.Scripts.Entities;
using Assets.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public TextMeshProUGUI title;
    public Item itemData;
    public bool isEquipped = false;
    public Button button;
    public Inventory inventory;

    public Texture2D attachedTex, detachedTex;

    private void Awake()
    {
        button.onClick.AddListener(Equip);
    }


    private void Start()
    {
        UpdateStatus();
    }


    public void UpdateStatus()
    {
        if (isEquipped)
            button.gameObject.GetComponent<RawImage>().texture = attachedTex;
        else
            button.gameObject.GetComponent<RawImage>().texture = detachedTex;


        inventory.owner.UpdateMountedTurrets();
        inventory.owner.UpdateAvailableGuns();
    }


    private void Equip()
    {
        isEquipped = !isEquipped;
        itemData.isMounted = isEquipped;

        if(!isEquipped)
        {
            itemData.prefab.transform.parent.GetComponent<Hardpoint>().RemoveTurret();
        }
        else
        {
            Transform parent = null;
            Hardpoint hp = null;
            foreach (Transform hardpoint in inventory.owner.hardPoints)
            {
                hp = hardpoint.gameObject.GetComponent<Hardpoint>();

                if (hardpoint.childCount == 0 && hp.isTurret == itemData.isTurret)
                {
                    parent = hardpoint;
                    break;
                }
            }
            if (parent != null)
            {
                var v = Instantiate(Resources.Load<GameObject>(itemData.prefabLocation), parent);
                v.GetComponent<Turret>().owner = inventory.owner;
                v.GetComponent<Turret>().isTurret = hp.isTurret;
                hp.assetPath = itemData.prefabLocation;
                v.transform.parent.GetComponent<Hardpoint>().assetPath = v.GetComponent<Turret>().assetPath;
                isEquipped = true;
                itemData.prefab = v;
                itemData.isMounted = isEquipped;
            }
        }

        UpdateStatus();

    }

    private void AsyncOpCompleted(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Transform parent = null;
            Hardpoint hp = null;
            foreach (Transform hardpoint in inventory.owner.hardPoints)
            {
                hp = hardpoint.gameObject.GetComponent<Hardpoint>();

                if (hardpoint.childCount == 0 && hp.isTurret == itemData.isTurret)
                {
                    parent = hardpoint;
                    break;
                }
            }

            if (parent != null)
            {
                var v = Instantiate(obj.Result, parent);
                v.GetComponent<Turret>().owner = inventory.owner;
                v.GetComponent<Turret>().isTurret = hp.isTurret;
                v.transform.parent.GetComponent<Hardpoint>().assetPath = v.GetComponent<Turret>().assetPath;
                isEquipped = true;
                itemData.prefab = v;
                itemData.isMounted = isEquipped;
                UpdateStatus();
            }
            
        }
    }

}
