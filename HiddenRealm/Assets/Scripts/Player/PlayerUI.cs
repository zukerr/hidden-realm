using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : NetworkBehaviour
{
    public UIHandler myUI;

    private PlayerRpg playerRpg;

    private List<GameObject> lootables;

    private int expBallState;

    public bool loaded = false;

    [SyncVar(hook = "SetupPublicName")]
    public string playerNamePublic;

    [SerializeField]
    public TextMeshProUGUI nameplate;

    // Start is called before the first frame update
    void Start()
    {
        nameplate.transform.parent.SetParent(null);
        nameplate.transform.parent.rotation = new Quaternion(0, 0, 0, nameplate.transform.parent.rotation.w);
        if (isLocalPlayer)
        {
            myUI = GameObject.Find("UI").GetComponent<UIHandler>();
            myUI.playerUI = GetComponent<PlayerUI>();
            playerRpg = GetComponent<PlayerRpg>();
            UpdatePlayerStats();
        }  
        //Debug.Log(playerRpg.gameObject.name);
        
        lootables = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(nameplate != null)
        {
            nameplate.transform.parent.position = transform.position;
        }
        if (isLocalPlayer)
        {
            if (nameplate.text != playerNamePublic)
            {
                nameplate.text = playerNamePublic;
            }

            myUI.healthImg.fillAmount = playerRpg.currentHp / playerRpg.MaxHp;
            myUI.energyImg.fillAmount = playerRpg.Energy / playerRpg.MaxEnergy;
            myUI.staminaImg.fillAmount = playerRpg.Stamina / playerRpg.MaxStamina;
            if(loaded)
            {
                UpdateExp();
            }

            myUI.StatsLevel.text = playerRpg.Level.ToString();
            myUI.StatsExp.text = playerRpg.Exp.ToString();
            myUI.StatsExpToLevel.text = (playerRpg.MaxExp - playerRpg.Exp).ToString();

            myUI.StatsVit.text = playerRpg.Vit.ToString();
            myUI.StatsInt.text = playerRpg.Int.ToString();
            myUI.StatsStr.text = playerRpg.Str.ToString();
            myUI.StatsDex.text = playerRpg.Dex.ToString();

            myUI.StatsHpText.text = Mathf.Round(playerRpg.currentHp) + "/" + playerRpg.MaxHp;
            myUI.StatsEpText.text = playerRpg.Energy + "/" + playerRpg.MaxEnergy;
            myUI.StatsAttackText.text = playerRpg.AttackLower + "-" + playerRpg.AttackHigher;
            myUI.StatsDefenseText.text = playerRpg.Defense.ToString();
            myUI.YangText.text = playerRpg.Yang.ToString() + " Yang";
        }
        //UpdatePlayerStats();
    }

    private void OnDestroy()
    {
        if(nameplate != null)
        {
            Destroy(nameplate.transform.parent.gameObject);
        }
    }

    [Command]
    public void CmdChangePublicName(string str)
    {
        playerNamePublic = str;
    }

    public void SetupPublicName(string oldVal, string newVal)
    {
        nameplate.text = newVal;
    }

    public void AddToLootables(GameObject item)
    {
        lootables.Add(item);
    }

    public void RemoveFromLootables(GameObject item)
    {
        lootables.Remove(item);
    }

    public void Loot()
    {
        if(lootables.Count == 0)
        {
            return;
        }
        int rng = Random.Range(0, lootables.Count);
        Debug.Log("picking quan: " + lootables[rng].GetComponent<LootBag>().quantity);
        bool result = myUI.inventory.PickupItem(lootables[rng].GetComponent<LootBag>().LootItem, 
            lootables[rng].GetComponent<LootBag>().upgradeLvl, lootables[rng].GetComponent<LootBag>().quantity);
        //NetworkServer.Destroy(lootables[rng]);
        if(result)
        {
            playerRpg.CmdDestroyOnServer(lootables[rng]);
        }
        else
        {
            myUI.ShowInfoBox("Inventory is full!");
        }
    }

    private void UpdateExp()
    {
        float expPercentage = (float)playerRpg.Exp / playerRpg.MaxExp;
        //Debug.Log(expPercentage);

        if ((expPercentage >= 0f) && (expPercentage <= 0.25f))
        {
            if(expBallState != 0)
            {
                expBallState = 0;
            }
            myUI.expOrb1.fillAmount = expPercentage / 0.25f;
            myUI.expOrb2.fillAmount = 0f;
            myUI.expOrb3.fillAmount = 0f;
            myUI.expOrb4.fillAmount = 0f;
        }
        else if ((expPercentage > 0.25f) && (expPercentage <= 0.5f))
        {
            if (expBallState != 1)
            {
                expBallState = 1;
                playerRpg.AddStatusPoint(0);
            }
            myUI.expOrb1.fillAmount = 1f;
            myUI.expOrb2.fillAmount = (expPercentage - 0.25f) / 0.25f;
            myUI.expOrb3.fillAmount = 0f;
            myUI.expOrb4.fillAmount = 0f;
        }
        else if ((expPercentage > 0.5f) && (expPercentage <= 0.75f))
        {
            if (expBallState != 2)
            {
                if(expBallState == 0)
                {
                    playerRpg.AddStatusPoint(0);
                }
                expBallState = 2;
                playerRpg.AddStatusPoint(1);
            }
            myUI.expOrb1.fillAmount = 1f;
            myUI.expOrb2.fillAmount = 1f;
            myUI.expOrb3.fillAmount = (expPercentage - 0.5f) / 0.25f;
            myUI.expOrb4.fillAmount = 0f;
        }
        else if ((expPercentage > 0.75f) && (expPercentage < 1f))
        {
            if (expBallState != 3)
            {
                if (expBallState == 1)
                {
                    playerRpg.AddStatusPoint(1);
                }
                expBallState = 3;
                playerRpg.AddStatusPoint(2);
            }
            myUI.expOrb1.fillAmount = 1f;
            myUI.expOrb2.fillAmount = 1f;
            myUI.expOrb3.fillAmount = 1f;
            myUI.expOrb4.fillAmount = (expPercentage - 0.75f) / 0.25f;
        }
    }

    public void UpdatePlayerStats()
    {
        if(((Weapon)myUI.WeaponSlot.item) != null)
        {
            playerRpg.EqAttackLower = ((Weapon)myUI.WeaponSlot.item).GetAttackLower(myUI.WeaponSlot.itemUI.UpgradeLevel);
            playerRpg.EqAttackHigher = ((Weapon)myUI.WeaponSlot.item).GetAttackHigher(myUI.WeaponSlot.itemUI.UpgradeLevel); ;
        }
        else
        {
            playerRpg.EqAttackLower = 0;
            playerRpg.EqAttackHigher = 0;
        }
        playerRpg.AttackLower = playerRpg.BaseAttackLower + playerRpg.EqAttackLower;
        playerRpg.AttackHigher = playerRpg.BaseAttackHigher + playerRpg.EqAttackHigher;

        if (((Armor)myUI.ArmorSlot.item) != null)
        {
            playerRpg.EqArmorDefense = ((Armor)myUI.ArmorSlot.item).GetDefense(myUI.ArmorSlot.itemUI.UpgradeLevel);
        }
        else
            playerRpg.EqArmorDefense = 0;

        if (((Helmet)myUI.HelmetSlot.item) != null)
            playerRpg.EqHelmetDefense = ((Helmet)myUI.HelmetSlot.item).GetDefense(myUI.HelmetSlot.itemUI.UpgradeLevel);
        else
            playerRpg.EqHelmetDefense = 0;

        if (((Shield)myUI.ShieldSlot.item) != null)
            playerRpg.EqShieldDefense = ((Shield)myUI.ShieldSlot.item).GetDefense(myUI.ShieldSlot.itemUI.UpgradeLevel);
        else
            playerRpg.EqShieldDefense = 0;

        playerRpg.EqDefense = playerRpg.EqArmorDefense + playerRpg.EqHelmetDefense + playerRpg.EqShieldDefense;
        playerRpg.Defense = playerRpg.BaseDefense + playerRpg.EqDefense;
    }
}
