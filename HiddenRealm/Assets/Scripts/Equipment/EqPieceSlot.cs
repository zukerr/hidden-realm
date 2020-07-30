using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqPieceSlot : MonoBehaviour
{
    public Inventory inventory;

    public EqPiece piece;
    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;

    //set externally
    public Item item = null;
    public ItemUI itemUI = null;
}
