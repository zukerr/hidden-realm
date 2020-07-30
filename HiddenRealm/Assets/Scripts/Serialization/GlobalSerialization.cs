using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GlobalSerialization
{
    public static readonly int spTabl1 = 99;
    public static readonly int spTabl2 = 3;

    public static string SerializeSpAquisitionTable(bool[,] tab)
    {
        int counter = 0;
        bool train = false;
        for(int i = 0; i < tab.GetLength(0); i++)
        {
            for(int j = 0; j < tab.GetLength(1); j++)
            {
                if(!train)
                {
                    if (tab[i, j])
                    {
                        counter++;
                    }
                    else
                    {
                        train = true;
                    }
                }
            }
        }
        int l1 = tab.GetLength(0);
        int l2 = tab.GetLength(1);
        return counter.ToString();
    }

    public static bool[,] DeserializeSpAquisitionTable(string serialTab)
    {
        int counter = (int)Int64.Parse(serialTab);
        int l1 = spTabl1;
        int l2 = spTabl2;

        bool[,] result = new bool[l1, l2];

        for (int i = 0; i < l1; i++)
        {
            for (int j = 0; j < l2; j++)
            {
                if (counter > 0)
                {
                    result[i, j] = true;
                    counter--;
                }
                else
                {
                    result[i, j] = false;
                }
            }
        }

        return result;
    }

    public static SerialItem SerializeItem(ItemUI item)
    {
        int id = ItemsDatabase.instance.GetIndexOfItem(item.item);
        int x = item.itemX;
        int y = item.itemY;
        int upgradeLvl = item.UpgradeLevel;
        int quantity = item.GetQuantity();

        return new SerialItem(id, x, y, upgradeLvl, quantity);
    }

    public static void DeserializeItem(SerialItem serialItem, Inventory inventory)
    {
        Item item = ItemsDatabase.instance.itemList[serialItem.itemID];
        int slotSize = inventory.SlotSize(item);

        if((serialItem.x == -1) && (serialItem.y == -1))
        {
            inventory.GenerateItemInEq(item, serialItem.upgradeLvl);
            inventory.GetPlayerRpg().gameObject.GetComponent<PlayerUI>().UpdatePlayerStats();
        }
        else
        {
            //the original item was in the inventory, not eq
            GameObject result = inventory.PutItemOnCoords(serialItem.x, serialItem.y, item, slotSize, serialItem.upgradeLvl);
            result.GetComponent<ItemUI>().ChangeQuantity(serialItem.quantity);
        }
    }

    public static string SerializeActionBar(ActionBarSerialization abs)
    {
        string result = "";

        for(int i = 0; i < abs.grid.Count; i++)
        {
            if(abs.grid[i].transform.GetChild(0).childCount > 0)
            {
                //if we are here, it means that this slot has assigned item
                result += "1";
                result += abs.grid[i].GetComponent<ActionBarSlot>().originalItem.itemX;
                result += abs.grid[i].GetComponent<ActionBarSlot>().originalItem.itemY;
            }
            else
            {
                result += "000";
            }
        }

        return result;
    }

    public static void DeserializeActionBar(string actionBar, Inventory inventory, ActionBarSerialization abs)
    {
        bool noItem = false;
        int currentX = -5;
        int currentY = -5;
        int currentActionBarSlot = -1;

        for(int i = 0; i < actionBar.Length; i++)
        {
            if(i % 3 == 0)
            {
                currentActionBarSlot++;
                if(actionBar[i] == '0')
                {
                    noItem = true;
                }
                else
                {
                    noItem = false;
                }
            }
            else if (i % 3 == 1)
            {
                if(!noItem)
                {
                    currentX = (int)Char.GetNumericValue(actionBar[i]);
                }
            }
            else if (i % 3 == 2)
            {
                if(!noItem)
                {
                    currentY = (int)Char.GetNumericValue(actionBar[i]);
                    inventory.MousePickItem(inventory.FindItem(currentX, currentY));
                    inventory.MousePutDownOnActionBar(abs.grid[currentActionBarSlot].GetComponent<ActionBarSlot>());
                }
            }
        }
    }

    public static SerialPosition SerializePosition(GameObject player)
    {
        float x = player.transform.position.x;
        float y = player.transform.position.y;
        float z = player.transform.rotation.z;

        return new SerialPosition(x, y, z);
    }

    public static void DeserializePosition(SerialPosition sPos, GameObject player)
    {
        player.transform.position = new Vector3(sPos.posX, sPos.posY, player.transform.position.z);
        player.transform.rotation = new Quaternion(player.transform.rotation.x, player.transform.rotation.y, sPos.rotZ, player.transform.rotation.w);
    }

    public static SerialPlayerRpg SerializePlayerRpg(PlayerRpg player)
    {
        string spointstab = SerializeSpAquisitionTable(player.spAcquisitionTable);
        return new SerialPlayerRpg(player.playerName, player.Yang, player.Level, player.Exp, player.StatusPoints, spointstab,
                                    player.currentHp, player.Energy, player.Stamina,
                                    player.BaseVit, player.BaseInt, player.BaseStr, player.BaseDex);
    }

    public static void DeserializePlayerRpg(SerialPlayerRpg serialPlayer, PlayerRpg player)
    {
        player.SetupSerializedValues(serialPlayer);
    }
}
