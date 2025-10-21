using System.Collections.Generic;
using UnityEngine;

public class HitBoxManager : MonoBehaviour
{
    public List<HitBox> BossHitBoxList = new List<HitBox>();
    
    private void OnValidate()
    {
        BossHitBoxList.Clear();
        HitBox[] found = GetComponentsInChildren<HitBox>(true);
        BossHitBoxList.AddRange(found);
        Debug.Log($"[BossColliderManager] 총 {BossHitBoxList.Count}개의 HitBox를 찾았습니다.");

        
        foreach(HitBox hit in found)
        {
            hit.gameObject.layer = LayerMask.NameToLayer("Enemy");
        }

    }
}
