using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerAttacks : NetworkBehaviour
{
    private Animator anim;

    [SerializeField]
    private GameObject weapon = null;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAuthority)
        {
            LiveAttack();
        }
    }

    private void LiveAttack()
    {
        if(GetComponent<PlayerUI>().myUI.ChatGO.GetComponent<ChatHandler>().mode == 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetBool("isattacking", true);
                GetComponent<PlayerRpg>().isAttacking = true;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                anim.SetBool("isattacking", false);
                GetComponent<PlayerRpg>().isAttacking = false;
                weapon.SetActive(false);
            }
        }
    }

    public void StopAttacking()
    {
        anim.SetBool("isattacking", false);
        GetComponent<PlayerRpg>().isAttacking = false;
        weapon.SetActive(false);
    }
}
