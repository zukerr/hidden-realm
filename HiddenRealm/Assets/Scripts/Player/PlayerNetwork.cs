using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerNetwork : NetworkBehaviour
{
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        gameObject.name = "LocalPlayer";
    }

    public void SerializeItem(SerialItem sitem)
    {
        if(isLocalPlayer)
        {
            CmdSaveItemToDatabase(sitem.itemID, sitem.x, sitem.y, sitem.upgradeLvl, sitem.quantity, GetComponent<PlayerRpg>().playerName);
        }
    }

    [Command]
    private void CmdSaveItemToDatabase(int id, int x, int y, int upgrade, int quantity, string playerName)
    {
        SerializationDatabase.instance.databaseControl.SaveItemOfPlayer(id, x, y, upgrade, quantity, playerName);
    }

    public void CallRpcLoadItemFromDatabase(SerialItem sitem, string playerName)
    {
        if(isServer)
        {
            RpcLoadItemFromDatabase(sitem.itemID, sitem.x, sitem.y, sitem.upgradeLvl, sitem.quantity, playerName);
        }
    }

    [ClientRpc]
    private void RpcLoadItemFromDatabase(int id, int x, int y, int upgrade, int quantity, string playerName)
    {
        if (GameObject.Find("LocalPlayer").GetComponent<PlayerRpg>().playerName.Equals(playerName))
        {
            GlobalSerialization.DeserializeItem(new SerialItem(id, x, y, upgrade, quantity), SerializationDatabase.instance.inventory);
        }
    }

    public void CallCmdInitiateLoadFromClient()
    {
        if(isLocalPlayer)
        {
            CmdInitiateLoadFromClient(GetComponent<PlayerRpg>().playerName);
        }
    }

    [Command]
    public void CmdInitiateLoadFromClient(string playerName)
    {
        SerializationDatabase.instance.SetupItemsFromDatabase(playerName);
    }

    public void CallCmdClearItemsOfPlayer()
    {
        if(isLocalPlayer)
        {
            CmdClearItemsOfPlayer(GetComponent<PlayerRpg>().playerName);
        }
    }

    [Command]
    private void CmdClearItemsOfPlayer(string playerName)
    {
        SerializationDatabase.instance.databaseControl.ClearItemsOfPlayer(playerName);
    }

    public void CallCmdSerializeActionBar(string actionBar)
    {
        if(isLocalPlayer)
        {
            CmdSerializeActionBar(actionBar, GetComponent<PlayerRpg>().playerName);
        }
    }

    [Command]
    private void CmdSerializeActionBar(string actionBar, string playerName)
    {
        SerializationDatabase.instance.databaseControl.SaveActionbarOfPlayer(actionBar, playerName);
    }

    public void CallRpcLoadActionBarFromDatabase(string actionBar, string playerName)
    {
        if(isServer)
        {
            RpcLoadActionBarFromDatabase(actionBar, playerName);
        }
    }

    [ClientRpc]
    private void RpcLoadActionBarFromDatabase(string actionBar, string playerName)
    {
        if (GameObject.Find("LocalPlayer").GetComponent<PlayerRpg>().playerName.Equals(playerName))
        {
            GlobalSerialization.DeserializeActionBar(actionBar, SerializationDatabase.instance.inventory, SerializationDatabase.instance.abs);
        }
    }

    public void CallCmdSerializePlayerPosition()
    {
        if(isLocalPlayer)
        {
            SerialPosition spos = GlobalSerialization.SerializePosition(gameObject);
            CmdSerializePlayerPosition(spos.posX, spos.posY, spos.rotZ, GetComponent<PlayerRpg>().playerName);
        }
    }

    [Command]
    private void CmdSerializePlayerPosition(float x, float y, float z, string playerName)
    {
        SerializationDatabase.instance.databaseControl.SavePositionOfPlayer(x, y, z, playerName);
    }

    public void CallRpcLoadPositionFromDatabase(SerialPosition spos, string playerName)
    {
        if(isServer)
        {
            RpcLoadPositionFromDatabase(spos.posX, spos.posY, spos.rotZ, playerName);
        }
    }

    [ClientRpc]
    private void RpcLoadPositionFromDatabase(float x, float y, float z, string playerName)
    {
        PlayerRpg localPlayer = GameObject.Find("LocalPlayer").GetComponent<PlayerRpg>();
        if (localPlayer.playerName.Equals(playerName))
        {
            GlobalSerialization.DeserializePosition(new SerialPosition(x, y, z), localPlayer.gameObject);
        }
    }

    public void CallCmdSerializePlayerRpg()
    {
        if(isLocalPlayer)
        {
            SerialPlayerRpg spr = GlobalSerialization.SerializePlayerRpg(GetComponent<PlayerRpg>());
            CmdSerializePlayerRpg(spr.cName, spr.yang, spr.level, spr.exp,
                spr.sPoints, spr.sPointsTab, spr.cHp, spr.cEn, spr.cSt, 
                spr.bVit, spr.bInt, spr.bStr, spr.bDex, GetComponent<PlayerRpg>().playerName);
        }
    }

    [Command]
    private void CmdSerializePlayerRpg(string cName, long yang, int level, long exp,
        int sPoints, string sPointsTab, float cHp, float cEn, float cSt,
        int bVit, int bInt, int bStr, int bDex, string playerName)
    {
        SerialPlayerRpg spr = new SerialPlayerRpg(cName, yang, level, exp, sPoints, sPointsTab, cHp, cEn, cSt, bVit, bInt, bStr, bDex);
        SerializationDatabase.instance.databaseControl.SavePlayerRpg(spr, playerName);
    }

    public void CallRpcLoadPlayerRpgFromDatabase(SerialPlayerRpg spr, string playerName)
    {
        if(isServer)
        {
            RpcLoadPlayerRpgFromDatabase(spr.cName, spr.yang, spr.level, spr.exp,
                spr.sPoints, spr.sPointsTab, spr.cHp, spr.cEn, spr.cSt,
                spr.bVit, spr.bInt, spr.bStr, spr.bDex, playerName);
        }
    }

    [ClientRpc]
    private void RpcLoadPlayerRpgFromDatabase(string cName, long yang, int level, long exp,
        int sPoints, string sPointsTab, float cHp, float cEn, float cSt,
        int bVit, int bInt, int bStr, int bDex, string playerName)
    {
        PlayerRpg localPlayer = GameObject.Find("LocalPlayer").GetComponent<PlayerRpg>();
        if (localPlayer.playerName.Equals(playerName))
        {
            GlobalSerialization.DeserializePlayerRpg(new SerialPlayerRpg(cName,
                yang, level, exp, sPoints, sPointsTab, cHp, cEn, cSt, bVit, bInt, bStr, bDex), localPlayer);
            localPlayer.SetupStatusAfterLoad();
        }
    }

    public void CallCmdCheckPlayerName(string login, string password)
    {
        if(isLocalPlayer)
        {
            CmdCheckPlayerName(login, password);
        }
    }

    [Command]
    private void CmdCheckPlayerName(string login, string password)
    {
        string cname = SerializationDatabase.instance.databaseControl.GetCharacterOfAcc(login, password);
        RpcSetupPlayerName(login, cname);
    }

    [ClientRpc]
    private void RpcSetupPlayerName(string login, string cname)
    {
        if(isLocalPlayer)
        {
            if(LoginScreen.login.Equals(login))
            {
                if (cname == "")
                {
                    Debug.Log("login successful. no characters on the account.");
                    LoginScreen.noCharacter = true;
                    LoginScreen.loginSuccessful = true;
                }
                else if (cname == "_no_acc")
                {
                    Debug.Log("login unsuccessful. account doesn't exist.");
                    LoginScreen.loginSuccessful = false;
                }
                else
                {
                    LoginScreen.loginSuccessful = true;
                    LoginScreen.noCharacter = false;
                    GetComponent<PlayerRpg>().playerName = cname;
                    GetComponent<PlayerUI>().CmdChangePublicName(cname);
                }
                LoginScreen.loginComplete = true;
            }
        }
    }

    public void CallCmdCreateNewCharacter(string login, string password, string characterName)
    {
        if(isLocalPlayer)
        {
            CmdCreateNewCharacter(login, password, characterName);
        }
    }

    [Command]
    private void CmdCreateNewCharacter(string login, string password, string characterName)
    {
        SerializationDatabase.instance.databaseControl.CreateNewCharacter(login, password, characterName);
        RpcNewCharReady(login);
    }

    [ClientRpc]
    private void RpcNewCharReady(string login)
    {
        if (isLocalPlayer)
        {
            if (LoginScreen.login.Equals(login))
            {
                LoginScreen.newCharacterReady = true;
            }
        }
    }

    public void CallCmdRetrieveSalt(string login)
    {
        if (isLocalPlayer)
        {
            CmdRetrieveSalt(login);
        }
    }

    [Command]
    private void CmdRetrieveSalt(string login)
    {
        string salt = SerializationDatabase.instance.databaseControl.GetAccountPwd(login);
        string pw = SerializationDatabase.instance.databaseControl.GetAccountPw(login);
        RpcRetrieveSalt(login, salt, pw);
    }

    [ClientRpc]
    private void RpcRetrieveSalt(string login, string salt, string pw)
    {
        if (isLocalPlayer)
        {
            if (LoginScreen.login.Equals(login))
            {
                LoginScreen.pwSaltBuffer = salt;
                LoginScreen.pwBuffer = pw;
            }
        }
    }


    public void InitiateChatMsgSync(string msg, string name)
    {
        if (isLocalPlayer)
        {
            CmdInitiateChatSync(msg, name);
        }
    }

    [Command]
    private void CmdInitiateChatSync(string msg, string name)
    {
        RpcDisplayMsg(msg, name);
    }

    [ClientRpc]
    private void RpcDisplayMsg(string msg, string name)
    {
        GameObject localPlayer = GameObject.Find("LocalPlayer");
        if(localPlayer.GetComponent<PlayerRpg>().playerName.Equals(name))
        {
            return;
        }
        Transform initiatingPlayer = null;
        Transform playerContainer = GameObject.Find("PlayerContainer").transform;
        for(int i = 0; i < playerContainer.childCount; i++)
        {
            if(playerContainer.GetChild(i).GetComponent<PlayerRpg>().playerName.Equals(name))
            {
                initiatingPlayer = playerContainer.GetChild(i);
                break;
            }
        }
        
        if (Vector3.Distance(localPlayer.transform.position, initiatingPlayer.position) < 30f)
        {
            localPlayer.GetComponent<PlayerUI>().myUI.ChatGO.GetComponent<ChatHandler>().DisplayMessage(name + ": " + msg);
        }
    }

    public void CallCmdMobGetHit(GameObject mob, GameObject player)
    {
        if(isLocalPlayer)
        {
            CmdMobGetHit(mob, player);
        }
    }

    [Command]
    private void CmdMobGetHit(GameObject mob, GameObject player)
    {
        mob.GetComponent<MobBehaviour>().GetHitOnServer(player);
    }
}
