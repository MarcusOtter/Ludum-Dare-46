using UnityEngine;

public class PlantsPlanter : MonoBehaviour
{
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private KeyCode _plantKey = KeyCode.Space;

    private void Update()
    {
        if (Input.GetKeyDown(_plantKey))
        {
            var slot = _playerInventory.CurrentSelectedSlot;

            if (slot == null) { return; }
            if (slot.Amount <= 0) { return; }

            var seed = slot.Seed;

            _playerInventory.PickUp(seed, -1);
            Instantiate(seed.PlantToGrowPrefab, transform.position, Quaternion.identity);
        }
    }
}
