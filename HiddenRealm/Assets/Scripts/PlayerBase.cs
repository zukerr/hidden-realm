using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerBase : NetworkBehaviour
{
    public static PlayerBase instance;

    private List<GameObject> players;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        players = new List<GameObject>();
    }

    [Command]
    public void CmdAddPlayer(GameObject player)
    {
        players.Add(player);
    }
}
