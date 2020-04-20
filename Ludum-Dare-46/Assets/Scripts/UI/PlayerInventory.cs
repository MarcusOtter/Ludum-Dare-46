using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    internal float CurrentWater { get; private set; }
    internal bool WaterIsFull => CurrentWater == _maxWaterAmount;
    internal InventorySlot CurrentSelectedSlot { get; private set; }

    [SerializeField] private float _maxWaterAmount;
    [SerializeField] private float _startingWaterAmount;
    [SerializeField] private InventoryItem _inventoryBoxPrefab;
    [SerializeField] private RectTransform _sidebar;
    [SerializeField] private Image _waterBarImage;

    private List<InventorySlot> _inventorySlots = new List<InventorySlot>();

    private int _slotIndex;

    private const float _scrollInterval = 0.05f;
    private float _lastScrollTime;

    private void Awake()
    {
        CurrentWater = _startingWaterAmount;
        _waterBarImage.fillAmount = CurrentWater / _maxWaterAmount;
    }

    public void Equip(int i)
    {
        _slotIndex = i;

        CurrentSelectedSlot?.Item.SetSelected(false);
 
        CurrentSelectedSlot = _inventorySlots[i];
        CurrentSelectedSlot.Item.SetSelected(true);
        return;
    }

    public void TryEquip(int i)
    {
        if (_inventorySlots.Count < 1) return;

        if (i < _inventorySlots.Count)
        {
            Equip(i);
        }
    }

    public void TryEquip(InventoryItem item)
    {
        for (int i = 0; i < _inventorySlots.Count; i++)
        {
            if (item == _inventorySlots[i].Item)
            {
                Equip(i);
            }
        }
    }

    public void Equip(InventoryItem item)
    {
        CurrentSelectedSlot?.Item.SetSelected(false);

        for(int i = 0; i < _inventorySlots.Count; i++)
        {
            if(item == _inventorySlots[i].Item)
            {
                CurrentSelectedSlot = _inventorySlots[i];
                CurrentSelectedSlot.Item.SetSelected(true);
                return;            
            }
        }
    }

    internal void ModifyWaterAmount(float waterDelta)
    {
        CurrentWater += waterDelta;
        CurrentWater = Mathf.Clamp(CurrentWater, 0f, _maxWaterAmount);
        _waterBarImage.fillAmount = CurrentWater / _maxWaterAmount;
    }

    internal void PickUp(Seed seed, int amount = 1)
    {
        int slotIndex = ReturnSlotContainingSeed(seed);
        if (slotIndex > -1)
        {
            _inventorySlots[slotIndex].Amount += amount;
            UpdateSeedNumber(slotIndex);
        }
        else
        {
            AddPanel(seed, amount);
            UpdateSeedNumber(_inventorySlots.Count - 1);
        }
    }

    private void Update()
    {
        for(int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                TryEquip(i);
            }
        }

        if(Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            if (Time.time < _lastScrollTime + _scrollInterval) { return; }

            _lastScrollTime = Time.time;

            int sign = -(int) Mathf.Sign(Input.GetAxisRaw("Mouse ScrollWheel"));
            if (_inventorySlots.Count == 0) { return; }

            int index = (int) _slotIndex;

            index += sign;
            if (index < 0) index = _inventorySlots.Count - 1;
            index %= _inventorySlots.Count;
            TryEquip(index);
        }
    }

    private int ReturnSlotContainingSeed(Seed seed)
    {
        for (int i = 0; i < _inventorySlots.Count; i++)
        {
            if (seed.PlantToGrowPrefab.PlantType == _inventorySlots[i].Seed.PlantToGrowPrefab.PlantType)
            {
                return i;
            }
        }
        return -1;
    }

    private void AddPanel(Seed seed, int amount)
    {
        var item = Instantiate(_inventoryBoxPrefab, _sidebar);
        item.index = _inventorySlots.Count;

        InventorySlot slot = new InventorySlot
        {
            Seed = seed,
            Item = item,
            Amount = amount
        };

        var textBox = slot.Item.transform.GetComponentInChildren<InventoryTextBox>();
        textBox.SetName(seed.PlantToGrowPrefab.name);
        textBox.SetDescription(seed.PlantToGrowPrefab.Description);

        slot.Item.SetUISprite(seed.PlantToGrowPrefab.GetFullyGrownSprite());

        _inventorySlots.Add(slot);

        if (_inventorySlots.Count == 1) { Equip(0); }
    }

    private void UpdateSeedNumber(int index)
    {
        _inventorySlots[index].Item.SetCounter(_inventorySlots[index].Amount);
    }

    [System.Serializable]
    public class InventorySlot
    {
        public InventoryItem Item;
        public Seed Seed;
        public int Amount;
    }
}
