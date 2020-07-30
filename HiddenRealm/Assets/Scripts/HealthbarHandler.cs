using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarHandler : MonoBehaviour
{
    [SerializeField]
    private Image image = null;
    [SerializeField]
    private MobRpg mob = null;
    [SerializeField]
    private MobBehaviour mobBehav = null;

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = mob.currentHp / mobBehav.GetData().maxHp;
    }
}
