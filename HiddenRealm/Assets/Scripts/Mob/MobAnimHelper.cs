using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAnimHelper : MonoBehaviour
{
    [SerializeField]
    private MobBehaviour mobBehav = null;

    public void AttachmentFunction()
    {
        mobBehav.DealDamageToPlayer();
    }
}
