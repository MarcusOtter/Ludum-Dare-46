using UnityEngine;

public class CloudDestroyer : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collider)
    {
        Destroy(collider.gameObject);
    }
}
