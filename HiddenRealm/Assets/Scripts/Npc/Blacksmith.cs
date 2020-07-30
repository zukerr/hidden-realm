using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Blacksmith : NpcWindowController
{
    private ItemUI item;
    private ItemUI ogItem;
    private PlayerRpg playerRpg;
    public Transform itemScreenLocation;
    public Inventory inventory;
    public GameObject UpgradeWindow;
    public TextMeshProUGUI UpgradeText;
    public TextMeshProUGUI CostText;

    [SerializeField]
    private UIHandler myUI = null;

    // Update is called once per frame
    void Update()
    {
        if(playerRpg != null)
        {
            if (Vector2.Distance(transform.position, playerRpg.transform.position) > 10f)
            {
                if (UpgradeWindow.activeSelf)
                {
                    CancelUpgrade();
                }
            }
        }
    }

    public void SetItem(ItemUI item)
    {
        this.item = item;
    }

    public void EnableUpgradeWindow()
    {
        myUI.SetActiveNpcWindow(this);
        inventory.MousePutDownOnBlacksmith(this);
    }

    public void SetDescriptionText()
    {
        UpgradeText.text = item.GetUpgradeDescription();
    }

    public void SetCostText()
    {
        CostText.text = "Price: " + item.GetUpgradeCost() + " Yang";
    }

    public void SetupUpgradeWindow(ItemUI _item, ItemUI _ogItem, PlayerRpg _playerRpg)
    {
        SetItem(_item);
        ogItem = _ogItem;
        playerRpg = _playerRpg;
        UpgradeWindow.SetActive(true);
        SetDescriptionText();
        SetCostText();
    }

    public void DoUpgrade()
    {
        bool enoughMoney = playerRpg.PayYang(item.GetUpgradeCost());
        if(enoughMoney)
        {
            //roll the chance of upgrade
            bool roll = GetUpgradeChance(ogItem.UpgradeLevel + 1);

            if(roll)
            {
                //this is what happens if upgrade successful
                Destroy(item.gameObject);
                ogItem.Upgrade();
                if (ogItem.UpgradeLevel == 9)
                {
                    CancelUpgrade();
                }
                else
                {
                    inventory.MousePickItem(ogItem);
                    inventory.MousePutDownOnBlacksmith(this);
                }
                //display upgrade complete
                myUI.ShowInfoBox("Upgrade successful.");
            }
            else
            {
                Destroy(item.gameObject);
                inventory.ClearInventorySlot(ogItem);
                Destroy(ogItem.gameObject);
                CancelUpgrade();
                myUI.ShowInfoBox("You failed.");
            }
        }
        else
        {
            //display not enough yang window
            myUI.ShowInfoBox("Not enough yang.");
        }
    }

    private bool GetUpgradeChance(int targetUpgradeLevel)
    {
        float chance = 0f;
        switch(targetUpgradeLevel)
        {
            case 1:
                chance = 0.9f;
                break;
            case 2:
                chance = 0.9f;
                break;
            case 3:
                chance = 0.9f;
                break;
            case 4:
                chance = 0.8f;
                break;
            case 5:
                chance = 0.7f;
                break;
            case 6:
                chance = 0.6f;
                break;
            case 7:
                chance = 0.5f;
                break;
            case 8:
                chance = 0.4f;
                break;
            case 9:
                chance = 0.3f;
                break;
        }
        float rng = Random.Range(0f, 1f);
        if(rng <= chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CancelUpgrade()
    {
        UpgradeWindow.SetActive(false);
        if(item != null)
        {
            Destroy(item.gameObject);
        }
        myUI.ClearActiveNpcWindow();
    }

    public override void CloseWindow()
    {
        CancelUpgrade();
    }
}
