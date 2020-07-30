using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Item/Equipable/Armor")]
public class Armor : EquippableItem
{
    public int[] defenseValues;
    public int[] yangUpgradeCost;
    public int sockets;

    public int GetDefense(int upgradeLvl)
    {
        return defenseValues[upgradeLvl];
    }
}
