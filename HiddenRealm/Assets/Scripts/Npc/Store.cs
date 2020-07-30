using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : NpcWindowController
{
    [SerializeField]
    private Inventory inventory = null;
    [SerializeField]
    private UIHandler myUI = null;
    [SerializeField]
    private Image buyButtonImg = null;
    [SerializeField]
    private Image sellButtonImg = null;
    [SerializeField]
    private Transform npcTransform = null;

    private PlayerRpg playerRpg;

    private bool buyMode = false;

    private Color32 defaultButtonColor;

    // Start is called before the first frame update
    void Start()
    {
        playerRpg = inventory.GetPlayerRpg();
        defaultButtonColor = new Color32(36, 36, 36, 255);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRpg != null)
        {
            if (Vector2.Distance(npcTransform.position, playerRpg.transform.position) > 10f)
            {
                if (gameObject.activeSelf)
                {
                    CloseStoreOnClick();
                }
            }
        }
    }

    public void SwitchBuyMode()
    {
        if (buyMode)
        {
            buyMode = false;
            buyButtonImg.color = defaultButtonColor;
        }
        else
        {
            if (inventory.SellMode)
            {
                SwitchSellMode();
            }
            buyMode = true;
            buyButtonImg.color = Color.black;
        }
    }

    public void SwitchSellMode()
    {
        if (inventory.SellMode)
        {
            inventory.SellMode = false;
            sellButtonImg.color = defaultButtonColor;
        }
        else
        {
            if (buyMode)
            {
                SwitchBuyMode();
            }
            inventory.SellMode = true;
            sellButtonImg.color = Color.black;
        }
    }

    public void BuyItem(int price, Item item)
    {
        if (buyMode)
        {
            bool enoughMoney = playerRpg.PayYang(price);
            if (enoughMoney)
            {
                inventory.PickupItem(item);
            }
            else
            {
                myUI.ShowInfoBox("Not enough yang.");
            }
        }
    }

    public void OpenStoreOnClick()
    {
        if(!gameObject.activeSelf)
        {
            buyMode = false;
            inventory.SellMode = false;
            gameObject.SetActive(true);
            myUI.SetActiveNpcWindow(this);
        }
    }

    public void CloseStoreOnClick()
    {
        buyMode = false;
        inventory.SellMode = false;
        buyButtonImg.color = defaultButtonColor;
        sellButtonImg.color = defaultButtonColor;
        gameObject.SetActive(false);
        myUI.ClearActiveNpcWindow();
    }

    public override void CloseWindow()
    {
        CloseStoreOnClick();
    }
}
