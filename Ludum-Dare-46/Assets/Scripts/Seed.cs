using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Seed : MonoBehaviour
{
    public Plant PlantToGrowPrefab;
    [Range(0, 100)] public int Probability;
}
