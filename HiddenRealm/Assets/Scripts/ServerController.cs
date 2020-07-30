using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerController : NetworkBehaviour
{
    [SerializeField]
    private UIHandler myUI = null;

    [SerializeField]
    private NetworkManager networkManager;


    // Start is called before the first frame update
    void Start()
    {
        if(isServerOnly)
        {
            Destroy(myUI.mapGO);
            Destroy(myUI.MinimapGo);
            Destroy(myUI.MinimapCamera);
            Destroy(myUI.BigmapCamera);
            Destroy(myUI.MainCamera);
            StartCoroutine(SetupMobsCor());
        }
    }

    private void SetupMobs()
    {
        GameObject mobContainer = GameObject.Find("MobContainer");
        for(int i = 0; i < mobContainer.transform.childCount; i++)
        {
            mobContainer.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
            mobContainer.transform.GetChild(i).GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private IEnumerator SetupMobsCor()
    {
        GameObject mobContainer = GameObject.Find("MobContainer");
        while (mobContainer.transform.childCount == 0)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        SetupMobs();
    }
}
