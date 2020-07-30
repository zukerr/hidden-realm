using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EqPiece
{
    Weapon,
    Armor,
    Helmet,
    Shield,
    Boots,
    Earrings,
    Necklace,
    Bracelet
}

public abstract class EquippableItem : Item
{
    public int levelRequirement;
    public int inventorySlots;
    public EqPiece eqPiece;
}
