using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerDeath : NetworkBehaviour
{
    private Animator anim;
    private PlayerRpg playerRpg;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerRpg = GetComponent<PlayerRpg>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerRpg.currentHp <= 0)
        {
            if(isLocalPlayer)
            {
                anim.SetBool("dead", true);
                playerRpg.CmdChangeIsDead(true);
                playerRpg.CancelHealthRegen();
                GetComponent<PlayerUI>().myUI.ResScreenGO.SetActive(true);
            }
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttacks>().StopAttacking();
            GetComponent<PlayerAttacks>().enabled = false;
            GetComponent<LookAtMouse>().enabled = false;
        }
    }

    public void ResurrectHere()
    {
        StartCoroutine(ResHereCor());
    }

    private IEnumerator ResHereCor()
    {
        playerRpg.RestoreHealthAfrerResurrection();
        while(playerRpg.currentHp <= 0)
        {
            yield return null;
        }
        anim.SetBool("dead", false);
        playerRpg.CmdChangeIsDead(false);
        playerRpg.InvokeHealthRegen();

        //remove exp
        float toRemove = 0.05f * playerRpg.MaxExp;
        playerRpg.RemoveExp((long)toRemove);

        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerAttacks>().enabled = true;
        GetComponent<LookAtMouse>().enabled = true;
        GetComponent<PlayerUI>().myUI.ResScreenGO.SetActive(false);
    }


    public void ResurrectInTown()
    {
        StartCoroutine(ResInTownCor());
    }

    private IEnumerator ResInTownCor()
    {
        transform.position = GameObject.Find("TownResurrectionSpot").transform.position;

        playerRpg.RestoreHealthAfrerResurrection();
        while (playerRpg.currentHp <= 0)
        {
            yield return null;
        }
        anim.SetBool("dead", false);
        playerRpg.CmdChangeIsDead(false);
        playerRpg.InvokeHealthRegen();
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerAttacks>().enabled = true;
        GetComponent<LookAtMouse>().enabled = true;
        GetComponent<PlayerUI>().myUI.ResScreenGO.SetActive(false);
    }
}
