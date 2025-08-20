using UnityEngine;

public class BossActions : MonoBehaviour
{
    public Blackboard BB { get; private set; }
    public void SetBlackboard(Blackboard bb) { BB = bb; }

    // === 디버그용 토글(키 입력으로 바뀜) ===
    [Header("Debug Toggles")]
    public bool dbgMagazineEmpty = false;
    public bool dbgUnderHeavyFire = false;

    [Header("Debug Ranges (for gizmo)")]
    public float ShootRange = 18f;
    public float OptimalRange = 14f;
    public float TooCloseRange = 6f;

    private void Update()
    {
        // 1: 탄약 없음 토글
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            dbgMagazineEmpty = !dbgMagazineEmpty;
            Debug.Log($"[DBG] MagazineEmpty = {dbgMagazineEmpty}");
        }
        // 2: 집중 사격 받는 중 토글
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            dbgUnderHeavyFire = !dbgUnderHeavyFire;
            Debug.Log($"[DBG] UnderHeavyFire = {dbgUnderHeavyFire}");
        }
    }

    // === 액션 훅(내용은 나중에 채움) ===
    public NodeState Approach()
    {
        print("접근중!");
        return NodeState.Success;
    }

    public NodeState Aim()
    {
        print("조준중");
        return NodeState.Success;
    }

    public NodeState Fire()
    {
        print("");
        return NodeState.Success;
    }
    public NodeState Reload()    { return NodeState.Success; }
    public NodeState KeepDistance(){ return NodeState.Success; }

    // === 조건 훅(지금은 디버그 토글/간단 true) ===
    public bool HasTarget()        => BB != null && BB.Target != null;
    public bool HasLineOfSight()
    {
        if (BB == null || BB.Target == null) return false;
        var d = Vector3.Distance(BB.OwnerTransform.position, BB.Target.position);
        return d <= ShootRange; // 아주 단순한 시야/사거리 판정
    }
    public bool IsMagazineEmpty()  => dbgMagazineEmpty;
    public bool IsUnderHeavyFire() => dbgUnderHeavyFire;
    public bool IsAnimFinished()   => true;
}