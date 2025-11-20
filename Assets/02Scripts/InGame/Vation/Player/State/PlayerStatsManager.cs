using UnityEngine;

namespace alpha
{
    public enum GaugeTpyes
    {
        None,
        Dash,
        Flight
    }

    public struct GaugeMappings
    {
        public GaugeTpyes GaugeType;
        public float Gaugevalue;
    }
    public class PlayerStatsManager : MonoBehaviour
    {
        private PlayerCore m_playerCore;

        [Header("[ Gauge Settings ]")]
        [SerializeField] 
        private GaugeMappings[] m_gauges;

        public int m_maxAcionGauge;
        public float CurrentActionGauge { get; private set; }

        [SerializeField]
        private float m_actionGaugeRegenarationAmount = 1f;

        private float m_actionGaugeRegenerationTimer = 0;

        [SerializeField]
        private float m_actionGaugeRegenarationDelay = 0f;

        public bool IsActionGaugeZero { get; private set; }
        public void InitializeModule(PlayerCore playerCore)
        {
            m_playerCore = playerCore;
        }

        // 레벨에 따른 스테미나 계산
        public int SetMaxActionGauge(int AddGauge)
        {
            int _stamina = 0;

            _stamina = AddGauge + m_maxAcionGauge;
            CurrentActionGauge = _stamina;
            return _stamina;
        }

        public void DecreaseActionGauge(float decreaseGauge)
        {
            if (CurrentActionGauge > 0)
                CurrentActionGauge -= decreaseGauge * (Time.deltaTime * 100);
            else
            {
                CurrentActionGauge = 0;
                IsActionGaugeZero = true;
            }

            RealTimeUIManager.Instance.ActionGaugeUI(CurrentActionGauge);
        }

        // 스테미나 회복
        public void RegenrateStamina()
        {
            if (m_playerCore.IsPerformingAction) return;

            m_actionGaugeRegenerationTimer += Time.deltaTime;

            // 액션 동작 이후 회복 시작 시간 딜레이
            if(m_actionGaugeRegenerationTimer >= m_actionGaugeRegenarationDelay)
            {
                if(CurrentActionGauge < m_maxAcionGauge)
                    CurrentActionGauge += m_actionGaugeRegenarationAmount * (Time.deltaTime * 100);
                if (CurrentActionGauge >= m_maxAcionGauge)
                    CurrentActionGauge = m_maxAcionGauge;
            }

            RealTimeUIManager.Instance.ActionGaugeUI(CurrentActionGauge);
        }

        // 타이머 리셋시키기
        public void ResetRegenerationTimer()
        {
            m_actionGaugeRegenerationTimer = 0;
        }

    }
}