using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCameraManger : MonoBehaviour
{
    public Camera MainCamera;
    [SerializeField]
    private CinemachineCamera m_freeLook;
    [SerializeField]
    private float m_originFOV = 60;
    [SerializeField]
    private float m_aimFOV = 40;

    // 허용 오차
    const float EPSILON = 0.01f;
    private float m_currentFOV;
    private float m_targetFOV;
    private float m_velocity = 0f; // 반드시 ref로 관리해야 함
    private float m_smoothTime = 0.1f; // 작을수록 빠르게, 클수록 느리게
    private void Start()
    {
        m_currentFOV = m_originFOV;
        m_targetFOV = m_originFOV;
        m_freeLook.Lens.FieldOfView = m_currentFOV;
    }
    public void AimFOV(bool isAim)
    {
        // Aim 상태에 따라 목표 FOV 변경
        m_targetFOV = isAim ? m_aimFOV : m_originFOV;
    }
    private void Update()
    {
        m_currentFOV = Mathf.SmoothDamp(m_currentFOV, m_targetFOV, ref m_velocity, m_smoothTime);

        m_freeLook.Lens.FieldOfView = m_currentFOV;
    }
}
