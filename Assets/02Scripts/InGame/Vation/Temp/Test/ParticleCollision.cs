using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    private HashSet<GameObject> m_alreadyHitObjs = new HashSet<GameObject>();

    private void OnEnable()
    {
        m_alreadyHitObjs.Clear();
    }
    private void OnParticleCollision(GameObject other)
    {
        if (m_alreadyHitObjs.Contains(other)) return;
        m_alreadyHitObjs.Add(other);
        Debug.Log(other.name);
    }
}
