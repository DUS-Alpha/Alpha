using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public enum CMType
{
    None = 0,
    MeleeCM,
    RangeRifleCM,
    RangeSniperCM
}
public class PlayerCameraManger : MonoBehaviour
{
    public Camera MainCamera;

    [SerializeField]
    private CinemachineBrain m_mainCMB;
    [SerializeField]
    private CinemachineCamera m_meleeCM;

    [SerializeField]
    private CinemachineCamera m_rangeRifleCM;
    [SerializeField]
    private CinemachineCamera m_rangeSniperCM;
    [Space(10)]

    [Header("[Scope FOV ]")]
    [SerializeField]
    private float m_originFOV = 60;
    [SerializeField]
    private float m_aimFOV = 35;
    [SerializeField]
    private float m_sniperFOV = 15;


    [Header("[ Effect ]")]
    [SerializeField]
    private CinemachineImpulseSource m_impulseSource; //Freelook카메라에 AddExtension의 CinemachineImpulseListener
    
    [SerializeField]
    private bool m_isCursorLock;

    // 허용 오차
    private float m_currentFOV;
    private float m_targetFOV;
    private float m_velocity = 0f;
    private float m_smoothTime = 0.1f; // 작을수록 빠르게, 클수록 느리게

    // Shak
    private Vector3 m_shakeVector;

    private void Awake()
    {
        m_rangeSniperCM.gameObject.SetActive(false);
    }

    private void Start()
    {
        m_currentFOV = m_originFOV;
        m_targetFOV = m_originFOV;
        m_rangeRifleCM.Lens.FieldOfView = m_currentFOV;

        m_meleeCM.Priority = 11;
        m_rangeRifleCM.Priority = 9;

        m_isCursorLock = true;
    }

    public void ChangeCM(CMType cm)
    {
        switch (cm)
        {
            case CMType.None:
            case CMType.MeleeCM:
                m_meleeCM.Priority = 11;
                m_rangeRifleCM.Priority = 9;
                m_rangeSniperCM.Priority = 0;
                Sniper(false);
                break;
            case CMType.RangeRifleCM:
                m_meleeCM.Priority = 9;
                m_rangeRifleCM.Priority = 11;
                m_rangeSniperCM.Priority = 0;
                Sniper(false);
                break;
        }
    }
    private void Sniper(bool isSniper)
    {
        if (isSniper)
        {
            m_mainCMB.DefaultBlend.Style = CinemachineBlendDefinition.Styles.Cut;
        }
        else
        {
            m_mainCMB.DefaultBlend.Style = CinemachineBlendDefinition.Styles.EaseInOut;
        }
    }

    public void AimFOV(bool isAim, bool isSniper)
    {
        if(isAim)
        {
            if(isSniper)
            {
                m_meleeCM.Priority = 9;
                m_rangeSniperCM.Priority = 10;
                m_rangeSniperCM.Priority = 20;
                m_rangeSniperCM.gameObject.SetActive(true);
                Sniper(true);
            }
            else
            {
                m_rangeSniperCM.gameObject.SetActive(false);
                ChangeCM(CMType.RangeRifleCM);
            }
        }

        m_targetFOV = isAim ? (isSniper ? m_sniperFOV : m_aimFOV) : m_originFOV;
        
    }
 
    private void Update()
    {
        SetCursorLock(m_isCursorLock);
        m_currentFOV = Mathf.SmoothDamp(m_currentFOV, m_targetFOV, ref m_velocity, m_smoothTime);
        m_rangeRifleCM.Lens.FieldOfView = m_currentFOV;
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
