using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace alpha
{
    public class PlayerNonCombatState : PlayerCombatStateBase
    {
        public PlayerNonCombatState(PlayerCore playerCore) : base(playerCore) { }

        public override void Enter()
        {

        }

        public override void Update()
        {
            bool _isSwap = m_InputM.IsSwap;
            int _swapNum = m_InputM.SwapNum;
            bool _isAttack = m_InputM.IsAttackBtn;
            bool _isSkill = m_InputM.IsSkill;
            bool _isAim = m_InputM.IsAim;
            bool _isDash = m_InputM.IsDash;

            if (_isSwap)
            {
                // Swap가능한지 체크
                /*if (m_Core.CanSwap(_swapNum))
                {
                    m_Core.SwitchCombatState(CombatStateType.Swap);
                }*/
            }

            m_Combat.InvokeRegenerateGauge();
            /*if (m_Combat.CanSwap)
            {
                m_Core.SwitchCombatState(CombatStateType.Swap);
            }

            if (m_Combat.CurrentSwapNum == 0) return;

            if (m_Combat.CurrentSwapNum < 4)
            {
                if (m_Combat.IsAttackBtn)
                {
                    m_Core.SwitchCombatState(CombatStateType.Attack);
                }

            }*/

            /*else if (m_Combat.IsReload)
                m_PlayerCore.SwitchCombatState(CombatStateType.Reload);
            else if (m_Combat.IsAttack || m_Combat.IsAim)
            {
                if(m_Combat.IsAim) m_Combat.SetAming();
                m_PlayerCore.SwitchCombatState(CombatStateType.InCombat);
            }
            else if (m_Combat.IsSkill)
                m_PlayerCore.SwitchCombatState(CombatStateType.Skill);*/
        }

        public override void Exit()
        {

        }
    }
}