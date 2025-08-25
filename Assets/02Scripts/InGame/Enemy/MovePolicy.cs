using UnityEngine;

public enum MoveMode { Approach, Orbit, Retreat }

[System.Serializable]
public class MovePolicy {
    public float idealRadius = 4f;
    public float band = 1.2f;
    public MoveMode Decide(Vector3 self, Transform target) {
        if (!target) return MoveMode.Orbit;
        float d = Vector3.Distance(self, target.position);
        float lo = idealRadius - band * 0.5f, hi = idealRadius + band * 0.5f;
        if (d < lo) return MoveMode.Retreat;
        if (d > hi) return MoveMode.Approach;
        return MoveMode.Orbit;
    }
}