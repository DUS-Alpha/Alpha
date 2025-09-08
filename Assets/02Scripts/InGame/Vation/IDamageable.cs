using UnityEngine;

public struct DamageMassage
{
    public float damageInfo;
}
public interface IDamageable
{
    void ApplyDamage(DamageMassage damageMassage);
}
