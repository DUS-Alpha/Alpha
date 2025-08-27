using UnityEngine;

// 이동 모드 (동일)
public enum MoveMode { Approach, Orbit, Retreat }

[System.Serializable]
public class MovePolicy
{
    
    public float idealRadius = 4f;
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

        // 1) 유지 시간 소모
        _holdTimer -= dt;

        // 2) 유지 시간 중에도 아주 가끔 즉흥 전환 허용(선택)
        if (_holdTimer > 0f && spontaneousChangePerSec > 0f)
        {
            // 초당 p 확률 → 프레임당 대략 (p * dt) 확률
            if (Random.value < spontaneousChangePerSec * dt)
            {
                RerollMode(); // 즉흥 전환
                // 유지 시간을 새로 배정
                _holdTimer = Random.Range(holdTimeRange.x, holdTimeRange.y);
                return _current;
            }
        }

        // 3) 유지 시간이 끝났으면 새 모드 뽑기
        if (_holdTimer <= 0f)
        {
            RerollMode();
            _holdTimer = Random.Range(holdTimeRange.x, holdTimeRange.y);
        }

        // 4) 현재 모드 유지
        return _current;
    }

    // 가중치 기반 룰렛 뽑기
    void RerollMode()
    {
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
