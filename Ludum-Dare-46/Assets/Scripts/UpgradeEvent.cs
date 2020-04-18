using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeEvent : MonoBehaviour
{
    internal static event Action<Upgrade> CallItSomething;

    // Start is called before the first frame update
    void Start()
    {
        CallItSomething?.Invoke(new Upgrade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
