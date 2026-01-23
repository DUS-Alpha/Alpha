using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace alpha
{
    // 공격 대기 상태
    public class PlayerCombatReadyState : PlayerStateBase
    {
        public PlayerCombatReadyState(PlayerCore playerCore) : base(playerCore) { }

        public override void Enter()
        {
            m_NextStateDelay = 0;
        }

        public override void Update()
        {
            /*bool _isSwap = m_InputM.IsSwap;
            int _swapNum = m_InputM.SwapNum;
            bool _isAttack = m_InputM.IsAttackBtn;
            bool _isSkill = m_InputM.IsSkill;
            bool _isAim = m_InputM.IsAim;
            bool _isDash = m_InputM.IsDash;


            m_NextStateDelay += Time.deltaTime;
            m_Combat.InvokeRegenerateGauge();

            if (_isAttack)
            {
                //m_Core.SwitchCombatState(CombatStateType.Attack);
                return;
            }

            if (m_NextStateDelay > 2.5f)
            {
                //m_Core.SwitchCombatState(CombatStateType.NonCombat);
                m_Combat.SetIsInCombat(false);
            }*/
        }
        public override void Exit()
        {

        }

    }
}