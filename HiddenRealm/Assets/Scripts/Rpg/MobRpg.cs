using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MobRpg : EntityRpg
{
    private Mob data;
    private bool died = false;

    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<MobBehaviour>().GetData();
        currentHp = data.maxHp;
        damage = data.level * 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHp <= 0)
        {
            if (!died)
            {
                StartCoroutine(DeathCor());
                died = true;
            }
        }
    }

    private IEnumerator DeathCor()
    {
        while(GetComponent<MobBehaviour>().GetTarget() == null)
        {
            yield return null;
        }

        Debug.Log(gameObject.name + " dying.");
        GetComponent<MobBehaviour>().anim.SetBool("isdead", true);
        GetComponent<MobBehaviour>().nameplate.SetActive(false);
        GetComponent<MobBehaviour>().ExitCombat();
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<MobBehaviour>().hardCollider.SetActive(false);
        GetComponent<MobBehaviour>().UpdateParent();
        AddMultipliedExp();
        CallDropItems();
        GrantYangForKiller();
        GetComponent<MobBehaviour>().enabled = false;
    }

    public void DeathProcedure()
    {
        if (currentHp <= 0)
        {
            if (!died)
            {
                Debug.Log(gameObject.name + " dying.");
                GetComponent<MobBehaviour>().anim.SetBool("isdead", true);
                GetComponent<MobBehaviour>().nameplate.SetActive(false);
                GetComponent<MobBehaviour>().ExitCombat();
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                GetComponent<MobBehaviour>().hardCollider.SetActive(false);
                GetComponent<MobBehaviour>().UpdateParent();
                AddMultipliedExp();
                CallDropItems();
                GrantYangForKiller();
                GetComponent<MobBehaviour>().enabled = false;
                died = true;
            }
        }
    }

    public bool IsDead()
    {
        return died;
    }

    public void ResetHealth()
    {
        currentHp = data.maxHp;
    }

    public Item GetLootTableElem(int index)
    {
        return data.lootTable[index];
    }

    private void AddMultipliedExp()
    {
        if(GetComponent<MobBehaviour>().GetTarget() == null)
        {
            return;
        }

        float baseValue = data.exp;
        int levelDiff = data.level - GetComponent<MobBehaviour>().GetTarget().Level;
        float multiplier = 1f;
        if((levelDiff >= 1) && (levelDiff <= 15))
        {
            multiplier += levelDiff * 0.05f;
        }
        else if(levelDiff > 15)
        {
            multiplier = 1.75f;
        }
        else if((levelDiff >= -1) && (levelDiff <= 0))
        {
            multiplier = 1f;
        }
        else if((levelDiff < -1) && (levelDiff > -15))
        {
            switch(levelDiff)
            {
                case -2:
                    multiplier = 0.98f;
                    break;
                case -3:
                    multiplier = 0.96f;
                    break;
                case -4:
                    multiplier = 0.94f;
                    break;
                case -5:
                    multiplier = 0.92f;
                    break;
                case -6:
                    multiplier = 0.90f;
                    break;
                case -7:
                    multiplier = 0.85f;
                    break;
                case -8:
                    multiplier = 0.80f;
                    break;
                case -9:
                    multiplier = 0.70f;
                    break;
                case -10:
                    multiplier = 0.50f;
                    break;
                case -11:
                    multiplier = 0.30f;
                    break;
                case -12:
                    multiplier = 0.20f;
                    break;
                case -13:
                    multiplier = 0.10f;
                    break;
                case -14:
                    multiplier = 0.0125f;
                    break;
            }
        }
        else if(levelDiff <= -15)
        {
            multiplier = 0f;
        }
        long result = (long)(baseValue * multiplier);
        GetComponent<MobBehaviour>().GetTarget().AddExp(result);
    }

    public void DropItems()
    {
        float rng = Random.Range(0f, 1f);
        for(int i = 0; i < data.lootTable.Length; i++)
        {
            rng = Random.Range(0f, 1f);
            float lootChance = data.lootChances[i];
            lootChance *= GlobalMultipliers.DROP_CHANCE;
            if ((rng >= 0) && (rng <= lootChance))
            {
                GameObject lootbagGO = Instantiate(GetComponent<MobBehaviour>().LootBagPrefab, transform.position, Quaternion.identity);
                NetworkServer.Spawn(lootbagGO);
                lootbagGO.GetComponent<LootBag>().itemDatabaseIndex = -2;
                lootbagGO.GetComponent<LootBag>().itemDatabaseIndex = ItemsDatabase.instance.GetIndexOfItem(data.lootTable[i]);
                lootbagGO.GetComponent<LootBag>().lootName = data.lootTable[i].GetLabel();
            }
        }
    }

    private void CallDropItems()
    {
        GetComponent<MobBehaviour>().GetTarget().CallCmdSpawn(gameObject);
    }

    private void GrantYangForKiller()
    {
        float rng = Random.Range(data.yangLower, data.yangHigher);
        GetComponent<MobBehaviour>().GetTarget().GrantYang((long)rng);
    }
}
