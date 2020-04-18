using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade")]
public class Upgrade : ScriptableObject
{
    [Header("Upgrade settings")]
    public UpgradeType Type;
    public string UpgradeText;
    public Sprite PlantUpgradeSprite;
    public int aquiredOnLevel;

    [Header("Upgrade values")]
    public float FloatValue;
    public float IntValue;
    public BulletType BulletTypeValue;
}

