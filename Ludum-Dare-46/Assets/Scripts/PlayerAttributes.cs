using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    internal float MovementSpeed { get; private set; }
    internal BulletType BulletType { get; private set; }

    private void ApplyUpgrade(Upgrade upgrade)
    {
        switch (upgrade.Type)
        {
            case UpgradeType.MovementSpeed:
                MovementSpeed += upgrade.FloatValue;
                break;

            case UpgradeType.BulletType:
                BulletType = upgrade.BulletTypeValue;
                break;
        }
    }
}
