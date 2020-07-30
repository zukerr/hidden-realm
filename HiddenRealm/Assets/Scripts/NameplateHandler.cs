using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameplateHandler : MonoBehaviour
{
    [SerializeField]
    private Transform mob = null;

    [SerializeField]
    private TextMeshProUGUI nameField = null;

    // Start is called before the first frame update
    void Start()
    {
        nameField.text = mob.GetComponent<MobBehaviour>().GetData().mobName;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = mob.position;
    }
}
