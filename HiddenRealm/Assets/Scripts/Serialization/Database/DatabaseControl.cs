using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using Mirror;

public class DatabaseControl : MonoBehaviour
{
    [SerializeField]
    private bool singlePlayerDevBuild = false;

    private string connectionString;

    // Start is called before the first frame update
    void Start()
    {
        if(!singlePlayerDevBuild)
        {
            //actual server path
            connectionString = "URI=file:" + "/server/database/char_database.db";
        }
        else
        {
            connectionString = "URI=file:" + Application.dataPath + "/../Database/char_database.db"; //Path to database
            Debug.Log(connectionString);
        }
    }

    private string QueryReady(int inpt)
    {
        return "'" + inpt + "'";
    }

    private string QueryReady(float inpt)
    {
        return "'" + inpt + "'";
    }

    private string QueryReady(long inpt)
    {
        return "'" + inpt + "'";
    }

    private string QueryReady(string inpt)
    {
        return "'" + inpt + "'";
    }

    private int GetPlayerId(string playerName)
    {
        IDbConnection dbconn = new SqliteConnection(connectionString);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT IdC " + "FROM Characters WHERE CName='" + playerName + "'";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int idC = reader.GetInt32(0);

            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
            return idC;
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        return -1;
    }

    private int GetAccountId(string login, string password)
    {
        int idA = -1;

        using (IDbConnection dbconn = new SqliteConnection(connectionString))
        {
            dbconn.Open();
            using (IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "SELECT IdA FROM Accounts WHERE Login=" + QueryReady(login) + " AND Password=" + QueryReady(password);
                dbcmd.CommandText = sqlQuery;
                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        idA = reader.GetInt32(0);
                    }
                }
            }
        }

        return idA;
    }

    public string GetAccountPwd(string login)
    {
        string pwd = "";

        using (IDbConnection dbconn = new SqliteConnection(connectionString))
        {
            dbconn.Open();
            using (IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "SELECT Pwd FROM Accounts WHERE Login=" + QueryReady(login);
                dbcmd.CommandText = sqlQuery;
                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pwd = reader.GetString(0);
                    }
                }
            }
        }

        return pwd;
    }

    public string GetAccountPw(string login)
    {
        string pw = "";

        using (IDbConnection dbconn = new SqliteConnection(connectionString))
        {
            dbconn.Open();
            using (IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "SELECT Password FROM Accounts WHERE Login=" + QueryReady(login);
                dbcmd.CommandText = sqlQuery;
                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pw = reader.GetString(0);
                    }
                }
            }
        }

        return pw;
    }

    private string GetFirstCharacterNameOfAccount(int idA)
    {
        string cname = "";

        if(idA == -1)
        {
            cname = "_no_acc";
        }

        using (IDbConnection dbconn = new SqliteConnection(connectionString))
        {
            dbconn.Open();
            using (IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "SELECT CName FROM Characters WHERE Account=" + QueryReady(idA);
                dbcmd.CommandText = sqlQuery;
                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cname = reader.GetString(0);
                    }
                }
            }
        }

        return cname;
    }

    public string GetCharacterOfAcc(string login, string password)
    {
        int idA = GetAccountId(login, password);
        return GetFirstCharacterNameOfAccount(idA);
    }

    private void CreateNewCharactersRecord(string playerName, int idA)
    {
        string _acc = QueryReady(idA);
        string _cname = QueryReady(playerName);
        string _yang = QueryReady(0);
        string _level = QueryReady(0);
        string _exp = QueryReady(0);
        string _sp = QueryReady(0);
        string _spt = QueryReady(" ");
        string _chp = QueryReady(0);
        string _cen = QueryReady(0);
        string _cst = QueryReady(0);
        string _bv = QueryReady(0);
        string _bi = QueryReady(0);
        string _bs = QueryReady(0);
        string _bd = QueryReady(0);

        using (IDbConnection dbconn = new SqliteConnection(connectionString))
        {
            dbconn.Open();
            using (IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "INSERT INTO Characters (Account, CName, Yang, Level, Exp, StatusPoints, StatusPointsTab, CurrentHp," 
                                    + " CurrentEnergy, CurrentStamina, BaseVit, BaseInt, BaseStr, BaseDex) values ("
                                    + _acc + ", "
                                    + _cname + ", "
                                    + _yang + ", "
                                    + _level + ", "
                                    + _exp + ", "
                                    + _sp + ", "
                                    + _spt + ", "
                                    + _chp + ", "
                                    + _cen + ", "
                                    + _cst + ", "
                                    + _bv + ", "
                                    + _bi + ", "
                                    + _bs + ", "
                                    + _bd
                                    + ")";
                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteNonQuery();
            }
        }
    }

    private void CreateNewCharacterPositionsRecord(string playerName)
    {
        string playerId = QueryReady(GetPlayerId(playerName));
        string x = QueryReady(0);
        string y = QueryReady(0);
        string z = QueryReady(0);

        using (IDbConnection dbconn = new SqliteConnection(connectionString))
        {
            dbconn.Open();
            using (IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "INSERT INTO CharacterPositions (IdC, PositionX, PositionY, RotationZ) values ("
                                    + playerId + ", "
                                    + x + ", "
                                    + y + ", "
                                    + z
                                    + ")";
                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteNonQuery();
            }
        }
    }

    private void CreateNewInventoriesRecord(string playerName)
    {
        string playerId = QueryReady(GetPlayerId(playerName));
        string actionBar = QueryReady("000000000000000000000000");

        using (IDbConnection dbconn = new SqliteConnection(connectionString))
        {
            dbconn.Open();
            using (IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "INSERT INTO Inventories (IdC, ActionBar) values ("
                                    + playerId + ", "
                                    + actionBar
                                    + ")";
                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteNonQuery();
            }
        }
    }

    public void CreateNewCharacter(string login, string password, string newPlayerName)
    {
        int idA = GetAccountId(login, password);
        CreateNewCharactersRecord(newPlayerName, idA);
        CreateNewCharacterPositionsRecord(newPlayerName);
        CreateNewInventoriesRecord(newPlayerName);
    }

    public void ClearItemsOfPlayer(string playerName)
    {
        using (IDbConnection dbconn = new SqliteConnection(connectionString))
        {
            dbconn.Open();
            using (IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "DELETE FROM Items WHERE IdC=" + QueryReady(GetPlayerId(playerName));
                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteNonQuery();
            }
        }
    }

    private void SaveItemOfPlayerFromSerial(SerialItem sitem, string playerName)
    {
        string playerId = QueryReady(GetPlayerId(playerName));
        string id = QueryReady(sitem.itemID);
        string x = QueryReady(sitem.x);
        string y = QueryReady(sitem.y);
        string upgrade = QueryReady(sitem.upgradeLvl);
        string quantity = QueryReady(sitem.quantity);

        using (IDbConnection dbconn = new SqliteConnection(connectionString))
        {
            dbconn.Open();
            using (IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "INSERT INTO Items (IdC, ItemID, XIndex, YIndex, UpgradeLvl, Quantity) values ("
                                    + playerId + ", "
                                    + id + ", "
                                    + x + ", "
                                    + y + ", "
                                    + upgrade + ", "
                                    + quantity
                                    + ")";
                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteNonQuery();
            }
        }
    }

    public void SaveItemOfPlayer(int id, int x, int y, int upgrade, int quantity, string playerName)
    {
        SaveItemOfPlayerFromSerial(new SerialItem(id, x, y, upgrade, quantity), playerName);
    }

    public List<SerialItem> LoadItemsOfPlayer(string playerName)
    {
        List<SerialItem> loadedItems = new List<SerialItem>();

        IDbConnection dbconn;
        dbconn = new SqliteConnection(connectionString);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT ItemID, XIndex, YIndex, UpgradeLvl, Quantity " + "FROM Items WHERE IdC=" + QueryReady(GetPlayerId(playerName));
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int itemId = reader.GetInt32(0);
            int x = reader.GetInt32(1);
            int y = reader.GetInt32(2);
            int upgrade = reader.GetInt32(3);
            int quantity = reader.GetInt32(4);

            //Debug.Log("idA: " + idA + " login: " + login + " password: " + password + " email: " + email);
            loadedItems.Add(new SerialItem(itemId, x, y, upgrade, quantity));
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

        return loadedItems;
    }

    public void SaveActionbarOfPlayer(string actionBar, string playerName)
    {
        string playerId = QueryReady(GetPlayerId(playerName));

        using (IDbConnection dbconn = new SqliteConnection(connectionString))
        {
            dbconn.Open();
            using (IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "UPDATE Inventories SET ActionBar='" + actionBar + "' WHERE IdC=" + playerId;
                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteNonQuery();
            }
        }
    }

    public string LoadActionBarOfPlayer(string playerName)
    {
        string playerId = QueryReady(GetPlayerId(playerName));
        string result = "";

        using (IDbConnection dbconn = new SqliteConnection(connectionString))
        {
            dbconn.Open();
            using (IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "SELECT ActionBar FROM Inventories WHERE IdC=" + playerId;
                dbcmd.CommandText = sqlQuery;
                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = reader.GetString(0);
                    }
                }
            }
        }

        return result;
    }

    public void SavePositionOfPlayer(float posX, float posY, float rotZ, string playerName)
    {
        string playerId = QueryReady(GetPlayerId(playerName));

        using (IDbConnection dbconn = new SqliteConnection(connectionString))
        {
            dbconn.Open();
            using (IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "UPDATE CharacterPositions" 
                    + " SET PositionX='" + posX 
                    + "', PositionY='" + posY 
                    + "', RotationZ='" + rotZ 
                    + "' WHERE IdC=" + playerId;
                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteNonQuery();
            }
        }
    }

    public SerialPosition LoadPositionOfPlayer(string playerName)
    {
        string playerId = QueryReady(GetPlayerId(playerName));
        SerialPosition sPos = null;

        using (IDbConnection dbconn = new SqliteConnection(connectionString))
        {
            dbconn.Open();
            using (IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "SELECT PositionX, PositionY, RotationZ FROM CharacterPositions WHERE IdC=" + playerId;
                dbcmd.CommandText = sqlQuery;
                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        float x = reader.GetFloat(0);
                        float y = reader.GetFloat(1);
                        float z = reader.GetFloat(2);

                        sPos = new SerialPosition(x, y, z);
                    }
                }
            }
        }

        return sPos;
    }

    public void SavePlayerRpg(SerialPlayerRpg spr, string playerName)
    {
        string playerId = QueryReady(GetPlayerId(playerName));

        using (IDbConnection dbconn = new SqliteConnection(connectionString))
        {
            dbconn.Open();
            using (IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "UPDATE Characters"
                    + " SET CName=" + QueryReady(spr.cName)
                    + ", Yang=" + QueryReady(spr.yang)
                    + ", Level=" + QueryReady(spr.level)
                    + ", Exp=" + QueryReady(spr.exp)
                    + ", StatusPoints=" + QueryReady(spr.sPoints)
                    + ", StatusPointsTab=" + QueryReady(spr.sPointsTab)
                    + ", CurrentHp=" + QueryReady(spr.cHp)
                    + ", CurrentEnergy=" + QueryReady(spr.cEn)
                    + ", CurrentStamina=" + QueryReady(spr.cSt)
                    + ", BaseVit=" + QueryReady(spr.bVit)
                    + ", BaseInt=" + QueryReady(spr.bInt)
                    + ", BaseStr=" + QueryReady(spr.bStr)
                    + ", BaseDex=" + QueryReady(spr.bDex)
                    + " WHERE IdC=" + playerId;
                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteNonQuery();
            }
        }
    }

    public SerialPlayerRpg LoadPlayerRpg(string playerName)
    {
        string playerId = QueryReady(GetPlayerId(playerName));
        SerialPlayerRpg spr = null;

        using (IDbConnection dbconn = new SqliteConnection(connectionString))
        {
            dbconn.Open();
            using (IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "SELECT CName, Yang, Level, Exp, StatusPoints, StatusPointsTab, CurrentHp," 
                    + " CurrentEnergy, CurrentStamina, BaseVit, BaseInt, BaseStr, BaseDex"
                    + " FROM Characters WHERE IdC=" + playerId;
                dbcmd.CommandText = sqlQuery;
                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string cname = reader.GetString(0);
                        long yang = reader.GetInt64(1);
                        int lvl = reader.GetInt32(2);
                        long exp = reader.GetInt64(3);
                        int spoints = reader.GetInt32(4);
                        string spointstab = reader.GetString(5);
                        float chp = reader.GetFloat(6);
                        float cen = reader.GetFloat(7);
                        float cst = reader.GetFloat(8);
                        int bvit = reader.GetInt32(9);
                        int bint = reader.GetInt32(10);
                        int bstr = reader.GetInt32(11);
                        int bdex = reader.GetInt32(12);

                        spr = new SerialPlayerRpg(cname, yang, lvl, exp, spoints, spointstab, chp, cen, cst, bvit, bint, bstr, bdex);
                    }
                }
            }
        }

        return spr;
    }
}
