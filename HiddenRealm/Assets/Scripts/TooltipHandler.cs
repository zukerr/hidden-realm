using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipHandler : MonoBehaviour
{
    public static TooltipHandler instance;

    [SerializeField]
    private GameObject tooltip = null;

    private TextMeshProUGUI tooltipText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        tooltipText = tooltip.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void Move(Vector2 newPosition)
    {
        tooltip.transform.position = newPosition;
    }

    public void SetText(string newText)
    {
        tooltipText.text = newText;
    }

    public void ShowTooltip()
    {
        tooltip.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
