using System;
using UnityEngine;

namespace alpha
{
    public class PlayerCombatManager : MonoBehaviour, ISwapCondition
    {
        // Ref Component
        private PlayerCore m_playerCore;

        public PlayerAnimationViewManager AniM { get; private set; }
        public PlayerAudioManager AudioM { get; private set; }

        public bool IsCombatLock => m_isCombatLock;
        private bool m_isCombatLock;

        // 특정 동작 진행중(근접공격, 스왑등 애니메이션 혹은 코드상)인지 판단
        public bool IsAction => m_isAction;
        private bool m_isAction;

        // Combat이 동작되는지만 체크하는
        // ==================== Swap
        public Item CurrentItem => m_currentItem;
        private Item m_currentItem;

        // ==================== Attack
        // Melee Attack
        public bool IsNextCombo => m_isNextCombo;
        private bool m_isNextCombo;

        public int NextComboNum => m_nextComboNum;
        private int m_nextComboNum;

        public bool IsInCombat => m_isInCombat;
        private bool m_isInCombat;

        public event Action OnAttack;

        public event Action<float> OnDecreaseGauge;
        public event Action OnRegenrateGauge;
        public event Action OnResetTimer;

        public event Func<float> OnCurrentRangeAttackGauge;
        public void InitializeModule(PlayerCore playerCore)
        {
            m_playerCore = playerCore;

            /*AniM = m_playerCore.AniManager;
            AudioM = m_playerCore.PlayerAudioManager;

            m_playerCore.OnCanSwapFunc += CanSwap;*/
        }
        
        // 애니메이터SMB에서 관리
        public void SetIsAction(bool isAttacking)
        {
            m_isAction = isAttacking;
        }

        public void SetIsCombatLock(bool isActionLock)
        {
            m_isCombatLock = isActionLock;
        }

        #region ======================================== SWAP
        public bool CanSwap(int num)
        {
            return !m_isCombatLock;
        }

        public void SetCurrentSwapItem(Item item)
        {
            m_currentItem = item;
        }
        #endregion ======================================== /SWAP

        #region ======================================== ATTACK && INCOMBAT
        // Melee Combo Attack
        public void SetIsNextCombo(bool isNextCombo)
        {
            m_isNextCombo = isNextCombo;
        }
        public void SetNextComboNum(int nextNum)
        {
            m_nextComboNum = nextNum;
        }

        public void SetIsInCombat(bool isInCombat)
        {
            m_isInCombat = isInCombat;
            AniM.SetIsInCombatAni(m_isInCombat);
        }

        public float GetCurrentGauge()
        {
           return OnCurrentRangeAttackGauge.Invoke();
        }
        public void InvokeDecreaseGauge(float amount)
        {
            OnDecreaseGauge?.Invoke(amount);
        }
        public void InvokeRegenerateGauge()
        {
            OnRegenrateGauge?.Invoke();
        }
        public void InvokeResetTimer()
        {
            OnResetTimer?.Invoke();
        }
        #endregion ======================================== /ATTACK && INCOMBAT

        #region ======================================== SKILL



        #endregion ======================================== /SKILL
        public void SetIKRigWeight(RigType rigType, bool isWeight)
        {
            //m_playerCore.IKController.SetRigWeight(rigType, isWeight);
        }
        /// <summary>
        /// 실제 데미지를 적용하는 메서드 (거리 체크 통과 후 호출)
        /// </summary>
        public void ApplyDamage(RaycastHit hit)
        {
            if (hit.collider.TryGetComponent<HitBox>(out HitBox _hitBox))
            {
                DamageMassage _damageMassage = new DamageMassage();
                //_damageMassage.Damager = damager;
                _damageMassage.HitNormal = hit.normal;
                _damageMassage.HitPoint = hit.point;
                //RangeWeaponItemDataSO _range = CurrentWeapon as RangeWeaponItemDataSO;
                //_damageMassage.Damage = _range.WeaponData.CombatData.Damage;

                _hitBox.damageable.ApplyDamage(_damageMassage);
                print("히트박스 데미지 완료");

            }
            // TODO: 피격 이펙트 재생 등

        }

        /// <summary>
        /// 거리 내 맞은 대상이 있는지 확인 (단순 체크용)
        /// </summary>
        public bool TryGetTarget(out RaycastHit hit, float maxDistance)
        {
            Vector3 origin = Camera.main.transform.position;
            Vector3 dir = Camera.main.transform.forward;

            bool isHit = Physics.Raycast(origin, dir, out hit, maxDistance, 1 << LayerMask.NameToLayer("Enemy"));

            return isHit;
        }

        public void SetColorMarkCrossHeadUI(bool isDistance)
        {
            //m_playerCore.RealTimeUIM.SetColorMarkCrossHead(isDistance);
            //RealTimeUIManager.Instance.SetColorMarkCrossHead(isDistance);
        }
        /// <summary>
        /// Player오브젝트 하위에 있는 각 Holder 오브젝트 On/Off 방식
        /// TODO 스왑시 스왑상태에서 시간에 의해 애니메이션 Num값과 실제 Swap값이 다르게 가~끔나옴 해결필요
        /// </summary>

        public void SetAming(bool isOffAiming = false)
        {
            //m_isAiming = !m_isAiming;
            //if (isOffAiming) m_isAiming = false;

            //RealTimeUIManager.Instance.ChangeSniperAimUI(m_isAiming ? (CurrentWeaponNum == 3 ? true : false) : false);
            //m_playerCore.CameraManger.AimFOV(m_isAiming, CurrentWeaponNum == 3);
        }

    }
}
