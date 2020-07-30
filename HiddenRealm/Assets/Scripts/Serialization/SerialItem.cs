using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialItem
{
    public int itemID;
    public int x;
    public int y;
    public int upgradeLvl;
    public int quantity;

    public SerialItem(int itemID, int x, int y, int upgradeLvl, int quantity)
    {
        this.itemID = itemID;
        this.x = x;
        this.y = y;
        this.upgradeLvl = upgradeLvl;
        this.quantity = quantity;
    }
}
