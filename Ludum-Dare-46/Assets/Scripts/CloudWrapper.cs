using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CloudWrapper : MonoBehaviour
{
    private Bounds _bounds;

    private void Awake()
    {
        _bounds = GetComponent<SpriteRenderer>().bounds;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        var cloudTransform = collider.transform;
        
        if (cloudTransform.position.x > _bounds.max.x)
        {
            cloudTransform.position = new Vector3(_bounds.min.x, cloudTransform.position.y, cloudTransform.position.z);
        }

        if (cloudTransform.position.x < _bounds.min.x)
        {
            cloudTransform.position = new Vector3(_bounds.max.x, cloudTransform.position.y, cloudTransform.position.z);
        }

        Destroy(collider.gameObject);
    }
}
