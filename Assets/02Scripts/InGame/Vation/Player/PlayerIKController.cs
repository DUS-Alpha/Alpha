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

    [SerializeField]
    private TwoBoneIKConstraint m_leftHandIK;
    [SerializeField]
    private TwoBoneIKConstraint m_rightHandIK;

    [SerializeField]
    private Transform m_leftHnadIKTargetTr;
    [SerializeField]
    private Transform m_rightHandIKTargetTr;

    [SerializeField]
    private Transform m_leftHintTargetTr;
    [SerializeField]
    private Transform m_rightHintTargetTr;

    private Transform m_defaultLeftHintTr;
    private Transform m_defaultRightHintTr;
    private void Awake()
    {
        SetWeight(RigType.Hand, 0);
        SetWeight(RigType.Aim, 0);

        m_defaultLeftHintTr = m_leftHandIK.data.hint.transform;
        m_defaultRightHintTr = m_rightHandIK.data.hint.transform;
    }

    public void SetWeight(RigType rigType, float value)
    {
        switch (rigType)
        {
            case RigType.Hand:
                m_handLayerRig.weight = value;
                break;
            case RigType.Aim:
                m_aimRig.weight = value;
                break;
        }
    }

    public void SetRigTarget(Transform leftHnadIKTr, Transform rightHandIKTr, Transform leftHintIKTr, Transform rightHintIKTr)
    {

        // 스왑애니메이션 완료후 위치값을해야 정상으로 포지션 지정된다!!

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
        }
    }
}