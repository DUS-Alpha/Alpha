using UnityEngine;
public interface IDamageable
{
    void ApplyDamage(DamageMassage damageMassage);
}

// 구조체인이유는 클래스로 구현시 참조에 의한 값 변경이 이루어지는것을 막기위해
public struct DamageMassage
{
    public GameObject Damager;
    public float Amount;

    public Vector3 HitPoint;
    public Vector3 HitNormal;
}

