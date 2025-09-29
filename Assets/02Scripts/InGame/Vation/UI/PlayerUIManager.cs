using System;
using TMPro;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;

    [SerializeField]
    private PlayerInventoryUI m_inventoryUI;

    [SerializeField]
    private TextMeshProUGUI m_ammoTMP;
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

    public void EquipInventory(EquipmentDataSO equipmentData)
    {
        m_inventoryUI.Equip(equipmentData);
    }

    // 현재 Ammo와 SaveAmmo만 표기하면됨
    public void SetAmmo(int currentAmmo, int saveAmmo, int maxAmmo)
    {
        m_ammoTMP.text = currentAmmo + " / " + saveAmmo;
    }
}
