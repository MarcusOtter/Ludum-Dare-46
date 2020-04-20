using UnityEngine;

public class SeedDrop : MonoBehaviour
{
    [SerializeField] private Seed[] _seedDrops;

    private enum Dropper
    {
        Enemy,
        Plant,
        None
    }

    private Dropper dropper = Dropper.None;
    private Enemy enemyComponent;
    private Plant plantComponent;

    private void Awake()
    {
        enemyComponent = GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            dropper = Dropper.Enemy;
        }
        else
        {
            plantComponent = GetComponent<Plant>();
            if (plantComponent != null)
            {
                dropper = Dropper.Plant;
            }
        }
    }

    private void OnEnable()
    {
        switch (dropper)
        {
            case Dropper.Plant:
                plantComponent.OnGrowthStageChanged += DropOnPlantGrowUp;
                break;
            case Dropper.Enemy:
                enemyComponent.OnStateChanged += DropOnEnemyDeath;
                break;
            default:
                Debug.LogError($"You've attached a seed dropper to something which can't drop a seed: \n{name}");
                break;
        }
    }

    private void OnDisable()
    {
        switch (dropper)
        {
            case Dropper.Plant:
                plantComponent.OnGrowthStageChanged -= DropOnPlantGrowUp;
                break;
            case Dropper.Enemy:
                enemyComponent.OnStateChanged -= DropOnEnemyDeath;
                break;
        }

    }

    public void DropOnEnemyDeath(EnemyState floob, EnemyState newState)
    {
        if (newState == EnemyState.Dead)
        {
            TryDropSeed();
        }
    }

    public void DropOnPlantGrowUp(GrowthStage floob, GrowthStage newState)
    {
        if (newState == GrowthStage.FullyGrown)
        {
            TryDropSeed();
        }
    }

    public void TryDropSeed()
    {
        for (int i = 0; i < _seedDrops.Length; i++)
        {
            int r = Random.Range(0, 100);
            if (r <= _seedDrops[i].Probability)
            {
                Instantiate(_seedDrops[i], transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0f), Quaternion.Euler(0,0,Random.Range(0f, 360f)));
            }
        }
    }
}
