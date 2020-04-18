using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Random = UnityEngine.Random;
public class UpgradeManager : MonoBehaviour
{
    private List<Upgrade> _randomUpgrades = new List<Upgrade>();
    private List<Upgrade> _scheduledUpgrades = new List<Upgrade>();

    [SerializeField] private Upgrade[] _allUpgrades = new Upgrade[0];

    internal static event Action<Upgrade> OnLevelUp;

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
    /// Returns random upgrade from the available upgrades list and removes it from the list afterwards
    /// </summary>
    private Upgrade GetUpgrade(int level) 
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

    /// <summary>
    /// Invokes the upgrade event with a random upgrade
    /// </summary>
    public void LevelUp(int level)
    {
        Upgrade upgrade = GetUpgrade(level);
        OnLevelUp?.Invoke(upgrade);
    }
}
