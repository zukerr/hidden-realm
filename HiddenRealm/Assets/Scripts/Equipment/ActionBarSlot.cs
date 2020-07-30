using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBarSlot : MonoBehaviour
{
    public Inventory inventory;
    public ItemUI originalItem = null;
    public GameObject copiedItem = null;
    public KeyCode assignedKey;

    public Item usableItem = null;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(assignedKey))
        {
            UseActionButton();
        }
    }

    public void UseActionButton()
    {
        if(usableItem != null)
        {
            if(usableItem is HealthPotion)
            {
                GameObject.Find("LocalPlayer").GetComponent<PlayerRpg>().UseHealthPotion(((HealthPotion)usableItem).healthAmount);
                originalItem.ChangeQuantity(originalItem.GetQuantity() - 1);
                if (originalItem.GetQuantity() == 0)
                {
                    originalItem.DestroyObservers();
                    inventory.ClearSlot(originalItem.itemX, originalItem.itemY, 1);
                    Destroy(originalItem.gameObject);
                    //Destroy(copiedItem);
                }
            }
        }
    }

    public void PutItemHere()
    {
        inventory.MousePutDownOnActionBar(this);
    }
}
