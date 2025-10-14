using System.Buffers.Text;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public enum CMType
{
    None = 0,
    BaseCM,
    SniperCM
}
public class PlayerCameraManger : MonoBehaviour
{
    public Camera MainCamera { get; private set; }

    [SerializeField]
    private CinemachineBrain m_mainCMB;
    [SerializeField]
    private CinemachineCamera m_baseCM;
    [SerializeField]
    private CinemachineCamera m_sniperCM;
    [Space(10)]

    [Header("[Scope FOV ]")]
    [SerializeField]
    private float m_originFOV = 60;
    [SerializeField]
    private float m_aimFOV = 35;
    [SerializeField]
    private float m_sniperFOV = 15;

    public float m_sensitivity;

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
        MainCamera = m_mainCMB.GetComponent<Camera>();
        m_sniperCM.gameObject.SetActive(false);
    }

    private void Start()
    {
        m_currentFOV = m_originFOV;
        m_targetFOV = m_originFOV;

        m_baseCM.Lens.FieldOfView = m_currentFOV;
        m_baseCM.Priority = 11;

        m_isCursorLock = true;
    }

    public void ChangeCM(CMType cm)
    {
        switch (cm)
        {
            case CMType.None:
            case CMType.BaseCM:
                m_baseCM.Priority = 10;
                m_sniperCM.Priority = 0;
                m_sniperCM.gameObject.SetActive(false);
                Sniper(false);
                break;
            case CMType.SniperCM:
                m_baseCM.Priority = 0;
                m_sniperCM.Priority = 10;
                m_sniperCM.gameObject.SetActive(true);
                Sniper(true);
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

    public void AimFOV(bool isAiming, bool isSniper)
    {
        if (isAiming)
        {
            if (isSniper)
            {
                ChangeCM(CMType.SniperCM);
            }
        }
        else
        {
            ChangeCM(CMType.BaseCM);
        }

        m_targetFOV = isAiming ? (isSniper ? m_sniperFOV : m_aimFOV) : m_originFOV;
    }
 
    private void Update()
    {
        SetCursorLock(m_isCursorLock);
        m_currentFOV = Mathf.SmoothDamp(m_currentFOV, m_targetFOV, ref m_velocity, m_smoothTime);
        m_baseCM.Lens.FieldOfView = m_currentFOV;
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
