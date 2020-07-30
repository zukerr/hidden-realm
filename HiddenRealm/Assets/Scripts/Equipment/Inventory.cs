using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject gridGO = null;
    [SerializeField]
    private GameObject itemsGO = null;
    [SerializeField]
    private GameObject oneSlotItemPrefab = null;
    [SerializeField]
    private GameObject twoSlotItemPrefab = null;
    [SerializeField]
    private GameObject threeSlotItemPrefab = null;
    [SerializeField]
    private UIHandler myUI = null;

    private int[,] grid;
    private int[,] rootItems;
    private GameObject[,] slots;
    private int[,] slotIds;
    private Item[,] items;

    private const int height = 9;
    private const int width = 5;

    private ItemUI airItem;

    public bool SellMode { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        grid = new int[width, height];
        slots = new GameObject[width, height];
        items = new Item[width, height];
        rootItems = new int[width, height];
        slotIds = new int[width, height];
        SetupSlots();
    }

    private void SetupSlots()
    {
        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                slots[j, (height - i - 1)] = gridGO.transform.GetChild((width * i) + j).gameObject;
                gridGO.transform.GetChild((width * i) + j).gameObject.GetComponent<InventorySlotUI>().inventory = this;
                gridGO.transform.GetChild((width * i) + j).gameObject.GetComponent<InventorySlotUI>().slotX = j;
                gridGO.transform.GetChild((width * i) + j).gameObject.GetComponent<InventorySlotUI>().slotY = (height - i - 1);
                slotIds[j, (height - i - 1)] = (width * i) + j;
                grid[j, (height - i - 1)] = -1;
                rootItems[j, (height - i - 1)] = -1;
                items[j, (height - i - 1)] = null;
            }
        }
    }

    public int SlotSize(Item item)
    {
        bool oneSlot = (!item.equipable) || ((item is EquippableItem) && (((EquippableItem)item).inventorySlots == 1));
        bool twoSlot = (item.equipable) && (((EquippableItem)item).inventorySlots == 2);
        bool threeSlot = (item.equipable) && (((EquippableItem)item).inventorySlots == 3);

        if(oneSlot)
        {
            return 1;
        }
        else if(twoSlot)
        {
            return 2;
        }
        else if(threeSlot)
        {
            return 3;
        }
        else
        {
            return -1;
        }
    }

    public GameObject GetApropriatePrefab(Item item)
    {
        int size = SlotSize(item);
        if(size == 1)
        {
            return oneSlotItemPrefab;
        }
        else if(size == 2)
        {
            return twoSlotItemPrefab;
        }
        else if(size == 3)
        {
            return threeSlotItemPrefab;
        }
        else
        {
            return null;
        }
    }

    private GameObject SetupNewUiItem(GameObject prefab, int i, int j, Item item, int upgradeLvl, int quantity)
    {
        GameObject newGo = Instantiate(prefab, slots[i, j].transform.position, Quaternion.identity, itemsGO.transform);
        newGo.GetComponent<ItemUI>().inventory = this;
        newGo.GetComponent<ItemUI>().item = item;
        newGo.GetComponent<ItemUI>().itemX = i;
        newGo.GetComponent<ItemUI>().itemY = j;
        if(upgradeLvl != -1)
        {
            newGo.GetComponent<ItemUI>().UpgradeLevel = upgradeLvl;
        }
        if (item is UsableItem)
        {
            newGo.GetComponent<ItemUI>().ChangeQuantity(((UsableItem)item).quantity);
        }
        if (quantity != -1)
        {
            newGo.GetComponent<ItemUI>().ChangeQuantity(quantity);
        }

        newGo.GetComponent<Image>().sprite = item.inventoryIcon;

        return newGo;
    }

    private GameObject OneSlotGuts(Item item, int i, int j, int upgradeLvl = -1, int quantity = -1)
    {
        items[i, j] = item;
        grid[i, j] = item.id;
        rootItems[i, j] = slotIds[i, j];
        return SetupNewUiItem(oneSlotItemPrefab, i, j, item, upgradeLvl, quantity);
    }

    private GameObject TwoSlotGuts(Item item, int i, int j, int upgradeLvl = -1, int quantity = -1)
    {
        items[i, j] = item;
        grid[i, j] = item.id;
        rootItems[i, j] = slotIds[i, j];
        

        items[i, j - 1] = item;
        grid[i, j - 1] = item.id;
        rootItems[i, j - 1] = slotIds[i, j];

        return SetupNewUiItem(twoSlotItemPrefab, i, j, item, upgradeLvl, quantity);
    }

    private GameObject ThreeSlotGuts(Item item, int i, int j, int upgradeLvl = -1, int quantity = -1)
    {
        items[i, j] = item;
        grid[i, j] = item.id;
        rootItems[i, j] = slotIds[i, j];
        
        items[i, j - 1] = item;
        grid[i, j - 1] = item.id;
        rootItems[i, j - 1] = slotIds[i, j];

        items[i, j - 2] = item;
        grid[i, j - 2] = item.id;
        rootItems[i, j - 2] = slotIds[i, j];

        return SetupNewUiItem(threeSlotItemPrefab, i, j, item, upgradeLvl, quantity);
    }

    private void Clear1Slot(int i, int j)
    {
        items[i, j] = null;
        grid[i, j] = -1;
        rootItems[i, j] = -1;
    }

    private void Clear2Slot(int i, int j)
    {
        items[i, j] = null;
        grid[i, j] = -1;
        rootItems[i, j] = -1;

        items[i, j - 1] = null;
        grid[i, j - 1] = -1;
        rootItems[i, j - 1] = -1;
    }

    private void Clear3Slot(int i, int j)
    {
        items[i, j] = null;
        grid[i, j] = -1;
        rootItems[i, j] = -1;

        items[i, j - 1] = null;
        grid[i, j - 1] = -1;
        rootItems[i, j - 1] = -1;

        items[i, j - 2] = null;
        grid[i, j - 2] = -1;
        rootItems[i, j - 2] = -1;
    }

    public GameObject PutItemOnCoords(int i, int j, Item item, int slotSize, int upgradeLvl)
    {
        if (slotSize == 1)
        {
            if (grid[i, j] == -1)
            {
                return OneSlotGuts(item, i, j, upgradeLvl);
            }
        }
        else if (slotSize == 2)
        {
            if ((grid[i, j] == -1) && (j > 0))
            {
                if (grid[i, j - 1] == -1)
                {
                    return TwoSlotGuts(item, i, j, upgradeLvl);
                }
            }
        }
        else if (slotSize == 3)
        {
            if ((grid[i, j] == -1) && (j > 1))
            {
                if (grid[i, j - 1] == -1)
                {
                    if (grid[i, j - 2] == -1)
                    {
                        return ThreeSlotGuts(item, i, j, upgradeLvl);
                    }
                }
            }
        }
        return null;
    }

    public bool ClearSlot(int i, int j, int slotSize)
    {
        if (slotSize == 1)
        {
            if (grid[i, j] != -1)
            {
                Clear1Slot(i, j);
                return true;
            }
        }
        else if (slotSize == 2)
        {
            if ((grid[i, j] != -1) && (j > 0))
            {
                if (grid[i, j - 1] != -1)
                {
                    Clear2Slot(i, j);
                    return true;
                }
            }
        }
        else if (slotSize == 3)
        {
            if ((grid[i, j] != -1) && (j > 1))
            {
                if (grid[i, j - 1] != -1)
                {
                    if (grid[i, j - 2] != -1)
                    {
                        Clear3Slot(i, j);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool PickupItem(Item item, int upgradeLvl = -1, int quantity = -1)
    {
        int slotSize = SlotSize(item);
        for (int j = (height - 1); j >= 0; j--)
        {
            for(int i = 0; i < width; i++)
            {
                if(slotSize == 1)
                {
                    if(grid[i, j] == -1)
                    {
                        OneSlotGuts(item, i, j, upgradeLvl, quantity);
                        return true;
                    }
                }
                else if (slotSize == 2)
                {
                    if ((grid[i, j] == -1) && (j > 0))
                    {
                        if(grid[i, j - 1] == -1)
                        {
                            TwoSlotGuts(item, i, j, upgradeLvl, quantity);
                            return true;
                        }
                    }
                }
                else if (slotSize == 3)
                {
                    if ((grid[i, j] == -1) && (j > 1))
                    {
                        if (grid[i, j - 1] == -1)
                        {
                            if (grid[i, j - 2] == -1)
                            {
                                ThreeSlotGuts(item, i, j, upgradeLvl, quantity);
                                return true;
                            }
                        }
                    }
                }
            }
        }
        Debug.Log("Inventory is full!");
        return false;
    }

    public void MousePickItem(ItemUI item)
    {
        airItem = item;
        if (SellMode)
        {
            SellItem();
        }
    }

    public void MousePutDown(InventorySlotUI slot)
    {
        if(airItem == null)
        {
            return;
        }
        if(airItem.onActionBar)
        {
            return;
        }
        int slotSize = SlotSize(airItem.item);
        GameObject result = PutItemOnCoords(slot.slotX, slot.slotY, airItem.item, slotSize, airItem.UpgradeLevel);
        result.GetComponent<ItemUI>().ChangeQuantity(airItem.GetQuantity());
        if (result != null)
        {
            //if it was previously in inventory, not eq
            if((airItem.itemX != -1) && (airItem.itemY != -1))
            {
                ClearSlot(airItem.itemX, airItem.itemY, slotSize);
            }
            else
            {
                airItem.eqPieceSlot.item = null;
                airItem.eqPieceSlot.itemUI = null;
                airItem.eqPieceSlot = null;
                Debug.Log("nulled items belonging to eq");
            }
            airItem.MoveObserversTo(result.GetComponent<ItemUI>());
            Destroy(airItem.gameObject);
            myUI.playerUI.UpdatePlayerStats();
        }
        airItem = null;
    }

    public void MousePutDownInEq(EqSlot slot)
    {
        if (airItem == null)
        {
            return;
        }
        if(!(airItem.item is EquippableItem))
        {
            airItem = null;
            return;
        }
        if(slot.parentEqPieceSlot.piece != ((EquippableItem)airItem.item).eqPiece)
        {
            airItem = null;
            return;
        }
        if(((EquippableItem)airItem.item).levelRequirement > myUI.playerUI.gameObject.GetComponent<PlayerRpg>().Level)
        {
            //level too low
            airItem = null;
            return;
        }
        if (slot.parentEqPieceSlot.item == null)
        {
            GenerateItemInEq(airItem.item, airItem.UpgradeLevel);

            int slotSize = SlotSize(airItem.item);
            ClearSlot(airItem.itemX, airItem.itemY, slotSize);
            Destroy(airItem.gameObject);
            myUI.playerUI.UpdatePlayerStats();
        }
        airItem = null;
    }

    //to be used in serialization process
    public void GenerateItemInEq(Item item, int upgradeLvl)
    {
        EqPieceSlot slot = myUI.GetEqPieceSlotByEqPiece(((EquippableItem)item).eqPiece);
        GameObject prefab = GetApropriatePrefab(item);
        GameObject newGo = Instantiate(prefab, slot.slot1.transform.position, Quaternion.identity, itemsGO.transform);
        newGo.GetComponent<ItemUI>().inventory = this;
        newGo.GetComponent<ItemUI>().item = item;
        newGo.GetComponent<ItemUI>().UpgradeLevel = upgradeLvl;
        newGo.GetComponent<ItemUI>().itemX = -1;
        newGo.GetComponent<ItemUI>().itemY = -1;
        newGo.GetComponent<ItemUI>().eqPieceSlot = slot;
        newGo.GetComponent<Image>().sprite = item.inventoryIcon;
        slot.item = item;
        slot.itemUI = newGo.GetComponent<ItemUI>();
    }

    public ItemUI FindItem(int x, int y)
    {
        for(int i = 0; i < itemsGO.transform.childCount; i++)
        {
            ItemUI _item = itemsGO.transform.GetChild(i).GetComponent<ItemUI>();
            if ((_item.itemX == x) && (_item.itemY == y))
            {
                return _item;
            }
        }
        return null;
    }

    public void MousePutDownOnActionBar(ActionBarSlot slot)
    {
        if(airItem == null)
        {
            return;
        }
        if(!(airItem.item is UsableItem))
        {
            airItem = null;
            return;
        }
        GameObject newGo = Instantiate(airItem.gameObject, slot.transform.position, Quaternion.identity, slot.transform.GetChild(0));
        slot.copiedItem = newGo;
        slot.originalItem = airItem;
        slot.usableItem = airItem.item;
        newGo.GetComponent<ItemUI>().onActionBar = true;
        newGo.GetComponent<ItemUI>().actionBarSlot = slot;
        newGo.GetComponent<ItemUI>().SetPointer(airItem);
        newGo.GetComponent<Image>().sprite = airItem.item.inventoryIcon;
        if (airItem.onActionBar)
        {
            slot.originalItem = airItem.transform.parent.parent.GetComponent<ActionBarSlot>().originalItem;
            newGo.GetComponent<ItemUI>().SetPointer(airItem.GetPointer());
            newGo.GetComponent<Image>().sprite = airItem.GetPointer().item.inventoryIcon;
            Destroy(airItem.gameObject);
        }
        airItem = null;
    }

    public void MousePutDownOnBlacksmith(Blacksmith blacksmith)
    {
        if (airItem == null)
        {
            myUI.ClearActiveNpcWindow();
            return;
        }
        if (!(airItem.item is EquippableItem))
        {
            airItem = null;
            myUI.ClearActiveNpcWindow();
            return;
        }
        if (airItem.UpgradeLevel == 9)
        {
            airItem = null;
            myUI.ClearActiveNpcWindow();
            return;
        }
        if(airItem.eqPieceSlot != null)
        {
            airItem = null;
            myUI.ClearActiveNpcWindow();
            return;
        }
        if(airItem.onActionBar)
        {
            airItem = null;
            myUI.ClearActiveNpcWindow();
            return;
        }
        GameObject prefab = GetApropriatePrefab(airItem.item);
        GameObject newGo = Instantiate(prefab, blacksmith.itemScreenLocation.position, Quaternion.identity, blacksmith.itemScreenLocation);
        newGo.GetComponent<Button>().enabled = false;
        newGo.GetComponent<ItemUI>().item = airItem.item;
        newGo.GetComponent<ItemUI>().UpgradeLevel = airItem.UpgradeLevel;
        newGo.GetComponent<ItemUI>().itemX = -1;
        newGo.GetComponent<ItemUI>().itemY = -1;
        newGo.GetComponent<Image>().sprite = airItem.item.inventoryIcon;
        blacksmith.SetupUpgradeWindow(newGo.GetComponent<ItemUI>(), airItem, myUI.playerUI.gameObject.GetComponent<PlayerRpg>());
        airItem = null;
    }

    public void MousePutDownOnThrowArea()
    {
        if (airItem == null)
        {
            return;
        }
        if (airItem.onActionBar)
        {
            return;
        }
        myUI.playerUI.gameObject.GetComponent<PlayerThrowItem>().ThrowItem(airItem.gameObject);
        airItem = null;
    }

    public void MouseClickThrowArea()
    {
        if (airItem == null)
        {
            return;
        }
        if (airItem.onActionBar)
        {
            return;
        }
        myUI.SetActiveThrowItemConfirmationBox();
    }

    public PlayerRpg GetPlayerRpg()
    {
        return myUI.playerUI.gameObject.GetComponent<PlayerRpg>();
    }

    public void SellItem()
    {
        if(airItem == null)
        {
            return;
        }
        int price = airItem.item.sellPrice;
        if(airItem.GetQuantity() > 0)
        {
            price = airItem.item.sellPrice * airItem.GetQuantity();
        }
        GetPlayerRpg().GrantYang(price);

        int slotSize = SlotSize(airItem.item);
        ClearSlot(airItem.itemX, airItem.itemY, slotSize);
        Destroy(airItem.gameObject);
        myUI.playerUI.UpdatePlayerStats();
    }

    public void ClearInventorySlot(ItemUI _item)
    {
        int slotSize = SlotSize(_item.item);
        ClearSlot(_item.itemX, _item.itemY, slotSize);
    }
}
