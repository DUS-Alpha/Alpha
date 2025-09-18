using Unity.Cinemachine;
using UnityEngine;

public class PlayerCameraManger : MonoBehaviour
{
    [SerializeField]
    private CinemachineCamera m_freeLook;
    [SerializeField]
    private float m_originFOV = 60;
    [SerializeField]
    private float m_aimFOV = 35;

    public void AimFOV(bool isAim)
    {
        // TODO : Lerp사용하여 부드럽게 전환
        if(isAim) m_freeLook.Lens.FieldOfView = m_aimFOV;
        else m_freeLook.Lens.FieldOfView = m_originFOV;

    }
}
