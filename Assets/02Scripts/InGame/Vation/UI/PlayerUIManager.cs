using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct CrossHairUI
{
    public Image[] Icons;
}

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;

    [SerializeField]
    private Image[] m_crossHairIcon;
    [SerializeField]
    private Image[] m_sniperUI;

    [SerializeField]
    private PlayerInventoryUI m_inventoryUI;

    [SerializeField]
    private TextMeshProUGUI m_ammoTMP;

    [SerializeField]
    private TextMeshProUGUI m_locomotionTMP;
    [SerializeField]
    private TextMeshProUGUI m_combatTMP;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ChangeSniperAimUI(false);
        SetAmmo(0, 0, 0);
    }
    private BaseUI GetUI<T>(out bool isAlreadyOpen) //out : 참조역할 이를 통해 BaseUI와 bool 두 타입을 반환
    {
        Type uiType = typeof(T);
        BaseUI _ui = null;

        isAlreadyOpen = false;

        var _uiObj = m_inventoryUI;
        _ui = _uiObj.GetComponent<BaseUI>();
        return _ui;
    }

    // InputManager의 해당 키를 통해 동작
    public void OpenUI<T>(BaseUIData uiData)
    {
        Type _uiType = typeof(T); //받아온 UI타입 저장

        bool _isAlreadyOpen = false;
        var _ui = GetUI<T>(out _isAlreadyOpen);

        _ui.SetInfo(uiData);
    }

    public void CloseUI(BaseUI ui)
    {

    }
    public void ChangeSniperAimUI(bool isSniper)
    {
        m_crossHairIcon[0].gameObject.SetActive(!isSniper);
        m_sniperUI[0].gameObject.SetActive(isSniper);
    }

    public void SetColorMarkCrossHead(bool isDistance, bool isSniper=false)
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

    public void EquipInventory(EquipmentDataSO equipmentData)
    {
        m_inventoryUI.Equip(equipmentData);
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
}
