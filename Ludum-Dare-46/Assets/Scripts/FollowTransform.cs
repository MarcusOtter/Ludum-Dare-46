using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [SerializeField] private Transform _transformToFollow;

    private void Update()
    {
        transform.position = _transformToFollow.position;
    }
}
