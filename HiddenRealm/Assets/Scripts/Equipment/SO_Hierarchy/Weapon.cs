using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Item/Equipable/Weapon")]
public class Weapon : EquippableItem
{
    public bool twoHanded;
    public int[] attackValuesLower;
    public int[] attackValuesHigher;
    public int[] yangUpgradeCost;
    public int sockets;

    public int GetAttackLower(int upgradeLvl)
    {
        return attackValuesLower[upgradeLvl];
    }

    public int GetAttackHigher(int upgradeLvl)
    {
        return attackValuesHigher[upgradeLvl];
    }
}
