using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Helmet", menuName = "Item/Equipable/Helmet")]
public class Helmet : EquippableItem
{
    public int[] defenseValues;
    public int[] yangUpgradeCost;

    public int GetDefense(int upgradeLvl)
    {
        return defenseValues[upgradeLvl];
    }
}
