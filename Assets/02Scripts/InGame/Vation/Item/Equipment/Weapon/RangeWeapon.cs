using UnityEngine;

public enum RangeTypes
{
    Pistol,
    Rifle,
    Sniper,
    Machinegun
}

public class RangeWeapon : Weapon
{
    public RangeTypes RangeType;
    public override void Attack(GameObject target)
    {

    }
}
