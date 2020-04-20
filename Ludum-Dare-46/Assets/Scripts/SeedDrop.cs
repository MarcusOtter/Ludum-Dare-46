using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedDrop : MonoBehaviour
{
    [SerializeField] private Seed[] _seedDrops;

    public void TryDropSeed()
    {
        for(int i = 0; i < _seedDrops.Length; i++)
        {
            int r = Random.Range(0, 100);
            if(r <= _seedDrops[i].Probability)
            {
                Instantiate(_seedDrops[i], transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0f), Quaternion.Euler(0,0,Random.Range(0f, 360f)));
            }
        }
    }
}
