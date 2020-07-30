using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqSlot : MonoBehaviour
{
    public EqPieceSlot parentEqPieceSlot;

    private void Start()
    {
        parentEqPieceSlot = transform.parent.GetComponent<EqPieceSlot>();
    }

    public Transform GetRootSlot()
    {
        return parentEqPieceSlot.slot1.transform;
    }

    public void MousePutItemHere()
    {
        parentEqPieceSlot.inventory.MousePutDownInEq(this);
    }
}
