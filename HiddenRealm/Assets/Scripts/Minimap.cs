using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public UIHandler myUI;
    public Transform minimapPlayerIndicator;
    public Transform bigmapPlayerIndicator;
    public GameObject npcIndicators;

    private bool flag = false;

    private void Start()
    {
        minimapPlayerIndicator.gameObject.SetActive(true);
        bigmapPlayerIndicator.gameObject.SetActive(true);
        npcIndicators.SetActive(true);
    }

    private void Update()
    {
        if(myUI.playerUI != null)
        {
            flag = true;
        }
    }

    private void LateUpdate()
    {
        if(flag)
        {
            if(myUI.playerUI == null)
            {
                return;
            }

            Vector3 newPos = myUI.playerUI.gameObject.transform.position;
            minimapPlayerIndicator.position = newPos;
            minimapPlayerIndicator.rotation = myUI.playerUI.gameObject.transform.rotation;
            bigmapPlayerIndicator.position = newPos;
            bigmapPlayerIndicator.rotation = myUI.playerUI.gameObject.transform.rotation;
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
    }
}
