using UnityEngine;

namespace alpha
{
    public interface IHealthModel
    {
        public int MaxHealth {  get; }
        public int CurrentHealth { get; }
        public bool IsDead { get; }

        public void ApplyDamage(int damage);

        public void Kill();

        public void Heal(int ammount);
    }
}