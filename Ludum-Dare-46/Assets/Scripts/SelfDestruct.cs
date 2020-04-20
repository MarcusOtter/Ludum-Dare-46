using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private float _timeUntilDestroyed = 2f;

    private void Awake()
    {
        Destroy(gameObject, _timeUntilDestroyed);
    }
}
