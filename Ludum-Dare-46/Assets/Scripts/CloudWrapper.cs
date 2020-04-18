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
        if (collider.GetComponent<Cloud>() == null) { return; }

        var cloudTransform = collider.transform;
        
        if (cloudTransform.position.x > _bounds.max.x)
        {
            cloudTransform.position = cloudTransform.position.With(x: _bounds.min.x);
        }

        if (cloudTransform.position.x < _bounds.min.x)
        {
            cloudTransform.position = cloudTransform.position.With(x: _bounds.max.x);
        }

        if (cloudTransform.position.y < _bounds.min.y)
        {
            cloudTransform.position = cloudTransform.position.With(y: _bounds.max.y);
        }

        if (cloudTransform.position.y > _bounds.max.y)
        {
            cloudTransform.position = cloudTransform.position.With(y: _bounds.min.y);
        }
    }
}
