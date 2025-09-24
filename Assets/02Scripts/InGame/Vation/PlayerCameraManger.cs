using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraManger : MonoBehaviour
{
    public Camera MainCamera;
    [SerializeField]
    private CinemachineCamera m_freeLook;
    [SerializeField]
    private CinemachineImpulseSource m_impulseSource; //Freelook카메라에 AddExtension의 CinemachineImpulseListener
    
    [Space(10)]

    [Header("[ FOV ]")]
    [SerializeField]
    private float m_originFOV = 60;
    [SerializeField]
    private float m_aimFOV = 40;

    
    [SerializeField]
    private bool m_isCursorLock;

    
    // 허용 오차
    private float m_currentFOV;
    private float m_targetFOV;
    private float m_velocity = 0f;
    private float m_smoothTime = 0.1f; // 작을수록 빠르게, 클수록 느리게

    // Shak
    private Vector3 m_shakeVector;
    private void Start()
    {
        m_currentFOV = m_originFOV;
        m_targetFOV = m_originFOV;
        m_freeLook.Lens.FieldOfView = m_currentFOV;

        m_isCursorLock = true;
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

        SetCursorLock(m_isCursorLock);
    }

    private void SetCursorLock(bool isCursorLock)
    {
        if (!m_isCursorLock)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;

            Cursor.visible = false;
        }
    }
    
    public void CameraShake(float intensity = 1f)
    {
        if (m_impulseSource != null)
        {
            m_impulseSource.DefaultVelocity = new Vector3(Random.Range(-0.03f, 0.03f), Random.Range(-0.05f, 0.05f), 0);
            m_impulseSource.GenerateImpulse(intensity);
        }
    }
}
