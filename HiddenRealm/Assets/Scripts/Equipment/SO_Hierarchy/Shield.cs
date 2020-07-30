using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shield", menuName = "Item/Equipable/Shield")]
public class Shield : EquippableItem
{
    public int[] defenseValues;
    public int[] yangUpgradeCost;

    public int GetDefense(int upgradeLvl)
    {
        return defenseValues[upgradeLvl];
    }
}
