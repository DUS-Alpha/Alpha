using System;
using UnityEngine;

namespace alpha
{
    public class PlayerCombatManager : MonoBehaviour, ISwap
    {
        // Ref Component
        private PlayerCore m_playerCore;
        private PlayerInputManager m_inputM;
        private PlayerEquipManager m_equipM;
        public PlayerAnimationManager AniM { get; private set; }
        public PlayerAudioManager AudioM { get; private set; }

        public bool IsCombatLock => m_isCombatLock;
        private bool m_isCombatLock;

        // 특정 동작 진행중(근접공격, 스왑등 애니메이션 혹은 코드상)인지 판단
        public bool IsAction => m_isAction;
        private bool m_isAction;

        public bool CanMove => m_canMove;
        private bool m_canMove;
        // Combat이 동작되는지만 체크하는
        // ==================== Swap
        public bool CanSwap1 => m_canSwap;
        private bool m_canSwap;

        public int CurrentSwapNum => m_currentSwapNum;
        private int m_currentSwapNum;

        public Item CurrentItem => m_currentItem;
        private Item m_currentItem;

        //public WeaponItemDataSO CurrentWeapon;

        // ==================== Attack
        // 공격 버튼 누른상태
        public bool IsAttackBtn => m_isAttacBtn;
        private bool m_isAttacBtn;

        // Melee Attack
        public bool IsNextCombo => m_isNextCombo;
        private bool m_isNextCombo;

        public int NextComboNum => m_nextComboNum;
        private int m_nextComboNum;

        // Range Attack
        public bool IsRangeAttack => m_isRangeAttack;
        private bool m_isRangeAttack;

        public bool IsInCombat => m_isInCombat;
        private bool m_isInCombat;

        public float CurrentGauge => m_currentGauge;
        private float m_currentGauge;
        private PlayerCore playerCore;

        public event Action<int> OnSwap;
        public event Action OnAttack;

        public PlayerCombatManager(PlayerCore playerCore)
        {
            this.playerCore = playerCore;
        }

        public event Action<float> OnDecreaseGauge;
        public event Action OnRegenrateGauge;
        public event Action OnResetTimer;

        public event Func<float> OnCurrentRangeAttackGauge;
        public void InitializeModule(PlayerCore playerCore)
        {
            m_playerCore = playerCore;
            m_inputM = m_playerCore.InputManager;
            m_equipM = m_playerCore.EquipmentManager;
            AniM = m_playerCore.AniManager;
            AudioM = m_playerCore.PlayerAudioManager;
        }

        // 파라미터 Trigger형태는 KeyDown방식으로 최대한 관리
        public void CheckInput()
        {
            if (m_isCombatLock)
            {
                m_isAttacBtn = false;
                m_canSwap = false;
                m_isAction = false;
                return;
            }

            // Swap (같은번호x, 액션중x, 무기nullx)
            if (!m_isAction)
            {
                int _swapNum = m_inputM.SwapNum;
                m_canSwap = m_currentSwapNum != _swapNum && m_equipM.CanSwap(_swapNum);
            }
            else
            {
                m_canSwap = false;
                //m_inputM.SetSwapNum(m_currentSwapNum);
            }

            // Attack
            if (CurrentItem != null)
                m_isAttacBtn = m_inputM.IsAttackBtn;
            else m_isAttacBtn = false;
        }
        private void Start()
        {
            SetCanMove(true);
        }
        private void Update()
        {

        }
        public void SetCanMove(bool canMove)
        {
            m_canMove = canMove;
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
        public bool CanSwap()
        {
            return m_canSwap;
        }
        public void Swap()
        {

        }
        public void EnterSwap()
        {
            // 스왑 막기
            //m_inputM.SetIsSwap(true);
           
            // 1. EquipManager에서 무기 교체
            int _swapNum = m_inputM.SwapNum;
            
            // 2. 애니메이션 동작
            m_playerCore.AniManager.SwapWeaponAni(_swapNum, false);

            // 3. EquipManager -> Combat 정보 전달 (TODO : 애니메이션 속도 고려하여 무기 활성화 딜레이 줄것)
            Item _item = m_equipM.TrySwap(_swapNum);

            // 4. 현재 정보 저장
            m_currentSwapNum = _swapNum;
            m_currentItem = _item;
        }

        public void ExitSwap(bool isFlying)
        {
            //m_inputM.SetIsSwap(false);
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
            m_playerCore.IKController.SetRigWeight(rigType, isWeight);
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

        #region ================================================ Enter, Exit State

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
            m_playerCore.RealTimeUIM.SetColorMarkCrossHead(isDistance);
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


        public void EnterSkill()
        {
            //SkillKey = m_playerCore.InputManager.SkillKey;
            //m_playerCore.AniController.SkillAni(SkillKey);
        }
        public void ExitSkill()
        {
            
        }
        #endregion ================================================ /Enter,Exit State
    }
}
