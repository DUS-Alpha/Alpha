using UnityEngine;
using UnityEngine.Serialization;


[RequireComponent(typeof(CombatDirector))]
[RequireComponent(typeof(StanceController))]
[RequireComponent(typeof(AttackSelector))]
[RequireComponent(typeof(CombatMover))]
public class BossActions : MonoBehaviour
{
    public Blackboard BB { get; private set; }
    public float FarRange = 20f;
    public float MidRange = 15f;
    public float CloseRange = 8f;

    public void SetBlackboard(Blackboard bb) => BB = bb;

    public CombatDirector director;

    void Awake() {
        if (!director) director = GetComponent<CombatDirector>();
        director.Bind(this); // BB / Animator / CombatMover 등 연결
    }

    // BT에서 이 함수만 부르면 됨
    // public NodeState DirectorTick() => director.Tick();
    public NodeState DirectorTick()
    {
        return director.Tick();
    }
}