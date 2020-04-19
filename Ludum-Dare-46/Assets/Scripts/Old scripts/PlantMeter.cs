using UnityEngine;
using UnityEngine.UI;

public class PlantMeter : MonoBehaviour
{
    private Image plantMeter;
    private PlayerAttributes attributes;

    private void Start()
    {
        plantMeter = GetComponent<Image>();
        attributes = FindObjectOfType<PlayerAttributes>();    
    }

    private void Update()
    {
        if (attributes != null)
            plantMeter.fillAmount = attributes.PlantMeterCurrent / attributes.PlantMeterMax;
        else
            plantMeter.fillAmount = 0.0f;
    }
}
