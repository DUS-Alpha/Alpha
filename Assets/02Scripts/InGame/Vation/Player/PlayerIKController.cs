using System.Security.Claims;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public enum RigType
{
    Hand,
    Aim
}

public class PlayerIKController : MonoBehaviour
{
    [SerializeField]
    private RigBuilder m_rigBuilder;

    [SerializeField]
    private Rig m_handLayerRig;
    [SerializeField]
    private Rig m_aimRig;
    private MultiAimConstraint m_aimMultiAim;

    [SerializeField]
    private float m_handWeight = 0.7f;
    [SerializeField]
    private float m_multiAimConstraintWeight = 0.7f;

    private void Awake()
    {
        SetRigWeight(RigType.Hand, false);
        SetRigWeight(RigType.Aim, false);
        m_aimMultiAim = m_aimRig.GetComponentInChildren<MultiAimConstraint>();
        m_aimMultiAim.weight = m_multiAimConstraintWeight;
    }

    public void SetRigWeight(RigType rigType, bool isWeight)
    {
        int _weight = isWeight ? 1 : 0;
        switch (rigType)
        {
            case RigType.Hand:
                    m_handLayerRig.weight = _weight;
                break;
            case RigType.Aim:
                    m_aimRig.weight = _weight;
                break;
        }
    }

    public void SetRigTarget(Transform leftHnadIKTr, Transform rightHandIKTr, Transform leftHintIKTr, Transform rightHintIKTr)
    {

       /* // 스왑애니메이션 완료후 위치값을해야 정상으로 포지션 지정된다!!

        // 아니지 TR저거를 계속 업데이트 테스트해보기
        if (leftHnadIKTr == null)
        {
            m_leftHandIK.weight = 0;
        }
        else
        {
            m_leftHnadIKTargetTr.localPosition = m_leftHnadIKTargetTr.parent.InverseTransformPoint(leftHnadIKTr.position);
            //m_leftHnadIKTargetTr.position = leftHnadIKTr.position;
            //m_leftHnadIKTargetTr.rotation = m_leftHnadIKTargetTr.rotation;
        }
        if (rightHandIKTr == null) m_rightHandIK.weight = 0;
        else
        {
            m_rightHandIKTargetTr.position = rightHandIKTr.position;
            //m_rightHandIKTargetTr.rotation = rightHandIKTr.rotation;
        }*/
    }
}