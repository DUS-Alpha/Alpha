using System;
using UnityEngine;

namespace alpha
{
    public enum GaugeTpyes
    {
        None,
        Action,
        RangeWeapon
    }

    public struct GaugeMappings
    {
        public GaugeTpyes GaugeType;
        public float Gaugevalue;
    }
    public class PlayerGaugeManager : MonoBehaviour
    {
        [SerializeField] RealTimeUIManager m_realTimeUIM;
        
        private PlayerCore m_playerCore;

        [Header("[ Gauge Settings ]")]
        [SerializeField] private GaugeMappings[] m_gauges;

        [Header("[ ActionGauge ]")]
        [SerializeField] private int m_maxAcionGauge = 100;
        [Tooltip("회복량"), SerializeField] private float m_actionGaugeRegenarationAmount = 0.5f;
        [Tooltip("회복시작딜레이"), SerializeField] private float m_actionGaugeRegenarationDelay = 1;

        private float m_actionGaugeRegenerationTimer = 0;

        public float CurrentActionGauge { get; private set; }
        public bool IsActionGaugeZero { get; private set; }

        [Header("[ RangeWeaonGauge ]")]
        [SerializeField] private int m_maxRangeWeaonGauge = 100;
        [Tooltip("회복량"), SerializeField] private float m_rangeWeaonGaugeRegenarationAmount = 0.05f;
        [Tooltip("회복시작딜레이"), SerializeField] private float m_rangeWeaonGaugeRegenarationDelay = 1;

        private float m_rangeWeaonRegenerationTimer = 0;

        public float CurrentRangeWeaonGauge { get; private set; }
        private bool m_isRangeWeaonGaugeZero;

        public event Action OnShuitGauge;
        public event Action OnCombatGauge;

        public void InitializeModule(PlayerCore playerCore)
        {
            m_playerCore = playerCore;
        }

        private void Start()
        {
            m_playerCore.LocomotionManager.OnDecreaseGauge += HandleDecreaseActionGauge;
            //m_playerCore.Locomotion.OnRegenrateGauge += RegenrateActionGauge;
            //m_playerCore.Locomotion.OnResetTimer += ResetRegenerationActionTimer;

            m_playerCore.CombatManager.OnDecreaseGauge += HandleDecreaseRangeWeaponGauge;
            m_playerCore.CombatManager.OnRegenrateGauge += RegenrateRangeWeaponGauge;
            m_playerCore.CombatManager.OnResetTimer += ResetRegenerationRangeWeaponTimer;
            m_playerCore.CombatManager.OnCurrentRangeAttackGauge += CurrentRangeWeaponGauge;

            m_realTimeUIM.SetGague(SetMaxActionGauge(0)/100, GaugeTpyes.Action);
            m_realTimeUIM.SetGague(SetMaxRangeWeaponGauge(0) / 100, GaugeTpyes.RangeWeapon);
        }
        private void Update()
        {
            m_realTimeUIM.SetGague(CurrentActionGauge/100, GaugeTpyes.Action);
            m_realTimeUIM.SetGague(CurrentRangeWeaonGauge / 100, GaugeTpyes.RangeWeapon);

            
        }

        #region ======================================== ACTION
        // 레벨에 따른 스테미나 계산
        public int SetMaxActionGauge(int AddGauge)
        {
            int _stamina = 0;

            _stamina = AddGauge + m_maxAcionGauge;
            CurrentActionGauge = _stamina;
            
            return _stamina;
        }

        public void HandleDecreaseActionGauge(float decreaseGauge)
        {
            if (CurrentActionGauge > 0)
            {
                CurrentActionGauge -= decreaseGauge;
                IsActionGaugeZero = false;
            }
            else
            {
                CurrentActionGauge = 0;
                IsActionGaugeZero = true;
            }
            
            m_realTimeUIM.SetGague(CurrentActionGauge/100, GaugeTpyes.Action);
        }

        // 게이지 회복
        public void RegenrateActionGauge()
        {
            if (m_playerCore.IsPerformingAction) return;

            m_actionGaugeRegenerationTimer += Time.deltaTime;

            // 액션 동작 이후 회복 시작 시간 딜레이
            if(m_actionGaugeRegenerationTimer >= m_actionGaugeRegenarationDelay)
            {
                if(CurrentActionGauge < m_maxAcionGauge)
                    CurrentActionGauge += m_actionGaugeRegenarationAmount;
                if (CurrentActionGauge >= m_maxAcionGauge)
                    CurrentActionGauge = m_maxAcionGauge;
            }
            m_realTimeUIM.SetGague(CurrentActionGauge/100, GaugeTpyes.Action);
        }

        // 타이머 리셋시키기
        public void ResetRegenerationActionTimer()
        {
            m_actionGaugeRegenerationTimer = 0;
        }
        #endregion ======================================== /ACTION

        #region ======================================== RANGEWEAPON

        private float CurrentRangeWeaponGauge()
        {
            return CurrentRangeWeaonGauge;
        }
        // 레벨에 따른 스테미나 계산
        public int SetMaxRangeWeaponGauge(int AddGauge)
        {
            int _stamina = 0;

            _stamina = AddGauge + m_maxRangeWeaonGauge;
            CurrentRangeWeaonGauge = _stamina;

            return _stamina;
        }

        public void HandleDecreaseRangeWeaponGauge(float decreaseGauge)
        {
            if (CurrentRangeWeaonGauge > 0)
            {
                CurrentRangeWeaonGauge -= decreaseGauge;
                m_isRangeWeaonGaugeZero = false;
            }
            else
            {
                CurrentRangeWeaonGauge = 0;
                m_isRangeWeaonGaugeZero = true;
            }

            m_realTimeUIM.SetGague(CurrentRangeWeaonGauge / 100, GaugeTpyes.RangeWeapon);
        }

        // 게이지 회복
        public void RegenrateRangeWeaponGauge()
        {
            if (m_playerCore.IsPerformingAction) return;

            m_rangeWeaonRegenerationTimer += Time.deltaTime;

            // 액션 동작 이후 회복 시작 시간 딜레이
            if (m_rangeWeaonRegenerationTimer >= m_rangeWeaonGaugeRegenarationDelay)
            {
                if (CurrentRangeWeaonGauge < m_maxRangeWeaonGauge)
                    CurrentRangeWeaonGauge += m_rangeWeaonGaugeRegenarationAmount;
                if (CurrentRangeWeaonGauge >= m_maxRangeWeaonGauge)
                    CurrentRangeWeaonGauge = m_maxRangeWeaonGauge;
            }
            m_realTimeUIM.SetGague(CurrentRangeWeaonGauge / 100, GaugeTpyes.RangeWeapon);
        }

        // 타이머 리셋시키기
        public void ResetRegenerationRangeWeaponTimer()
        {
            m_rangeWeaonRegenerationTimer = 0;
        }
        #endregion ======================================== /RANGEWEAPON


    }
}