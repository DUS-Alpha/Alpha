using System;
using UnityEngine;
namespace alpha
{
    [CreateAssetMenu(fileName = "MeleeWeapon", menuName = "Scriptable Objects/ItemData/MeleeWeapon")]
    public class MeleeWeaponItemDataSO : WeaponItemDataSO
    {
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            WeaponType = EWeaponTypes.Melee;
        }
#endif

        private void OnTriggerEnter(Collider other)
        {
            /*if(other.CompareTag("Enemy"))
            {
                IDamageable _damageableTarget;
                if (other.TryGetComponent<IDamageable>(out _damageableTarget))
                {
                    DamageMassage _damageMassage = new DamageMassage();
                    _damageMassage.Damage = this.WeaponData.CombatData.Damage;

                    // 데미지 전달
                    _damageableTarget.ApplyDamage(_damageMassage);
                    // 맞은 곳에 BloodEffect 재생(몬스터쪽에서해도 괜찮음)

                    Debug.Log(other.name);
                }
            }*/
        }
        public void DoDamage(Transform pos, float radius)
        {
            /*Collider[] hits = Physics.OverlapSphere(pos.position, radius, 1 << LayerMask.NameToLayer("Enemy"));
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<IDamageable>(out IDamageable _damageableTarget))
                {
                    DamageMassage _damassage = new DamageMassage
                    {
                        //HitNormal = hit.normal,
                        //HitPoint = hit.point,
                        Damage = WeaponData.CombatData.Damage
                    };
                    _damageableTarget.ApplyDamage(_damassage);
                }
            }*/
        }
    }
}