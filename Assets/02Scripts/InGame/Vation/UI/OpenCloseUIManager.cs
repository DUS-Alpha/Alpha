using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenCloseUIManager : MonoBehaviour
{
    public static OpenCloseUIManager Instance;

    public Transform UICanvaTr;
    public Transform ClosedUITr;

    private BaseUI m_FrontUI;
    
    // Open,Close UI ObjectPooling
    private Dictionary<System.Type, GameObject> m_OpenUIPool = new Dictionary<System.Type, GameObject>();
    private Dictionary<System.Type, GameObject> m_CloseUIPool = new Dictionary<System.Type, GameObject>();


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {

    }

    #region ================================================================================ MainFunction
    private BaseUI GetUI<T>(out bool isAlreadyOpen)     // T : 열고자할 UI, out : 참조역할 이를 통해 BaseUI와 bool 두 타입을 반환
    {
        Type _uiType = typeof(T);

        BaseUI _ui = null;
        isAlreadyOpen = false;


        if (m_OpenUIPool.ContainsKey(_uiType))
        {
            _ui = m_OpenUIPool[_uiType].GetComponent<BaseUI>();
            isAlreadyOpen = true;
        }
        else if (m_CloseUIPool.ContainsKey(_uiType))
        {
            _ui = m_CloseUIPool[_uiType].GetComponent<BaseUI>();
            m_CloseUIPool.Remove(_uiType);
        }
        else // 한번도 생성한적이 없다면 새로 생성
        {
            var _uiObj = Instantiate(Resources.Load($"UI/{_uiType}", typeof(GameObject))) as GameObject;
            _ui = _uiObj.GetComponent<BaseUI>();
        }
        return _ui;
    }

    public void OpenUI<T>(BaseUIData uiData)
    {
        Type _uiType = typeof(T); //받아온 UI타입 저장

        bool _isAlreadyOpen = false;
        var _ui = GetUI<T>(out _isAlreadyOpen);

        if (_ui == null)
        {
            Debug.LogError($"{_uiType}는 존재하지 않습니다.");
            return;
        }
        if(_isAlreadyOpen)
        {
            Debug.LogError($"{_uiType}는 이미 열려있습니다");
            return;
        }

        var _siblingIndex = UICanvaTr.childCount;
        _ui.Init(UICanvaTr);                            // 위치시킬 상단 Tr
        _ui.transform.SetSiblingIndex(_siblingIndex);   // 자식개수가 곧 SiblingIndex 마지막 인덱스 
        _ui.gameObject.SetActive(true);
        _ui.SetInfo(uiData);
        _ui.ShowUI();

        m_FrontUI = _ui;    // 가장 상단의 UI
        m_OpenUIPool[_uiType] = _ui.gameObject;
    }

    public void CloseUI(BaseUI ui)
    {
        Type _uiType = ui.GetType();

        ui.gameObject.SetActive(false);
        m_OpenUIPool.Remove(_uiType);
        m_CloseUIPool[_uiType] = ui.gameObject;
        ui.transform.SetParent(ClosedUITr);

        m_FrontUI = null;
        // 가장 마지막에 열린 ui가 맨앞이기에
        var _lastChild = UICanvaTr.GetChild(UICanvaTr.childCount - 1);
        if(_lastChild)
        {
            m_FrontUI = _lastChild.gameObject.GetComponent<BaseUI>();
        }
    }
    #endregion ================================================================================ /MainFunction

    #region ================================================================================ SubFunction
    // 특정 UI의 Open 확인 후 가져오기
    public BaseUI GetActiveUI<T>()
    {
        var _uiType = typeof(T);
        return m_OpenUIPool.ContainsKey(_uiType) ? m_OpenUIPool[_uiType].GetComponent<BaseUI>() : null;
    }

    // 열림 UI 존재 유무
    public bool ExistsOpenUI()
    {
        return m_FrontUI != null;
    }

    public BaseUI GetCurrentFrontUI()
    {
        return m_FrontUI;
    }

    // 최상단 UI 닫기
    public void CloseCurrentFrontUI()
    {
        m_FrontUI.CloseUI();
    }

    public void CloseAllOpenUI()
    {
        while(m_FrontUI)
        {
            m_FrontUI.CloseUI(false);
        }
    }
    #endregion ================================================================================ /SubFunction

    
}
