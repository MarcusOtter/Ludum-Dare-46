using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private float _timeUntilDestruction;

    private void Awake()
    {
        Destroy(gameObject, _timeUntilDestruction);
    }
}
