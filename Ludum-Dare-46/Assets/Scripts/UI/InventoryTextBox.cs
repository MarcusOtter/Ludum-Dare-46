using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTextBox : MonoBehaviour
{
    private InventoryItem item;

    private void Awake()
    {
        transform.localScale = Vector2.up;
        item = GetComponentInParent<InventoryItem>();
        if (item == null) { print("Return"); return;}

        item.OnHover.AddListener(Show);
        item.OnStopHover.AddListener(Hide);
    }

    public void Show()
    {
        print(InventoryItem.easeTime);
        LeanTween.scaleX(gameObject, 1f, InventoryItem.easeTime);
    }
    public void Hide()
    {
        LeanTween.scaleX(gameObject, 0f, InventoryItem.easeTime);
    }


}
