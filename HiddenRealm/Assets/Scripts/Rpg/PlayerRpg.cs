using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerRpg : EntityRpg
{
    [SyncVar]
    public string playerName = "Zuker";

    public long Exp { get; private set; }
    public long MaxExp { get; private set; }
    public int Level { get; private set; } = 0;
    public readonly int MAX_LEVEL = 99;

    public long Yang { get; private set; }

    public int StatusPoints { get; private set; }
    public bool[,] spAcquisitionTable;

    public float MaxHp { get; private set; } = 100f;
    public float Energy { get; private set; }
    public float MaxEnergy { get; private set; } = 100f;
    public float Stamina { get; private set; }
    public float MaxStamina { get; private set; } = 100f;
    public float HealthRegenRate { get; private set; } = 1f;

    public float BaseMaxHp { get; private set; }
    public float EqMaxHp { get; private set; }
    public float BaseMaxEnergy { get; private set; }
    public float EqMaxEnergy { get; private set; }

    public float AttackLower { get; set; }
    public float AttackHigher { get; set; }
    public float Defense { get; set; }
    public float BaseAttackLower { get; set; }
    public float BaseAttackHigher { get; set; }
    public float BaseDefense { get; set; }
    public float EqAttackLower { get; set; }
    public float EqAttackHigher { get; set; }
    public float EqDefense { get; set; }
    public float EqArmorDefense { get; set; }
    public float EqHelmetDefense { get; set; }
    public float EqShieldDefense { get; set; }

    public int Vit { get; set; }
    public int Int { get; set; }
    public int Str { get; set; }
    public int Dex { get; set; }

    public int BaseVit { get; set; }
    public int BaseInt { get; set; }
    public int BaseStr { get; set; }
    public int BaseDex { get; set; }

    public int EqVit { get; set; }
    public int EqInt { get; set; }
    public int EqStr { get; set; }
    public int EqDex { get; set; }

    public bool isMoving = false;
    public bool isAttacking = false;

    [SyncVar]
    public bool IsDead = false;

    [SyncVar]
    public float dmgPublic;

    // Start is called before the first frame update
    void Start()
    {
        if(isLocalPlayer)
        {
            //HERE IS ALL STARTING SETUP
            LevelUp();
            SetupStartingStats();
            InvokeRepeating("HealthRegen", 1.0f, 5f);
            InvokeRepeating("BackupToDatabase", 180f, 3600f);
        }
    }

    private void BackupToDatabase()
    {
        if(!LoginScreen.isDeveloper)
        {
            if (SerializationDatabase.instance.loadedIn)
            {
                SerializationDatabase.instance.SaveItemsToDatabase();
            }
        }
    }

    public void SetupSerializedValues(SerialPlayerRpg spr)
    {
        CmdSetName(spr.cName);
        Yang = spr.yang;
        Level = spr.level;
        RefreshMaxExp();
        Exp = spr.exp;
        StatusPoints = spr.sPoints;
        spAcquisitionTable = GlobalSerialization.DeserializeSpAquisitionTable(spr.sPointsTab);

        BaseVit = spr.bVit;
        BaseInt = spr.bInt;
        BaseStr = spr.bStr;
        BaseDex = spr.bDex;
        CalculateStatsDerivatives();

        CmdManipulateHealth(spr.cHp);
        Energy = spr.cEn;
        Stamina = spr.cSt;
    }

    [Command]
    public void CmdSetName(string name)
    {
        playerName = name;
    }

    [Command]
    public void CmdChangeIsDead(bool value)
    {
        IsDead = value;
    }

    public bool PayYang(long value)
    {
        if(Yang >= value)
        {
            Yang -= value;
            return true;
        }
        else
        {
            Debug.Log("Not enough yang.");
            return false;
        }
    }

    public void SetupStatusAfterLoad()
    {
        if(StatusPoints > 0)
        {
            GetComponent<PlayerUI>().myUI.AddStatButtonsSetActive(true);
        }
    }

    public void AddStatusPoint(int orbNumber)
    {
        //orbNumber is 0, 1 or 2 and resembles the orb just filled
        if(spAcquisitionTable[Level - 1, orbNumber] == false)
        {
            StatusPoints++;
            spAcquisitionTable[Level - 1, orbNumber] = true;
            GetComponent<PlayerUI>().myUI.AddStatButtonsSetActive(true);
        }
    }
    public void RemoveStatusPoint()
    {
        StatusPoints--;
    }
    public void IncreaseBaseVit()
    {
        BaseVit++;
        CalculateStatsDerivatives();
    }
    public void IncreaseBaseInt()
    {
        BaseInt++;
        CalculateStatsDerivatives();
    }
    public void IncreaseBaseStr()
    {
        BaseStr++;
        CalculateStatsDerivatives();
    }
    public void IncreaseBaseDex()
    {
        BaseDex++;
        CalculateStatsDerivatives();
    }

    public void CalculateStatsDerivatives()
    {
        Vit = BaseVit + EqVit;
        BaseDefense = (Vit / 4) * 3;
        BaseMaxHp = Vit * 40;

        Int = BaseInt + EqInt;
        BaseMaxEnergy = Int * 20;

        Str = BaseStr + EqStr;
        BaseAttackLower = Str * 2;
        BaseAttackHigher = Str * 2;

        Dex = BaseDex + EqDex;
        BaseAttackLower += Dex;
        BaseAttackHigher += Dex;

        AttackLower = BaseAttackLower + EqAttackLower;
        AttackHigher = BaseAttackHigher + EqAttackHigher;
        MaxHp = BaseMaxHp + EqMaxHp;
        Defense = BaseDefense + EqDefense;
        MaxEnergy = BaseMaxEnergy + EqMaxEnergy;
    }

    private void SetupSpTab()
    {
        spAcquisitionTable = new bool[MAX_LEVEL, 3];
        for(int i = 0; i < MAX_LEVEL; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                spAcquisitionTable[i, j] = false;
            }
        }
    }

    private void SetupStartingStats()
    {
        BaseVit = 4;
        BaseInt = 3;
        BaseStr = 6;
        BaseDex = 3;
        SetupSpTab();
        CalculateStatsDerivatives();
        CmdManipulateHealth(MaxHp);
        Energy = MaxEnergy;
        Stamina = MaxStamina;
    }

    public void CancelHealthRegen()
    {
        CancelInvoke("HealthRegen");
    }

    public void InvokeHealthRegen()
    {
        InvokeRepeating("HealthRegen", 1.0f, 5f);
    }

    [Command]
    private void CmdManipulateHealth(float value)
    {
        currentHp = value;
    }

    private void HealthRegen()
    {
        if(isLocalPlayer)
        {
            if (currentHp < MaxHp)
            {
                if ((!isMoving) && (!isAttacking))
                {
                    CmdRestoreHealth(15f + (MaxHp * HealthRegenRate * 0.05f), MaxHp);
                }
                else
                {
                    CmdRestoreHealth(15f + (MaxHp * HealthRegenRate * 0.01f), MaxHp);
                }
                if (currentHp > MaxHp)
                {
                    CmdManipulateHealth(MaxHp);
                }
            }
        }
    }

    public void UseHealthPotion(float value)
    {
        //temporary instant potion
        CmdRestoreHealth(value, MaxHp);
        if (currentHp > MaxHp)
        {
            CmdManipulateHealth(MaxHp);
        }
    }

    public void RestoreHealthAfrerResurrection()
    {
        CmdManipulateHealth(0.1f * MaxHp);
    }

    [Command]
    private void CmdRestoreHealth(float value, float mHp)
    {
        currentHp += value;
        if(currentHp > mHp)
        {
            currentHp = mHp;
        }
    }

    public void Attack(GameObject other)
    {
        if(isLocalPlayer)
        {
            RandomizeDamageValue();
            CmdDealDamage(other, damage);
        }
    }

    //to be run only on the server
    public void AttackServerVersion(GameObject other)
    {
        if(isServer)
        {
            RpcRandomizeDmg(other, playerName);
        }
    }

    [Command]
    private void CmdChangeDmgPublic(float value)
    {
        dmgPublic = value;
    }

    [ClientRpc]
    private void RpcRandomizeDmg(GameObject other, string pName)
    {
        if(GameObject.Find("LocalPlayer").GetComponent<PlayerRpg>().playerName.Equals(pName))
        {
            RandomizeDamageValue();
            CmdDealDamage(other, damage);
        }
    }

    [Command]
    public void CmdDealDamage(GameObject other, float inptDamage)
    {
        other.GetComponent<EntityRpg>().currentHp -= inptDamage;
    }

    public void GetDamage(float value)
    {
        if (isLocalPlayer)
        {
            //defense mechanics
            float trueValue = value - Defense;
            if (trueValue < 0)
                trueValue = 0;
            CmdGetDamage(trueValue);
        }
    }

    [Command]
    private void CmdGetDamage(float value)
    {
        currentHp -= value;
    }

    private void LevelUp()
    {
        Level++;
        MaxExp = ExpTable.table[Level];
    }

    public void RefreshMaxExp()
    {
        MaxExp = ExpTable.table[Level];
    }

    public void AddExp(long value)
    {
        Exp += value;
        if(Exp >= MaxExp)
        {
            Exp -= MaxExp;
            LevelUp();
        }
    }

    public void RemoveExp(long value)
    {
        if(Exp >= value)
        {
            Exp -= value;
        }
        else
        {
            Exp = 0;
        }
    }

    public void RandomizeDamageValue()
    {
        damage = Mathf.Floor(Random.Range(AttackLower, AttackHigher));
    }

    public void CallCmdSpawn(GameObject go)
    {
        if (isLocalPlayer)
        {
            CmdSpawn(go);
        }
    }

    [Command]
    private void CmdSpawn(GameObject mob)
    {
        mob.GetComponent<MobRpg>().DropItems();
    }

    [Command]
    public void CmdDestroyOnServer(GameObject go)
    {
        NetworkServer.Destroy(go);
    }

    public void GrantYang(long value)
    {
        Yang += value;
    }
}
