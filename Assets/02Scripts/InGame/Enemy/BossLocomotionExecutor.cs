using UnityEngine;

//어떻게 실행할건지 하는 실행기 
public class BossLocomotionExecutor : MonoBehaviour {
    [Tooltip("[ Player ]")]
    public Transform target; // 보스가 따라갈/응시 할 대상(대부분은 플레이어가 될것 )
    public Animator animator;

    [Header("이동 및 회전 파라미터")] 
    
    public float rotSpeed = 12f; //회전 속도 
    public float approachSpeed = 3.2f; //플레이어 접근 속도
    public float retreatSpeed = 3.0f; // 퇴각 혹은 뒤로가는 속도 
    public float orbitSideSpeed = 3.0f; // 오른쪽 왼쪽으로 피하는 속도
    public float orbitFwdGain=2.0f; //거리 오차 확인 -> 전/후 보정 강도(클 수록 거리 맞추기 적극적) 
    
    //블렌드 트리  파라미터 해시(애니메이터의 파라미터를 코드로 접근하는 방법)
    static readonly int MoveX = Animator.StringToHash("MoveX");
    static readonly int MoveZ = Animator.StringToHash("MoveZ");
    
    // 좌/우 스트레이프 방향(+1:오른쪽, -1:왼쪽), 주기적 전환용 타이머
    float sideSign = 1f, sideTimer;

    /// <summary>
    /// 선택된 MoveMode에 맞춰 실제 이동/회전/애니 파라미터를 업데이트한다.
    /// CombatMover에서 매 프레임 호출해주는 진입점.
    /// </summary>
     public void Tick(MoveMode mode, float idealRadius, float dt) {
        if (!target) return;

        // 1) 플레이어 방향 벡터(수평 평탄화)
        Vector3 to = target.position - transform.position;
        to.y = 0;

        // 2) 부드럽게 응시(항상 타깃 바라보기)
        if (to.sqrMagnitude > 0.0001f) {
            var look = Quaternion.LookRotation(to.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, rotSpeed * dt);
        }

        // 로컬 축(앞/오른쪽) 캐시
        Vector3 fwd = transform.forward;
        Vector3 right = transform.right;

        // 최종 속도 벡터(월드 기준)
        Vector3 vel = Vector3.zero;
        print(mode);

        // 3) 모드별 속도 구성
        switch (mode) {
            case MoveMode.Approach:            // 멀다 → 앞으로 붙기
                vel = fwd * approachSpeed;
                break;

            case MoveMode.Retreat:             // 가깝다 → 뒤로 빠지기
                vel = -fwd * retreatSpeed;
                break;

            case MoveMode.Orbit:               // 적정 거리 → 좌/우로 흐르며 거리 미세 보정
                sideTimer -= dt;
                // 주기적으로 좌/우 방향을 랜덤 전환(움직임이 더 자연스러워짐)
                if (sideTimer <= 0f) {
                    sideTimer = Random.Range(1.0f, 2.0f);
                    if (Random.value < 0.5f) sideSign = -sideSign;
                }
                // 거리 오차(목표반경 - 현재거리) → 전/후 보정량
                float dist = to.magnitude;
                float error = Mathf.Clamp(idealRadius - dist, -1f, 1f);

                // 좌/우 성분 + 전/후 보정 성분의 합성
                vel = right * (orbitSideSpeed * sideSign)
                    +  fwd   * (error * orbitFwdGain);
                break;
        }

        // 4) 실제 이동(Transform 방식)
        transform.position += vel * dt;

        // 5) 블렌드 트리 입력(로컬 투영값)
        //    MoveX: 좌/우(+는 오른쪽), MoveZ: 전/후(+는 앞)
        float localX = Vector3.Dot(vel, right);
        float localZ = Vector3.Dot(vel, fwd);

        // 댐핑을 주면 블렌딩이 부드럽고 "훅훅 흐르는" 느낌이 자연스러움
        animator.SetFloat(MoveX, localX, 0.1f, dt);
        animator.SetFloat(MoveZ, localZ, 0.1f, dt);
    }
}