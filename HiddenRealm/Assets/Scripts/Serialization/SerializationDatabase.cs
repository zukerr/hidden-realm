using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializationDatabase : MonoBehaviour
{
    public static SerializationDatabase instance;

    private List<SerialItem> serializedItems;
    [SerializeField]
    private GameObject itemsParentObject = null;
    [SerializeField]
    public Inventory inventory;
    [SerializeField]
    public DatabaseControl databaseControl;
    [SerializeField]
    public ActionBarSerialization abs;

    public bool loadedIn = false;

    private void Start()
    {
        instance = this;
    }

    private void SerializeItems()
    {
        serializedItems = new List<SerialItem>();

        for(int i = 0; i < itemsParentObject.transform.childCount; i++)
        {
            serializedItems.Add(GlobalSerialization.SerializeItem(itemsParentObject.transform.GetChild(i).GetComponent<ItemUI>()));
        }
    }

    //this is called on clients, assigned to button for tests
    public void SaveItemsToDatabase()
    {
        inventory.GetPlayerRpg().gameObject.GetComponent<PlayerNetwork>().CallCmdClearItemsOfPlayer();
        SerializeItems();
        foreach (SerialItem sitem in serializedItems)
        {
            inventory.GetPlayerRpg().gameObject.GetComponent<PlayerNetwork>().SerializeItem(sitem);
        }
        inventory.GetPlayerRpg().gameObject.GetComponent<PlayerNetwork>().CallCmdSerializeActionBar(GlobalSerialization.SerializeActionBar(abs));
        inventory.GetPlayerRpg().gameObject.GetComponent<PlayerNetwork>().CallCmdSerializePlayerPosition();
        inventory.GetPlayerRpg().gameObject.GetComponent<PlayerNetwork>().CallCmdSerializePlayerRpg();
    }

    //this is called on client, assigned to button for tests
    public void InitLoadOnButton()
    {
        inventory.GetPlayerRpg().gameObject.GetComponent<PlayerNetwork>().CallCmdInitiateLoadFromClient();
        //inventory.GetPlayerRpg().gameObject.GetComponent<PlayerUI>().UpdatePlayerStats();
        inventory.GetPlayerRpg().gameObject.GetComponent<PlayerUI>().loaded = true;
        loadedIn = true;
    }
    
    //this is called on server
    public void SetupItemsFromDatabase(string playerName)
    {
        serializedItems = databaseControl.LoadItemsOfPlayer(playerName);
        GameObject playerGo = null;
        if (GameObject.Find("Player(Clone)") == null)
        {
            playerGo = inventory.GetPlayerRpg().gameObject;
        }
        else
        {
            playerGo = GameObject.Find("Player(Clone)");
        }
        foreach (SerialItem sitem in serializedItems)
        {
            playerGo.GetComponent<PlayerNetwork>().CallRpcLoadItemFromDatabase(sitem, playerName);
        }
        playerGo.GetComponent<PlayerNetwork>().CallRpcLoadActionBarFromDatabase(databaseControl.LoadActionBarOfPlayer(playerName), playerName);
        playerGo.GetComponent<PlayerNetwork>().CallRpcLoadPositionFromDatabase(databaseControl.LoadPositionOfPlayer(playerName), playerName);
        playerGo.GetComponent<PlayerNetwork>().CallRpcLoadPlayerRpgFromDatabase(databaseControl.LoadPlayerRpg(playerName), playerName);
    }
}
