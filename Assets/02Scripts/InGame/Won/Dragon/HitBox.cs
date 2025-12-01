using System;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public String partName;
    public IDamageable damageable;

    private void Awake()
    {
        damageable = GetComponentInParent<IDamageable>();
    }
}
