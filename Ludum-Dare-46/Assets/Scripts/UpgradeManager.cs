using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Random = UnityEngine.Random;
public class UpgradeManager : MonoBehaviour
{
    private List<Upgrade> _availableUpgrades = new List<Upgrade>();
    [SerializeField] private Upgrade[] _allUpgrades = new Upgrade[0];

    internal static event Action<Upgrade> OnLevelUp;

    private void Start()
    {
        PopulateAvailableUpgrades();
    }
    private void PopulateAvailableUpgrades()
    {
        for(int i = 0; i < _allUpgrades.Length; i++)
        {
            _availableUpgrades.Add(_allUpgrades[i]);
        }
    }




    /// <summary>
    /// Returns random upgrade from the available upgrades list and removes it from the list afterwards
    /// </summary>
    private Upgrade GetRandomUpgrade() 
    {
        int r = Random.Range(0, _availableUpgrades.Count);
        Upgrade upgrade = _availableUpgrades[r];
        _availableUpgrades.RemoveAt(r);

        if (_availableUpgrades.Count == 0) PopulateAvailableUpgrades();

        return upgrade;
    }


    /// <summary>
    /// Invokes the upgrade event with a random upgrade
    /// </summary>
    public void LevelUp()
    {
        Upgrade upgrade = GetRandomUpgrade();
        OnLevelUp?.Invoke(upgrade);
    }
}
