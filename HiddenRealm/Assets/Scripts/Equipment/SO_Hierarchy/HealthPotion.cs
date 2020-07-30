using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Health Potion", menuName = "Item/Usable/Health Potion")]
public class HealthPotion : UsableItem
{
    public float healthAmount;
    public long cost;
}
