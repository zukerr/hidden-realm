using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject grid = null;
    [SerializeField]
    private GameObject textSlotPrefab = null;
    [SerializeField]
    private GameObject inptFieldGO = null;
    [SerializeField]
    private UIHandler myUI = null;
    [SerializeField]
    private Scrollbar scrollBar = null;

    private float inptFieldHeight = 20f;
    private string playerName = "";

    //0 - passive
    //1 - typing
    public int mode = 0;

    // Start is called before the first frame update
    void Start()
    {
        ClearChat();
    }

    public void ClearChat()
    {
        for(int i = 0; i < grid.transform.childCount; i++)
        {
            grid.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    public string GetPlayerName()
    {
        if(playerName.Equals(""))
        {
            playerName = GameObject.Find("LocalPlayer").GetComponent<PlayerRpg>().playerName;
        }
        return playerName;
    }

    public void DisplayMessage(string msg)
    {
        GameObject entry = Instantiate(textSlotPrefab, grid.transform);
        entry.GetComponent<TextMeshProUGUI>().text = msg;
        Destroy(grid.transform.GetChild(0).gameObject);
        scrollBar.value = 0;
    }

    public void TypingMode()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + inptFieldHeight, transform.position.x);
        inptFieldGO.SetActive(true);
        inptFieldGO.GetComponent<TMP_InputField>().ActivateInputField();
        mode = 1;
    }

    public void PassiveMode()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - inptFieldHeight, transform.position.x);
        inptFieldGO.SetActive(false);
        mode = 0;
    }

    public void ConfirmTypedMsg()
    {
        string msg = inptFieldGO.GetComponent<TMP_InputField>().text;
        if(msg.Length > 0)
        {
            if (msg.Length <= 35)
            {
                DisplayMessage(GetPlayerName() + ": " + msg);
                GameObject.Find("LocalPlayer").GetComponent<PlayerNetwork>().InitiateChatMsgSync(msg, GetPlayerName());
                inptFieldGO.GetComponent<TMP_InputField>().text = "";
                inptFieldGO.GetComponent<TMP_InputField>().ActivateInputField();
            }
            else
            {
                myUI.ShowInfoBox("Message too long!");
            }
        }
        else
        {
            PassiveMode();
        }
    }
}
