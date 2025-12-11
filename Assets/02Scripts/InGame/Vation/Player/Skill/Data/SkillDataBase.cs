using UnityEngine;
using UnityEngine.UI;

public enum EKeyTypes
{
    Q,
    E,
    Z,
    C
}

public enum ESkillTypes
{
    Attack,
    Dash,       // Dodge(회피) 여부
    Recovery,   // 회복
    Buff,       // 강화
}

public enum EActivationTypes
{
    Active,
    Passive
}

public enum ECastingTypes
{
    InstantCast,            // 즉시 발동 (트레이서 점멸, 솔저 힐 스탤)
    TargetingCast,          // 타겟 지정 발동
    TargetPointCast,        // 발동 후 마우스로 지점 지정 (메이 얼음벽)
    TargetDirectionCast,    // 방향만 지정해 발동 (라인 돌진)
    ChargeCast,             // 충전, 떼면 발동 (한조 활)
    ChanneledCast,          // 지속 시전, 취소 시 종료 (자리야 방벽,모이라 치유/흡수 ), 지속시간 동안 효과를 유지하기 위해서 시전자가 다른 행동을 할 수 없는 스킬
    ProjectileCast,         // 투사체 발사 (솔저 로켓, 파라 로켓)
    DeployableCast,         // 설치형 오브젝트 (토르비욘 포탑)
    ToggleCast,             // On/Off (전환도 포함)
    DelayedCast             // 지연 후 발동 (정크렛 지뢰)
}

public enum ESkillTargetTypes
{
    RaycastHit,             // 레이기반 (맥크리 권총, 즉시 맞는 형태)
    Projectile,             // 투사체 기반 (파라 로켓, 날라가서 맞아봐야 아는)
    SingleTarget,           // 단일 타겟 (로그호그 Hook)
    AreaOfEffect,           // 원형 범위 (자리야 폭발)
    ConeShape,              // 전방 원뿔 (파라 부스터 충격)
    Self,                   //
    MultiTarget,            // 범위 내 최대 인원 만큼
    GroundPlacement,        // 지면에 설치 (윈스턴 방벽, 시메 포탑)
    ForwardDash             // 방향성 이동 + 충돌 (라인 박치기)
}


public class SkillDataBase : ScriptableObject
{
    [Header("[ Base ]")]
    public string SkillName;
    public EKeyTypes KeyType;
    public ESkillTypes SkillType;
    public EActivationTypes ActivationType;
    public ECastingTypes CastingType;
    public ESkillTargetTypes SkillTargetType;

    public Sprite Icon;
    public float CoolDown;
    public bool CanMove;
    [TextArea] public string Description;
}
