using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InventoryTextBox : MonoBehaviour
{
    private InventoryItem item;
    public void SetName(string name)
    {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
    }

    public void SetDescription(string description)
    {
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = description;
    }

    private void Awake()
    {
        transform.localScale = Vector2.up;
        item = GetComponentInParent<InventoryItem>();
        if (item == null) { print("Return"); return; }

        item.OnHover.AddListener(Show);
        item.OnStopHover.AddListener(Hide);
    }

    public void Show()
    {
        LeanTween.scaleX(gameObject, 1f, InventoryItem.easeTime);
    }
    public void Hide()
    {
        LeanTween.scaleX(gameObject, 0f, InventoryItem.easeTime);
    }


}
