using UnityEngine;

public class CheckRangeCycle : MonoBehaviour
{
    
    public NodeState CheckRange(Blackboard bb,CheckDistanceSetting checkDistanceSetting)
    {
        if (bb.Target ==null|| bb.OwnerTransform == null)
        {
            checkDistanceSetting._type = DistanceCheckType.Unknown;
            Debug.LogWarning("[CheckRangeCycle] Blackboard 또는 Target/Owner 미설정");
            return NodeState.Failure;
        }
        
        
        float dist = Vector3.Distance(bb.Target.position, bb.OwnerTransform.position);
        
        // 기준값(동적으로 바뀔 수 있는 값)을 로드
        float n = checkDistanceSetting._minRange;   // 보통 n
        float n2 = checkDistanceSetting._maxRange;  // 보통 n2
        
        if (dist <= n)
            checkDistanceSetting._type = DistanceCheckType.Close;
        else if (dist > n && dist <= n2)
            checkDistanceSetting._type = DistanceCheckType.Mid;
        else
            checkDistanceSetting._type = DistanceCheckType.Far;
        
        Debug.Log($"[CheckRangeCycle] Distance changed: {checkDistanceSetting._type}");
        
        return NodeState.Success;
    }
}
