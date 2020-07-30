using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    public Inventory inventory;

    public int itemX;
    public int itemY;

    public EqPieceSlot eqPieceSlot = null;
    public ActionBarSlot actionBarSlot = null;
    private int quantity = 0;
    public bool onActionBar = false;

    public TextMeshProUGUI stackText;
    private ItemUI pointerItemUI = null;
    private List<ItemUI> itemObservers = new List<ItemUI>();

    public int UpgradeLevel { get; set; } = 0;

    private string externalDescription = "";

    private void Update()
    {
        UpdateQuantityText();
    }

    public void Upgrade()
    {
        UpgradeLevel++;
    }

    public void MousePickMe()
    {
        inventory.MousePickItem(this);
    }

    public void ChangeQuantity(int value)
    {
        quantity = value;
        //UpdateQuantityText();
    }

    public int GetQuantity()
    {
        return quantity;
    }

    public void UpdateQuantityText()
    {
        if(!item.equipable)
        {
            if (pointerItemUI == null)
            {
                if(quantity > 0)
                {
                    stackText.text = quantity.ToString();
                }
                else
                {
                    stackText.text = "";
                }
            }
            else
            {
                stackText.text = pointerItemUI.GetQuantity().ToString();
            }
        } 
    }

    public void SetPointer(ItemUI item)
    {
        pointerItemUI = item;
        item.itemObservers.Add(this);
    }

    public ItemUI GetPointer()
    {
        return pointerItemUI;
    }

    public void DestroyObservers()
    {
        foreach(ItemUI i in itemObservers)
        {
            Destroy(i.gameObject);
        }
        itemObservers = new List<ItemUI>();
    }

    public void MoveObserversTo(ItemUI other)
    {
        foreach(ItemUI i in itemObservers)
        {
            i.SetPointer(other);
            if (i.actionBarSlot != null)
            {
                i.actionBarSlot.originalItem = other;
            }
        }
    }

    private void OnDestroy()
    {
        if(pointerItemUI != null)
        {
            pointerItemUI.itemObservers.Remove(this);
        }
        TooltipHandler.instance.HideTooltip();
    }

    public void AddExternalDescription(string str)
    {
        externalDescription = str;
    }

    public string GetStringFullname()
    {
        string result = item.itemName;
        if (item.equipable)
        {
            result = result + "+" + UpgradeLevel;
        }
        if(quantity > 1)
        {
            result += "(" + quantity + ")";
        }
        return result;
    }

    public string GetDescription()
    {
        string result = item.itemName;
        if(item.equipable)
        {
            string upgradeLevel = UpgradeLevel.ToString();
            result = result + "+" + upgradeLevel;
            result += "\nLevel Required: " + ((EquippableItem)item).levelRequirement;
            if(item is Armor || item is Shield || item is Helmet)
            {
                if (item is Armor)
                    result += "\nDefense " + ((Armor)item).GetDefense(UpgradeLevel);
                if (item is Shield)
                    result += "\nDefense " + ((Shield)item).GetDefense(UpgradeLevel);
                if (item is Helmet)
                    result += "\nDefense " + ((Helmet)item).GetDefense(UpgradeLevel);
            }
            if(item is Weapon)
            {
                result += "\nAttack Values: " + ((Weapon)item).GetAttackLower(UpgradeLevel) + "-" + ((Weapon)item).GetAttackHigher(UpgradeLevel);
            }
        }
        result += "\nSell Price: " + item.sellPrice;
        result += externalDescription;
        return result;
    }

    public string GetUpgradeDescription()
    {
        string result = item.itemName;
        if (item.equipable)
        {
            int upgradeLvl = UpgradeLevel + 1;
            result = result + "+" + upgradeLvl;
            result += "\nLevel Required: " + ((EquippableItem)item).levelRequirement;
            if (item is Armor || item is Shield || item is Helmet)
            {
                if (item is Armor)
                    result += "\nDefense " + ((Armor)item).GetDefense(upgradeLvl);
                if (item is Shield)
                    result += "\nDefense " + ((Shield)item).GetDefense(upgradeLvl);
                if (item is Helmet)
                    result += "\nDefense " + ((Helmet)item).GetDefense(upgradeLvl);
            }
            if (item is Weapon)
            {
                result += "\nAttack Values: " + ((Weapon)item).GetAttackLower(upgradeLvl) + "-" + ((Weapon)item).GetAttackHigher(upgradeLvl);
            }
        }
        return result;
    }

    public int GetUpgradeCost()
    {
        int cost = -1;
        int upgradeLvl = UpgradeLevel + 1;
        if (item is Armor || item is Shield || item is Helmet)
        {
            if (item is Armor)
                cost = ((Armor)item).yangUpgradeCost[upgradeLvl];
            if (item is Shield)
                cost = ((Shield)item).yangUpgradeCost[upgradeLvl];
            if (item is Helmet)
                cost = ((Helmet)item).yangUpgradeCost[upgradeLvl];
        }
        if (item is Weapon)
        {
            cost = ((Weapon)item).yangUpgradeCost[upgradeLvl];
        }
        return cost;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("pointer entered item");
        TooltipHandler.instance.Move(transform.position);
        TooltipHandler.instance.SetText(GetDescription());
        TooltipHandler.instance.ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("pointer exited item");
        TooltipHandler.instance.HideTooltip();
    }
}
