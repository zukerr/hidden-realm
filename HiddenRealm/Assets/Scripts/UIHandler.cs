using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHandler : MonoBehaviour
{
    public PlayerUI playerUI;

    public GameObject StatsGO;
    public GameObject ResScreenGO;
    public GameObject mapGO;
    public GameObject EscMenuGO;
    public GameObject LoginScreenGO;
    public GameObject ChatGO;

    public GameObject MinimapGo;
    public GameObject MinimapCamera;
    public GameObject BigmapCamera;
    public GameObject MainCamera;

    public Image expOrb1;
    public Image expOrb2;
    public Image expOrb3;
    public Image expOrb4;

    public Image healthImg;
    public Image energyImg;
    public Image staminaImg;

    public Inventory inventory;

    public EqPieceSlot WeaponSlot;
    public EqPieceSlot ArmorSlot;
    public EqPieceSlot HelmetSlot;
    public EqPieceSlot ShieldSlot;
    public EqPieceSlot BootsSlot;
    public EqPieceSlot EarringsSlot;
    public EqPieceSlot NecklaceSlot;
    public EqPieceSlot BraceletSlot;

    public TextMeshProUGUI YangText;

    public TextMeshProUGUI StatsLevel;
    public TextMeshProUGUI StatsExp;
    public TextMeshProUGUI StatsExpToLevel;

    public TextMeshProUGUI StatsVit;
    public TextMeshProUGUI StatsInt;
    public TextMeshProUGUI StatsStr;
    public TextMeshProUGUI StatsDex;

    public GameObject AddVitButton;
    public GameObject AddIntButton;
    public GameObject AddStrButton;
    public GameObject AddDexButton;

    public TextMeshProUGUI StatsHpText;
    public TextMeshProUGUI StatsEpText;
    public TextMeshProUGUI StatsAttackText;
    public TextMeshProUGUI StatsDefenseText;

    public GameObject InfoBox;
    public TextMeshProUGUI InfoBoxText;

    public GameObject ThrowConfirmationBox;

    private NpcWindowController activeNpcWindow = null;

    // Update is called once per frame
    void Update()
    {
        if(SerializationDatabase.instance.loadedIn)
        {
            if(ChatGO.GetComponent<ChatHandler>().mode == 0)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    playerUI.Loot();
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
                    SwitchInventory();
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    SwitchStats();
                }
                if (Input.GetKeyDown(KeyCode.M))
                {
                    SwitchMap();
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //enable esc menu
                SwitchEscMenu();
            }
            if(Input.GetKeyDown(KeyCode.Return))
            {
                if(ChatGO.GetComponent<ChatHandler>().mode == 0)
                {
                    ChatGO.GetComponent<ChatHandler>().TypingMode();
                }
                else
                {
                    ChatGO.GetComponent<ChatHandler>().ConfirmTypedMsg();
                }
            }
        }
    }

    private void SwitchGameobject(GameObject go)
    {
        if (go.activeSelf)
        {
            go.SetActive(false);
        }
        else
        {
            go.SetActive(true);
        }
    }

    public void SwitchInventory()
    {
        SwitchGameobject(inventory.gameObject);
    }

    public void SwitchStats()
    {
        SwitchGameobject(StatsGO);
    }

    public void SwitchMap()
    {
        SwitchGameobject(mapGO);
    }

    public void SwitchEscMenu()
    {
        SwitchGameobject(EscMenuGO);
    }

    public void AddStatButtonsSetActive(bool value)
    {
        AddVitButton.SetActive(value);
        AddIntButton.SetActive(value);
        AddStrButton.SetActive(value);
        AddDexButton.SetActive(value);
    }

    public void AddVitOnClick()
    {
        playerUI.gameObject.GetComponent<PlayerRpg>().IncreaseBaseVit();
        playerUI.gameObject.GetComponent<PlayerRpg>().RemoveStatusPoint();
        if(playerUI.gameObject.GetComponent<PlayerRpg>().StatusPoints == 0)
        {
            AddStatButtonsSetActive(false);
        }
    }
    public void AddIntOnClick()
    {
        playerUI.gameObject.GetComponent<PlayerRpg>().IncreaseBaseInt();
        playerUI.gameObject.GetComponent<PlayerRpg>().RemoveStatusPoint();
        if (playerUI.gameObject.GetComponent<PlayerRpg>().StatusPoints == 0)
        {
            AddStatButtonsSetActive(false);
        }
    }
    public void AddStrOnClick()
    {
        playerUI.gameObject.GetComponent<PlayerRpg>().IncreaseBaseStr();
        playerUI.gameObject.GetComponent<PlayerRpg>().RemoveStatusPoint();
        if (playerUI.gameObject.GetComponent<PlayerRpg>().StatusPoints == 0)
        {
            AddStatButtonsSetActive(false);
        }
    }
    public void AddDexOnClick()
    {
        playerUI.gameObject.GetComponent<PlayerRpg>().IncreaseBaseDex();
        playerUI.gameObject.GetComponent<PlayerRpg>().RemoveStatusPoint();
        if (playerUI.gameObject.GetComponent<PlayerRpg>().StatusPoints == 0)
        {
            AddStatButtonsSetActive(false);
        }
    }

    public void ResHereOnClick()
    {
        playerUI.gameObject.GetComponent<PlayerDeath>().ResurrectHere();
    }
    public void ResInTownOnClick()
    {
        playerUI.gameObject.GetComponent<PlayerDeath>().ResurrectInTown();
    }

    public void ShowInfoBox(string info)
    {
        InfoBoxText.text = info;
        InfoBox.SetActive(true);
    }

    public void SetActiveNpcWindow(NpcWindowController window)
    {
        if(activeNpcWindow != null)
        {
            activeNpcWindow.CloseWindow();
            ClearActiveNpcWindow();
        }
        activeNpcWindow = window;
    }
    public void ClearActiveNpcWindow()
    {
        activeNpcWindow = null;
    }

    public void TestButtonFunc()
    {
        Debug.Log("Test button clicked.");
    }

    public void SetActiveThrowItemConfirmationBox()
    {
        ThrowConfirmationBox.SetActive(true);
    }

    public EqPieceSlot GetEqPieceSlotByEqPiece(EqPiece eqpiece)
    {
        switch(eqpiece)
        {
            case EqPiece.Weapon:
                return WeaponSlot;
            case EqPiece.Armor:
                return ArmorSlot;
            case EqPiece.Helmet:
                return HelmetSlot;
            case EqPiece.Shield:
                return ShieldSlot;
            case EqPiece.Boots:
                return BootsSlot;
            case EqPiece.Earrings:
                return EarringsSlot;
            case EqPiece.Necklace:
                return NecklaceSlot;
            case EqPiece.Bracelet:
                return BraceletSlot;
        }
        return null;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Logout()
    {
        StartCoroutine(BackupAndLogout());
    }

    private IEnumerator BackupAndLogout()
    {
        SerializationDatabase.instance.SaveItemsToDatabase();
        ShowInfoBox("You'll be logged out in 2 seconds...");
        yield return new WaitForSeconds(2);
        LoginScreenGO.SetActive(true);
        LoginScreenGO.GetComponent<LoginScreen>().AfterLogoutSetup();
    }
}
