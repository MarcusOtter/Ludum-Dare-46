using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    internal float CurrentWater { get; private set; }

    [SerializeField] private float _maxWaterCapacity;
    [SerializeField] private float _startingWaterAmount;
    [SerializeField] private InventoryItem _inventoryBoxPrefab;
    [SerializeField] private RectTransform _sidebar;
    [SerializeField] private Image _waterBarImage;


    private int ReturnSlotContainingSeed(Seed seed)
    {
        for(int i = 0; i < _inventorySlots.Count; i++)
        {
            if(seed.PlantToGrowPrefab.PlantType == _inventorySlots[i]._seed.PlantToGrowPrefab.PlantType)
            {
                return i;
            }
        }
        return -1;
    }

    private void Update()
    {
        for(int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                TryEquip(i);
        }

        if(Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            if (Time.time < lastScroll + scrollinterval) return;

            lastScroll = Time.time;

    private uint _slotIndex;

    private const float _scrollInterval = 0.05f;
    private float _lastScrollTime;

    private void Awake()
    {
        CurrentWater = _startingWaterAmount;
        _waterBarImage.fillAmount = CurrentWater / _maxWaterCapacity;
    }

    public void Equip(int i)
    {
        _slotIndex = (uint) i;

        _currentSelectedSlot?._item.SetSelected(false);
 
        _currentSelectedSlot = _inventorySlots[i];
        _currentSelectedSlot._item.SetSelected(true);
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
            if (item == _inventorySlots[i]._item)
            {
                Equip(i);
            }
        }
    }

    public void Equip(InventoryItem item)
    {
        _currentSelectedSlot?._item.SetSelected(false);

        for(int i = 0; i < _inventorySlots.Count; i++)
        {
            if(item == _inventorySlots[i]._item)
            {
                _currentSelectedSlot = _inventorySlots[i];
                _currentSelectedSlot._item.SetSelected(true);
                return;            
            }
        }
    }

    internal void ModifyWaterAmount(float waterDelta)
    {
        CurrentWater += waterDelta;
        CurrentWater = Mathf.Clamp(CurrentWater, 0f, _maxWaterCapacity);
        _waterBarImage.fillAmount = CurrentWater / _maxWaterCapacity;
    }

    internal void PickUp(Seed seed, int amount = 1)
    {
        int slotIndex = ReturnSlotContainingSeed(seed);
        if (slotIndex > -1)
        {
            _inventorySlots[slotIndex]._amount += amount;
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
        for(int i = 0; i < _inventorySlots.Count; i++)
        {
            if(seed.PlantToGrowPrefab.PlantType == _inventorySlots[i]._seed.PlantToGrowPrefab.PlantType)
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
        
        InventorySlot slot = new InventorySlot();
        slot._seed = seed;
        slot._item = item;
        slot._amount = amount;


        //you saw nothing
        slot._item.transform.GetChild(0).GetComponent<InventoryTextBox>().SetName(seed.PlantToGrowPrefab.name);
        slot._item.transform.GetChild(0).GetComponent<InventoryTextBox>().SetDescription(seed.PlantToGrowPrefab.Description);
        slot._item.SetUISprite(seed.PlantToGrowPrefab.GetComponent<SpriteRenderer>().sprite);

        _inventorySlots.Add(slot);

        if (_inventorySlots.Count == 1) { Equip(0); }
    }

    private void UpdateSeedNumber(int index)
    {
        _inventorySlots[index]._item.SetCounter(_inventorySlots[index]._amount);
    }

    [System.Serializable]
    public class InventorySlot
    {
        public InventoryItem _item;
        public Seed _seed;
        public int _amount;
    }
}
