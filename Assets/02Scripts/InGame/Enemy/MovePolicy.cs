using UnityEngine;

// 이동 모드 (동일)
public enum MoveMode { Approach, Orbit, Retreat }

[System.Serializable]
public class MovePolicy
{
    //적정거리
    public float idealRadius = 3f;
    // ─────────────────────────────────────────────────────
    // 랜덤 정책 설정값 (인스펙터에서 조절)
    // ─────────────────────────────────────────────────────
    [Header("랜덤 정책 확률(가중치)")]
    [Tooltip("Approach(접근)으로 선택될 상대 가중치")]
    public float wApproach = 1.0f;

    [Tooltip("Orbit(유지/스트레이프)로 선택될 상대 가중치")]
    public float wOrbit = 2.0f;

    [Tooltip("Retreat(후퇴)로 선택될 상대 가중치")]
    public float wRetreat = 1.0f;

    [Header("모드 유지 시간(점착성)")]
    [Tooltip("한 번 고른 모드를 최소한 유지할 시간(초)")]
    public Vector2 holdTimeRange = new Vector2(0.6f, 1.2f);

    [Header("가끔 즉흥적으로 바꾸기(선택)")]
    [Tooltip("유지 시간 도중에도, 초당 이 확률로 가끔 즉흥 전환(0=안함)")]
    [Range(0f, 1f)] public float spontaneousChangePerSec = 0.0f;

    public bool  IsTooClose = false;          // 붙음 감지 플래그
    public float retreatExitDist = 5f;      // 여기까지 벌어지면 후퇴 종료(절대값)
    public bool  useDeltaFromIdeal = true;    // true면 idealRadius + delta 방식
    public float retreatExitDelta = 0.6f;     // idealRadius + Δ로 종료 거리 결정
    public float forcedRetreatMinTime = 0.20f; // 최소 후퇴 유지 시간(핑퐁 방지)
        
    // 내부 타이머
    float _forceRetreatTimer = 0f;
    // 내부 상태
    MoveMode _current = MoveMode.Orbit; // 시작 기본값
    float _holdTimer = 0f;

    /// <summary>
    /// 랜덤 정책: 거리/각도 등은 전혀 보지 않고,
    /// 확률(가중치)로 모드를 뽑은 뒤 일정 시간 유지한다.
    /// CombatMover에서 매 프레임 호출됨.
    /// </summary>
    public MoveMode Decide(Vector3 self, Transform target)
    {
        float dt = Time.deltaTime;
        if (!target) return _current;

        // 거리 (수평만 쓰고 싶으면 y=0 평탄화)
        float dis = Vector3.Distance(self, target.position);

        // 종료 기준 거리 계산
        float exitDist = useDeltaFromIdeal ? (idealRadius + retreatExitDelta)
            : retreatExitDist;

        // ── A) "찍힘" 상태 유지 중이면: 무조건 Retreat 고정 ──
        if (IsTooClose)
        {
            _forceRetreatTimer -= dt;

            // 충분히 벌어졌고 + 최소 유지시간 지났으면 해제
            if (dis >= exitDist && _forceRetreatTimer <= 0f)
            {
                IsTooClose = false;
                _holdTimer = 0f;             // 다음 프레임에 자연스럽게 새 모드 뽑도록
            }
            else
            {
                _current = MoveMode.Retreat; // 아직은 계속 후퇴
                return _current;
            }
        }

        // ── B) 아직 안 찍혔는데, 임계선 안으로 들어오면 '한 번 찍기' ──
        if (dis <= idealRadius)              // 원하는 임계선(예: 3m)
        {
            IsTooClose = true;
            _forceRetreatTimer = Mathf.Max(_forceRetreatTimer, forcedRetreatMinTime);
            _current = MoveMode.Retreat;     // 즉시 후퇴 시작
            return _current;
        }

        // ── C) 위 두 조건에 안 걸리면 기존 랜덤 정책 ──
        _holdTimer -= dt;

        if (_holdTimer > 0f && spontaneousChangePerSec > 0f)
        {
            if (Random.value < spontaneousChangePerSec * dt)
            {
                RerollMode();
                _holdTimer = Random.Range(holdTimeRange.x, holdTimeRange.y);
                return _current;
            }
        }

        if (_holdTimer <= 0f)
        {
            RerollMode();
            _holdTimer = Random.Range(holdTimeRange.x, holdTimeRange.y);
        }

        return _current;
    }


    // 가중치 기반 룰렛 뽑기
    void RerollMode()
    {
        //플레이어와 적의 거리가 3(idealRadius) 아래면  무조건 후퇴를 뽑게 하기 
      
        
        float a = Mathf.Max(0f, wApproach);
        float o = Mathf.Max(0f, wOrbit);
        float r = Mathf.Max(0f, wRetreat);
        float sum = a + o + r;

        // 전부 0이면 기본 Orbit
        if (sum <= 0f) { _current = MoveMode.Orbit; return; }

        float pick = Random.value * sum;
        if ((pick -= a) <= 0f) { _current = MoveMode.Approach; return; }
        if ((pick -= o) <= 0f) { _current = MoveMode.Orbit;    return; }
        _current = MoveMode.Retreat;
    }
}
