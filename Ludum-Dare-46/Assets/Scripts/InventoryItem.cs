using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;


public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color outlineHoverColor;

    internal bool _selected;

    public UnityEvent OnHover;
    public UnityEvent OnStopHover;
    private Outline outline;

    private PlayerInventory inventory;

    public int index;
    private Button button;
    private float maxScale = 1.2f;
    internal readonly static float easeTime = 0.2f;

    public static bool hoveringBox;

    private RectTransform textBox;

    internal Seed selsctedSeed;

    private void Awake()
    {

        button = GetComponent<Button>();
        outline = GetComponent<Outline>();
        inventory = GetComponentInParent<PlayerInventory>();
        if (inventory == null) print("null inventory");
        button.onClick.AddListener(EquipThis);

        transform.localScale = Vector2.right;

        LeanTween.scaleY(gameObject, 1f, easeTime);

        //outline.effectColor = Color.black;
    }

    public void SetSelected(bool selected)
    {
        _selected = selected;
        outline.effectColor = selected ? outlineHoverColor : Color.black;

        LeanTween.cancel(gameObject);

        if(selected)
        {
            Vector2 selectionScale = new Vector2(1.1f, 1.1f);
            transform.localScale = selectionScale;
            LeanTween.scale(gameObject, new Vector2(1, 1), easeTime).setEase(LeanTweenType.easeOutSine);
        }
    }

    public void SetCounter(int counter)
    {
        transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().SetText(counter.ToString());
        LeanTween.cancel(gameObject);
        transform.localScale = new Vector2(1, 1) * 1.1f;
        LeanTween.scale(gameObject, new Vector2(1,1), easeTime).setEase(LeanTweenType.easeOutSine);
    }

    public void SetUISprite(Sprite sprite)
    {
        transform.GetChild(1).GetComponent<Image>().sprite = sprite;
    }

    public void EquipThis()
    {
        inventory.Equip(this);
    }

    private void ScaleButton(bool scaleUp)
    {
        Vector2 newScale = scaleUp ? new Vector2(maxScale, maxScale) : new Vector2(1f,1f);
        LeanTween.scale(gameObject, newScale, easeTime).setEase(LeanTweenType.easeOutSine); ;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        hoveringBox = true;

        OnHover.Invoke();
        ScaleButton(true);
        LeanTween.value(gameObject, outline.effectColor, outlineHoverColor, easeTime).setOnUpdate(SetOutlineColor).setEase(LeanTweenType.easeOutSine); ;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoveringBox = false;

        OnStopHover.Invoke();
        ScaleButton(false);
        if (!_selected) LeanTween.value(gameObject, outline.effectColor, Color.black, easeTime).setOnUpdate(SetOutlineColor).setEase(LeanTweenType.easeOutSine); ;
    }

    private void SetOutlineColor(Color c)
    {
        outline.effectColor = c;
    }
}
