using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WaterArea : MonoBehaviour
{
    [SerializeField] private float _waterPerSecond;

    private List<IWaterable> _waterablesInWaterArea = new List<IWaterable>();

    private void Update()
    {
        foreach(var waterable in _waterablesInWaterArea)
        {
            if (waterable == null)
            {
                _waterablesInWaterArea.Remove(waterable);
                continue;
            }

            waterable.Water(_waterPerSecond * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var waterable = collider.GetComponent<IWaterable>();
        if (waterable == null) { return; }

        _waterablesInWaterArea.Add(waterable);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        var waterable = collider.GetComponent<IWaterable>();
        if (waterable == null) { return; }

        if (_waterablesInWaterArea.Contains(waterable))
        {
            _waterablesInWaterArea.Remove(waterable);
        }
    }
}
