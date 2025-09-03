using System.Linq;
using UnityEngine;


public class PlayerCombat : MonoBehaviour
{
    private PlayerCore m_playerCore;
    private CharacterController m_characterController;

    public bool IsAttack { get; private set; }
    public bool IsWeaponSwap { get; private set; }
    public int m_currentWeaponNum;
    private int m_swapWeaponNum;
    public void Initialize(PlayerCore playerCore)
    {
        m_playerCore = playerCore;
        m_characterController = m_playerCore.PlayerCharacterController;

    }

    private void Start()
    {
        m_playerCore.CheckInputAction += CheckInput;
        m_currentWeaponNum = 0;
    }
    public void CheckInput()
    {
        IsAttack = m_playerCore.InputHandler.IsAttack;
        m_swapWeaponNum = m_playerCore.InputHandler.SwapWeaponNum;
    }


    // TODO : 전략패턴
    public void Attack(bool isAttack)
    {
        // 현재 무기에 따라 값 공격 방식 변경
        // TODO : Melee일 때 앞으로 살짝 이동이 필요할듯?
        m_playerCore.AniController.AttackAni(isAttack);
        
    }

    public void Aiming()
    {

    }

    // 현재는 단순 Holder만 OnOff
    public void SwapWeapon()
    {
        if (m_swapWeaponNum == m_currentWeaponNum) return;
        if (IsWeaponSwap) return;

        m_currentWeaponNum = m_swapWeaponNum;

        m_playerCore.EquipmentController.SwapWeapon(m_currentWeaponNum);
        m_playerCore.AniController.SwapWeaponAni(m_currentWeaponNum);
    }

    public void OnAnimatorMove()
    {
        if (m_playerCore.AniController.IsRootMotion)
        {
            m_playerCore.AniController.UpdateAnimatorTransformValue();
            // Animator가 계산한 이동량을 가져와서 CharacterController에 적용
            Vector3 _deltaPosition = m_playerCore.AniController.RootMotionPos;
            //_deltaPosition.y = m_playerCore.Locomotion.BaseGravity * Time.deltaTime; // 중력 보정 (필요 시)

            m_characterController.Move(_deltaPosition);
            m_characterController.transform.rotation *= m_playerCore.AniController.RootMotionRot;
        }
    }
}
