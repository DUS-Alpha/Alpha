using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct CrossHairUI
{
    public Image[] Icons;
}

public class RealTimeUIManager : MonoBehaviour
{
    public static RealTimeUIManager Instance;

    [SerializeField]
    private Image[] m_crossHairIcon;
    [SerializeField]
    private Image[] m_sniperUI;

    [SerializeField]
    private TextMeshProUGUI m_ammoTMP;

    [SerializeField]
    private TextMeshProUGUI m_locomotionTMP;
    [SerializeField]
    private TextMeshProUGUI m_combatTMP;

    [SerializeField]
    private TextMeshProUGUI m_flyGaugeTMP;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    void Start()
    {
        ChangeSniperAimUI(false);
        SetAmmo(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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

    public void CurrentLocomotionState(string state)
    {
        m_locomotionTMP.text = "Locomotion \n" + state;
    }
    public void CurrentCombatState(string state)
    {
        m_combatTMP.text = "Combat \n" + state;
    }

    public void FlyGaugeUI(float t)
    {
        m_flyGaugeTMP.text = t.ToString();
    }
}
