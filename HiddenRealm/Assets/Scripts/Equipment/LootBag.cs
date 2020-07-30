using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class LootBag : NetworkBehaviour
{
    public Item LootItem { get; set; }

    [SyncVar(hook = "OnSetItem")]
    public int itemDatabaseIndex;

    [SyncVar(hook = "SetupNameplate")]
    public string lootName;
    [SyncVar]
    public int upgradeLvl;
    [SyncVar]
    public int quantity;

    [SerializeField]
    private TextMeshProUGUI nameplate = null;

    private void Start()
    {
        Invoke("AutoDestruction", 180f);
    }

    private void Update()
    {
        if(nameplate.text != lootName)
        {
            nameplate.text = lootName;
        }
    }

    private void SetupNameplate(string oldVal, string newVal)
    {
        nameplate.text = newVal;
    }

    private void AutoDestruction()
    {
        NetworkServer.Destroy(gameObject);
    }

    private void OnSetItem(int oldVal, int index)
    {
        if(index != -2)
        {
            LootItem = ItemsDatabase.instance.itemList[index];
            Debug.Log("hook worked! hooked item: " + LootItem.itemName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerRpg>() != null)
        {
            collision.gameObject.GetComponent<PlayerUI>().AddToLootables(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerRpg>() != null)
        {
            collision.gameObject.GetComponent<PlayerUI>().RemoveFromLootables(gameObject);
        }
    }
}
