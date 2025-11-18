using UnityEngine;

//애니메이션 이벤트에 활용 될 함수들과 파라미터 들
public class BossAnimaitionEvents : MonoBehaviour
{
    [SerializeField] private Collider RunRange; //  달리기 패턴 나올때 타켓 범위로 사용될 콜라이더 


    #region 달리기 범위 껏다키기

    public void RunRangeEnableEvent()
    {
        if (RunRange ==null) return;
        RunRange.enabled = true;
        
    }
    public void RunRangeDisableEvent()
    {
        if (RunRange ==null) return;
        RunRange.enabled = false;
    }

    #endregion
}
