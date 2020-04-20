using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Animator))]
public class CloudSucker : MonoBehaviour
{
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private float _waterConsumptionPerSecond;
    [SerializeField] private string _suckingBoolName;

    private List<Cloud> _cloudsBeingSucked = new List<Cloud>();
    private Animator _animator;
    private int _suckingBoolHash;

    private void Awake()
    {
        _suckingBoolHash = Animator.StringToHash(_suckingBoolName);
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool(_suckingBoolHash, false);
        if (_playerInventory == null) { return; }
        if (_playerInventory.WaterIsFull) { return; }

        foreach(var cloud in _cloudsBeingSucked)
        {
            _animator.SetBool(_suckingBoolHash, true);
            cloud.ModifyWater(-_waterConsumptionPerSecond * Time.deltaTime);
            _playerInventory.ModifyWaterAmount(_waterConsumptionPerSecond * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.parent == null) { return; }
        if (!collider.transform.parent.TryGetComponent<Cloud>(out var cloud)) { return; }
        _cloudsBeingSucked.AddIfNotContains(cloud);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.transform.parent == null) { return; }
        if (!collider.transform.parent.TryGetComponent<Cloud>(out var cloud)) { return; }
        _cloudsBeingSucked.RemoveIfContains(cloud);
    }
}
