using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class EntityRpg : NetworkBehaviour
{
    [SerializeField]
    public float damage;

    [SyncVar]
    public float currentHp = 1f;

    public void DealDamage(EntityRpg target)
    {
        target.currentHp -= damage;
    }
}
