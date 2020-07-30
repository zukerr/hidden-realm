using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    [SerializeField]
    private Item item = null;
    [SerializeField]
    private int price = 0;

    //uncomment and use later if you want to buy items from store with non 0 upgrade level
    //[SerializeField]
    //private int upgradeLevel = 0;

    [SerializeField]
    private Image itemImg = null;

    [SerializeField]
    private Store parentStore = null;

    private void Start()
    {
        itemImg.sprite = item.inventoryIcon;
        GetComponent<ItemUI>().AddExternalDescription("\nBuy Price: " + price);
        if (item is UsableItem)
        {
            GetComponent<ItemUI>().ChangeQuantity(((UsableItem)item).quantity);
            GetComponent<ItemUI>().UpdateQuantityText();
        }
    }

    public void BuyItemOnClick()
    {
        parentStore.BuyItem(price, item);
    }
}
