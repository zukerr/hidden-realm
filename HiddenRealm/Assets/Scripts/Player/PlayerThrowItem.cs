using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerThrowItem : NetworkBehaviour
{
    [SerializeField]
    private GameObject lootbagPrefab = null;

    private GameObject lootbag;

    public void ThrowItem(GameObject itemGO)
    {
        if(isLocalPlayer)
        {
            int id = ItemsDatabase.instance.GetIndexOfItem(itemGO.GetComponent<ItemUI>().item);
            int upgradeLvl = itemGO.GetComponent<ItemUI>().UpgradeLevel;
            int quantity = itemGO.GetComponent<ItemUI>().GetQuantity();
            string iName = itemGO.GetComponent<ItemUI>().GetStringFullname();
            CmdThrowItem(id, upgradeLvl, quantity, iName);
            itemGO.GetComponent<ItemUI>().inventory.ClearInventorySlot(itemGO.GetComponent<ItemUI>());
            itemGO.GetComponent<ItemUI>().DestroyObservers();
            Destroy(itemGO);
        }
    }

    [Command]
    private void CmdThrowItem(int itemDatabaseId, int itemUpgradeLvl, int itemQuantity, string itemName)
    {
        lootbag = Instantiate(lootbagPrefab, transform.position, Quaternion.identity);
        Debug.Log("instantiated lootbag.");
        NetworkServer.Spawn(lootbag);
        Debug.Log("spawned lootbag.");
        lootbag.GetComponent<LootBag>().itemDatabaseIndex = -2;
        lootbag.GetComponent<LootBag>().itemDatabaseIndex = itemDatabaseId;
        lootbag.GetComponent<LootBag>().quantity = itemQuantity;
        lootbag.GetComponent<LootBag>().upgradeLvl = itemUpgradeLvl;
        lootbag.GetComponent<LootBag>().lootName = itemName;
        Debug.Log("set lootbag up.");
        Debug.Log("quan: " + lootbag.GetComponent<LootBag>().quantity);
    }
}
