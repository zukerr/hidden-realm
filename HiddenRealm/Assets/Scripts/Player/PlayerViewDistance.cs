using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerViewDistance : NetworkBehaviour
{
    private Transform mobContainer;
    private Transform playerContainer;

    // Start is called before the first frame update
    void Start()
    {
        playerContainer = GameObject.Find("PlayerContainer").transform;
        transform.SetParent(playerContainer);
        if (isLocalPlayer)
        {
            if(gameObject.name.Equals("LocalPlayer"))
            {
                mobContainer = GameObject.Find("MobContainer").transform;
                InvokeRepeating("FilterMobs", 2f, 1f);
                InvokeRepeating("FilterPlayers", 2f, 1.5f);
            }
        }
    }

    private void FilterMobs()
    {
        for(int i = 0; i < mobContainer.childCount; i++)
        {
            if(Vector3.Distance(transform.position, mobContainer.GetChild(i).position) > 30f)
            {
                //mobContainer.GetChild(i).gameObject.SetActive(false);
                ChangeStateOfMob(mobContainer.GetChild(i), false);
            }
            else
            {
                if(mobContainer.GetChild(i).GetComponent<NetworkAnimator>().enabled == false)
                {
                    ChangeStateOfMob(mobContainer.GetChild(i), true);
                }
            }
        }
    }

    private void ChangeStateOfMob(Transform mob, bool state)
    {
        mob.GetComponent<NetworkAnimator>().enabled = state;
        mob.GetComponent<MobBehaviour>().enabled = state;
        mob.GetComponent<MobRpg>().enabled = state;
        mob.GetComponent<BoxCollider2D>().enabled = state;

        for (int i = 0; i < mob.childCount; i++)
        {
            mob.GetChild(i).gameObject.SetActive(state);
        }
    }

    private void FilterPlayers()
    {
        for(int i = 0; i < playerContainer.childCount; i++)
        {
            if(!playerContainer.GetChild(i).gameObject.name.Equals("LocalPlayer"))
            {
                if (Vector3.Distance(transform.position, playerContainer.GetChild(i).position) > 30f)
                {
                    ChangeStateOfPlayer(playerContainer.GetChild(i), false);
                }
                else
                {
                    if (playerContainer.GetChild(i).GetComponent<NetworkAnimator>().enabled == false)
                    {
                        ChangeStateOfPlayer(playerContainer.GetChild(i), true);
                    }
                }
            }
        }
    }

    private void ChangeStateOfPlayer(Transform player, bool state)
    {
        if(!state)
        {
            GameObject namePlate = player.GetComponent<PlayerUI>().nameplate.transform.parent.gameObject;
            player.GetComponent<PlayerUI>().enabled = state;
            namePlate.SetActive(state);
        }
        else
        {
            player.GetComponent<PlayerUI>().enabled = state;
            GameObject namePlate = player.GetComponent<PlayerUI>().nameplate.transform.parent.gameObject;
            namePlate.SetActive(state);
        }

        player.GetComponent<PlayerMovement>().enabled = state;
        player.GetComponent<PlayerAttacks>().enabled = state;
        player.GetComponent<LookAtMouse>().enabled = state;
        player.GetComponent<PlayerDeath>().enabled = state;
        player.GetComponent<PlayerRpg>().enabled = state;
        player.GetComponent<PlayerThrowItem>().enabled = state;
        player.GetComponent<NetworkAnimator>().enabled = state;
        player.GetComponent<Animator>().enabled = state;
        player.GetComponent<SpriteRenderer>().enabled = state;
        player.GetComponent<BoxCollider2D>().enabled = state;
    }
}
