using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.SceneManagement;
using BCrypt.Net;
using BCrypt;

public class LoginScreen : MonoBehaviour
{
    public static bool loggedIn = false;

    private bool serverSelected = false;
    [SerializeField]
    private GameObject ServerPanel = null;
    [SerializeField]
    private GameObject LoginPanel = null;
    [SerializeField]
    private GameObject Server1ButtonFrame = null;
    [SerializeField]
    private NetworkManager netManager;
    [SerializeField]
    private SerializationDatabase serialDatabase = null;
    [SerializeField]
    private GameObject loginInputField = null;
    [SerializeField]
    private GameObject passwordInputField = null;
    [SerializeField]
    private GameObject charSelectScreen = null;
    [SerializeField]
    private GameObject noCharScreen = null;
    [SerializeField]
    private TextMeshProUGUI charSelectPlayerName = null;
    [SerializeField]
    private TMP_InputField noCharNewCharInputField = null;

    public static string login = "";
    private string password = "";
    private string plainPw = "";

    private readonly string pwSalt = "^SB%.A%@!";
    public static string pwSaltBuffer = "";
    public static string pwBuffer = "";

    public static bool loginSuccessful = true;
    public static bool loginComplete = false;
    public static bool noCharacter = false;
    public static bool newCharacterReady = false;
    public static bool isDeveloper = false;


    private void Awake()
    {
        netManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(LoginPanel.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (loginInputField.GetComponent<TMP_InputField>().isFocused)
                {
                    passwordInputField.GetComponent<TMP_InputField>().Select();
                }
                else if (passwordInputField.GetComponent<TMP_InputField>().isFocused)
                {
                    loginInputField.GetComponent<TMP_InputField>().Select();
                }
                else
                {
                    loginInputField.GetComponent<TMP_InputField>().Select();
                }
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                LogInButton();
            }
        }
        if(charSelectScreen.activeInHierarchy)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                EnterTheGameButton();
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                CharSelectCancelButton();
            }
        }
    }

    public void SelectServer()
    {
        serverSelected = true;
        Server1ButtonFrame.SetActive(true);
    }

    public void ProceedToLoginPanelButton()
    {
        if(serverSelected)
        {
            ServerPanel.SetActive(false);
            LoginPanel.SetActive(true);
            Server1ButtonFrame.SetActive(false);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoBackToServerPanelButton()
    {
        ServerPanel.SetActive(true);
        LoginPanel.SetActive(false);
        serverSelected = false;
    }

    public void LogInButton()
    {
        loggedIn = true;
        loginComplete = false;
        login = loginInputField.GetComponent<TMP_InputField>().text;
        password = passwordInputField.GetComponent<TMP_InputField>().text;
        plainPw = password;
        if(!NetworkClient.isConnected)
        {
            netManager.StartClient();
        }
        StartCoroutine(WaitForPlayer());
    }

    private IEnumerator WaitForPlayer()
    {
        while(GameObject.Find("LocalPlayer") == null)
        {
            //loading
            yield return null;
        }

        //setting up default values
        loginSuccessful = true;
        loginComplete = false;
        noCharacter = false;

        //Here we will ask server for our character id
        GameObject.Find("LocalPlayer").GetComponent<PlayerNetwork>().CallCmdRetrieveSalt(login);

        while (pwBuffer.Equals(""))
        {
            yield return null;
        }
        password = plainPw + pwSalt + pwSaltBuffer;
        if(BCrypt.Net.BCrypt.Verify(password, pwBuffer))
        {
            password = pwBuffer;
            pwBuffer = "";
        }
        GameObject.Find("LocalPlayer").GetComponent<PlayerNetwork>().CallCmdCheckPlayerName(login, password);

        while(!loginComplete)
        {
            yield return null;
        }

        Debug.Log(GameObject.Find("LocalPlayer").GetComponent<PlayerRpg>().playerName);

        if(loginSuccessful)
        {
            LoginPanel.SetActive(false);
            if(!noCharacter)
            {
                charSelectPlayerName.text = GameObject.Find("LocalPlayer").GetComponent<PlayerRpg>().playerName;
                charSelectScreen.SetActive(true);
                noCharScreen.SetActive(false);
            }
            else
            {
                noCharScreen.SetActive(true);
                charSelectScreen.SetActive(false);
            }
        }
        else
        {
            GameObject.Find("UI").GetComponent<UIHandler>().ShowInfoBox("Account doesn't exist.");
        }
    }

    public void EnterTheGameButton()
    {
        serialDatabase.InitLoadOnButton();
        charSelectScreen.SetActive(false);
        ServerPanel.SetActive(true);
        plainPw = "";
        gameObject.SetActive(false);
    }

    public void CharSelectCancelButton()
    {
        charSelectScreen.SetActive(false);
        LoginPanel.SetActive(true);
    }

    public void noCharCancelButton()
    {
        noCharScreen.SetActive(false);
        LoginPanel.SetActive(true);
    }

    public void CreateNewCharacterButton()
    {
        newCharacterReady = false;
        string characterName = noCharNewCharInputField.text;
        GameObject.Find("LocalPlayer").GetComponent<PlayerRpg>().playerName = characterName;
        GameObject.Find("LocalPlayer").GetComponent<PlayerNetwork>().CallCmdCreateNewCharacter(login, password, characterName);
        StartCoroutine(WaitForNewChar());
    }

    private IEnumerator WaitForNewChar()
    {
        while(!newCharacterReady)
        {
            yield return null;
        }
        loggedIn = true;
        loginComplete = false;
        serialDatabase.SaveItemsToDatabase();
        StartCoroutine(WaitForPlayer());
        noCharScreen.SetActive(false);
    }

    public void DeveloperButton()
    {
        login = "ZukerGM";
        plainPw = "passwd123";

        loggedIn = true;
        loginComplete = false;
        isDeveloper = true;
        netManager.StartHost();
        StartCoroutine(WaitForPlayer());
    }

    public void AfterLogoutSetup()
    {
        ServerPanel.SetActive(true);
        LoginPanel.SetActive(false);
        charSelectScreen.SetActive(false);
        noCharScreen.SetActive(false);
        if (!isDeveloper)
            netManager.StopClient();
        else
            netManager.StopHost();
        SceneManager.LoadScene("Main");
    }

    public void ServerButton()
    {
        netManager.StartServer();
        UIHandler myUI = GameObject.Find("UI").GetComponent<UIHandler>();
        Destroy(myUI.mapGO);
        Destroy(myUI.MinimapGo);
        Destroy(myUI.MinimapCamera);
        Destroy(myUI.BigmapCamera);
        Destroy(myUI.MainCamera);
    }
}
