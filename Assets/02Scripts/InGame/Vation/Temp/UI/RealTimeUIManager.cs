using alpha;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct CrossHairUI
{
    public Image[] Icons;
}

public class RealTimeUIManager : MonoBehaviour, IGauge
{
    [Header("[ StateInfo ]")]
    [SerializeField] private TextMeshProUGUI m_locomotionTMP;
    [SerializeField] private TextMeshProUGUI m_combatTMP;

    [Header("[ Status ]")]
    [SerializeField] private Image m_hpBar;

    [Header("[ Gague ]")]
    [SerializeField] private Image m_actionGauge;
    [SerializeField] private Image m_RangeWeaponGauge;

    [Header("[ Skill ]")]
    [SerializeField] private Image Skill_Q;
    [SerializeField] private Image SkillCoolDown_Q;

    [SerializeField] private Image Skill_E;
    [SerializeField] private Image SkillCoolDown_E;

    [SerializeField] private Image Skill_Z;
    [SerializeField] private Image SkillCoolDown_Z;

    [SerializeField] private Image Skill_C;
    [SerializeField] private Image SkillCoolDown_C;

    [Header("[ CrossHair ]")]
    [SerializeField] private Image[] m_crossHairIcon;
    [SerializeField] private Image[] m_sniperUI;

    [SerializeField] private TextMeshProUGUI m_ammoTMP;

    private void Awake()
    {
        //if(Instance == null)
        //Instance = this;
    }

    void Start()
    {
        ChangeSniperAimUI(false);
        SetAmmo(0, 0, 0);
    }

    #region ======================================== STATE 
    public void CurrentLocomotionState(string state)
    {
        m_locomotionTMP.text = "LocomotionManager \n" + state;
    }
    public void CurrentCombatState(string state)
    {
        m_combatTMP.text = "CombatManager \n" + state;
    }
    #endregion ======================================== /STATE

    #region ======================================== GAUGE

    // TODO : if문이 아닌 DI패턴으로 관리해보기
    public void SetGague(float gauge, GaugeTpyes gaugeTpye)
    {
        if (gaugeTpye == GaugeTpyes.Action)
        {
            m_actionGauge.fillAmount = gauge;
        }
        else if (gaugeTpye == GaugeTpyes.RangeWeapon)
        {
            m_RangeWeaponGauge.fillAmount = gauge;
        }
    }

    #endregion ======================================== /GAUGE

    public void ChangeSniperAimUI(bool isSniper)
    {
        m_crossHairIcon[0].gameObject.SetActive(!isSniper);
        m_sniperUI[0].gameObject.SetActive(isSniper);
    }

    public void SetColorMarkCrossHead(bool isDistance, bool isSniper = false)
    {
        Image[] _targetImage;
        if (isSniper)
            _targetImage = m_sniperUI;
        else
            _targetImage = m_crossHairIcon;

        for (int i = 0; i < _targetImage.Length; i++)
        {
            Color _color = _targetImage[i].color;
            _color = isDistance ? Color.green : Color.white;
            _targetImage[i].color = _color;
        }
    }

    // 현재 Ammo와 SaveAmmo만 표기하면됨
    public void SetAmmo(int currentAmmo, int saveAmmo, int maxAmmo)
    {
        m_ammoTMP.text = currentAmmo + " / " + saveAmmo;
    }
}
