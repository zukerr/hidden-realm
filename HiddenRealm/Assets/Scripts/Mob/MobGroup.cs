using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mob Group", menuName = "Mob Group")]
public class MobGroup : ScriptableObject
{
    public List<Mob> mobs;
}
