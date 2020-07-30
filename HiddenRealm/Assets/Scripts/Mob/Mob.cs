using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mob", menuName = "Mob")]
public class Mob : ScriptableObject
{
    public int level;
    public int mobClass;
    public long exp;
    public float maxHp;
    public string mobName;
    public GameObject prefab;

    public Item[] lootTable;
    public float[] lootChances;

    public long yangLower;
    public long yangHigher;
}
