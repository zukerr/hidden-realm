using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotUI : MonoBehaviour
{
    public Inventory inventory;

    public int slotX;
    public int slotY;

    public void MousePutItemHere()
    {
        inventory.MousePutDown(this);
    }
}
