using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    private PlayerInventory inventory;
    private void Start()
    {
        inventory = FindObjectOfType<PlayerInventory>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var seed = collision.transform.GetComponent<Seed>();
        if (seed != null)
        {
            print($"Picked up{seed.PlantToGrowPrefab.PlantType}");
            inventory?.PickUp(seed, 1);
            Destroy(seed.gameObject);
        }
    }
}
