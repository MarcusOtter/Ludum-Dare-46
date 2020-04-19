using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    private InventorySlot currentSelectedSlot;

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

    public void Equip(InventoryItem item)
    {
        for(int i = 0; i < _inventorySlots.Count; i++)
        {
            if(item == _inventorySlots[i]._item)
            {
                currentSelectedSlot = _inventorySlots[i];
                print($"Equip {_inventorySlots[i]._seed.PlantToGrowPrefab.PlantType}");
                return;            
            }
        }
        print("Slot is not in the inventory, fix it");
    }

    internal void PickUp(Seed seed, int amount = 1)
    {
        int slotIndex = ReturnSlotContainingSeed(seed);
        if (slotIndex > -1)
        {
            _inventorySlots[slotIndex]._amount += amount;
        }
        else
        {
            AddPanel(seed, amount);
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

        _inventorySlots.Add(slot);

    }

    [System.Serializable]
    public class InventorySlot
    {
        public InventoryItem _item;
        public Seed _seed;
        public int _amount;
    }

}
