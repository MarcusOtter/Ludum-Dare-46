using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private Upgrade[] _allUpgrades;

    private readonly List<Upgrade> _randomUpgrades = new List<Upgrade>();
    private readonly List<Upgrade> _scheduledUpgrades = new List<Upgrade>();

    private void Start()
    {
        PopulateRandomUpgrades();
        PopulateScheduledUpgrades();
    }

    private void PopulateScheduledUpgrades()
    {
        for (int i = 0; i < _allUpgrades.Length; i++)
        {
            if (_allUpgrades[i].aquiredOnLevel >= 1)
            {
                _scheduledUpgrades.Add(_allUpgrades[i]);
            }
        }
    }

    private void PopulateRandomUpgrades()
    {
        for(int i = 0; i < _allUpgrades.Length; i++)
        {
            if(_allUpgrades[i].aquiredOnLevel < 1)
            {
                _randomUpgrades.Add(_allUpgrades[i]);
            }
        }
    }

    /// <summary>
    /// Returns random upgrade from the available upgrades list and removes it from the list afterwards.
    /// </summary>
    internal Upgrade GetUpgrade(int level) 
    {
        Upgrade upgrade;

        for(int i = 0; i < _scheduledUpgrades.Count; i++)
        {
            if(level == _scheduledUpgrades[i].aquiredOnLevel)
            {
                upgrade = _scheduledUpgrades[i];
                _scheduledUpgrades.RemoveAt(i);
                return upgrade;
            }
        }
        
        int index = Random.Range(0, _randomUpgrades.Count);
        upgrade = _randomUpgrades[index];
        _randomUpgrades.RemoveAt(index);

        if (_randomUpgrades.Count == 0) PopulateRandomUpgrades();
        
        return upgrade;
    }
}
