using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade")]
public class Upgrade : ScriptableObject
{
    [Header("Upgrade settings")]
    public UpgradeType Type;
    public string UpgradeText;
    public Sprite PlantUpgradeSprite;
    public int aquiredOnLevel = -1;

    [Header("Upgrade values")]
    public float FloatValue;
    public float IntValue;
    public Bullet BulletPrefab;
}

