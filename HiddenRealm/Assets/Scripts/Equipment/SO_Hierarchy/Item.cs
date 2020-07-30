using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string itemName;
    public int id;
    public Sprite inventoryIcon;
    public bool equipable;
    public int sellPrice;

    public bool Equals(Item other)
    {
        if(itemName.Equals(other.itemName))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string GetLabel()
    {
        string result = itemName;
        if(equipable)
        {
            result += "+0";
        }
        return result;
    }
}
