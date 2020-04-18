using UnityEngine;

public class SingletonSpawner : MonoBehaviour
{
    internal static SingletonSpawner Instance { get; private set; }

    [SerializeField] private MonoBehaviour[] _singletonPrefabsToSpawn;

    private void Awake()
    {
        if (!IsAloneSingleton()) { return; }

        foreach (var singleton in _singletonPrefabsToSpawn)
        {
            Instantiate(singleton, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

    private bool IsAloneSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
            return true;
        }

        Destroy(gameObject);
        return false;
    }
}
