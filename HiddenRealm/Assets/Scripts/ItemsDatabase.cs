using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsDatabase : MonoBehaviour
{
    public static ItemsDatabase instance = null;

    public List<Item> itemList;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public int GetIndexOfItem(Item item)
    {
        foreach(Item i in itemList)
        {
            if(i != null)
            {
                if (i.Equals(item))
                {
                    Debug.Log("Found index of " + item.itemName + ": " + itemList.IndexOf(i));
                    return itemList.IndexOf(i);
                }
            }
        }
        return -1;
    }
}
