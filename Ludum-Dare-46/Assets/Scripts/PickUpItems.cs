using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    private PlayerInventory _inventory;

    private void Start()
    {
        _inventory = FindObjectOfType<PlayerInventory>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var seed = collider.transform.GetComponent<Seed>();
        if (seed != null)
        {
            _inventory.PickUp(seed, 1);
            Destroy(seed.gameObject);
        }
    }
}
