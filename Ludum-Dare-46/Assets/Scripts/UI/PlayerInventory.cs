using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    private InventorySlot currentSelectedSlot;

    private uint slotindex;

    private float lastScroll = 0f;
    private float scrollinterval = 0.05f;

    [SerializeField] internal float totalWaterCapacity, currentWater, startWater;

    private List<InventorySlot> _inventorySlots = new List<InventorySlot>();
    [SerializeField] private InventoryItem _inventoryBoxPrefab;
    [SerializeField] private RectTransform _sidebar;

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

            int sign = -(int)Mathf.Sign(Input.GetAxisRaw("Mouse ScrollWheel"));
            print(sign);
            if (_inventorySlots.Count == 0) return;

            int index = (int)slotindex;

            index += sign;
            if (index < 0) index = _inventorySlots.Count - 1;
            index %= _inventorySlots.Count;
            TryEquip(index);
        }
    }

    public void Equip(int i)
    {
        slotindex = (uint)i;

        currentSelectedSlot?._item.SetSelected(false);
 
        currentSelectedSlot = _inventorySlots[i];
        currentSelectedSlot._item.SetSelected(true);
        print($"Equip {_inventorySlots[i]._seed.PlantToGrowPrefab.PlantType}");
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
        currentSelectedSlot?._item.SetSelected(false);

        for(int i = 0; i < _inventorySlots.Count; i++)
        {
            if(item == _inventorySlots[i]._item)
            {
                currentSelectedSlot = _inventorySlots[i];
                currentSelectedSlot._item.SetSelected(true);
                print($"Equip {_inventorySlots[i]._seed.PlantToGrowPrefab.PlantType}");
                return;            
            }
        }
        //print("Slot is not in the inventory, fix it");
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

        if (_inventorySlots.Count == 1) Equip(0);

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
