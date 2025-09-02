using UnityEngine;

public class StanceController : MonoBehaviour
{
    public enum Stance { OrbitL, OrbitR, ApproachArc, RetreatArc }

    [System.Serializable]
    public class Config
    {
        [Tooltip("한 사이클에서 수행할 스탠스 횟수 범위(정수로 반올림). 최소 0이면 곧장 공격도 가능.")]
        public Vector2 countRange = new(0, 5);

        [Tooltip("각 스탠스를 유지하는 시간(초) 범위")]
        public Vector2 timeRange  = new(0.6f, 1.2f);

        [Header("공격 진입 확률")]
        [Tooltip("각 스탠스가 끝날 때, 곧바로 공격으로 넘어갈 확률 (0~1)")]
        [Range(0f, 1f)] public float attackChancePerStance = 0.35f;

        [Tooltip("사이클 시작 시 스탠스 개수가 0으로 뽑혔을 때 바로 공격으로 갈지 여부")]
        public bool attackImmediatelyIfZeroStance = true;
    }

    public Config cfg = new();

    [SerializeField]
    int stanceLeft;          // 남은 스탠스 횟수
    [SerializeField]
    float stanceTimer;       // 현재 스탠스 남은 시간
    Stance current;

    /// <summary>이번 사이클의 스탠스를 다 썼는가?</summary>
    public bool IsCycleFinished => stanceLeft <= 0;

    /// <summary>사이클(이동 스탠스 반복) 초기화</summary>
    public void ResetCycle()
    {
        stanceLeft = Random.Range(Mathf.RoundToInt(cfg.countRange.x),
                                  Mathf.RoundToInt(cfg.countRange.y) + 1);

        // 스탠스 0으로 시작하면 타이머는 의미 없음
        current = PickNext(Stance.OrbitL);
        stanceTimer = (stanceLeft > 0)
            ? Random.Range(cfg.timeRange.x, cfg.timeRange.y)
            : 0f;
    }

    /// <summary>
    /// 스탠스를 진행하고 이동을 적용. 
    /// 반환값: true면 "이번에 바로 공격으로 넘어가자"는 신호.
    /// </summary>
    public bool TickAndApply(CombatMover mover, Transform target)
    {
        // 0개 사이클로 뽑혔고, 곧장 공격 가기로 설정되어 있으면 즉시 true
        if (stanceLeft <= 0 && cfg.attackImmediatelyIfZeroStance)
            return true;

        // 스탠스 진행 중이면 이동 실행
        if (stanceLeft > 0)
            mover.CombatMove(target);

        // 타이머 소모
        stanceTimer -= Time.deltaTime;

        if (stanceLeft > 0 && stanceTimer <= 0f)
        {
            // 스탠스 하나 종료
            stanceLeft--;

            // ★ 스탠스 종료 시점에 확률적으로 바로 공격 진입
            if (Random.value < cfg.attackChancePerStance)
            {
                stanceLeft = 0; // 남은 스탠스 버리고 공격으로
                return true;
            }

            // 남아 있으면 다음 스탠스로 전환
            if (stanceLeft > 0)
            {
                current = PickNext(current);
                stanceTimer = Random.Range(cfg.timeRange.x, cfg.timeRange.y);
            }
        }

        return false; // 아직 공격으로 넘어가지 않음
    }

    Stance PickNext(Stance prev)
    {
        float r = Random.value;
        if (r < 0.55f) return (prev == Stance.OrbitL) ? Stance.OrbitR : Stance.OrbitL;
        if (r < 0.80f) return Stance.ApproachArc;
        return Stance.RetreatArc;
    }
}
