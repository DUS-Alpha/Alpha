using System;
using UnityEngine;


public class BaseUIData
{
    public Action OnShow;
    public Action OnClose;
}
public class BaseUI : MonoBehaviour
{
    public Animation m_UIOpenAnim;

    private Action m_OnShow;
    private Action m_OnClose;
    public virtual void Init()
    {
        m_OnShow = null;
        m_OnClose = null;
    }

    public virtual void SetInfo(BaseUIData uiData)
    {

    }
    public virtual void ShowUI()
    {
        if (m_UIOpenAnim)
        {
            m_UIOpenAnim.Play();
        }

        m_OnShow?.Invoke();
        m_OnShow = null;
    }
    public virtual void CloseUI(bool isCloseAll = false)
    {
        // 씬 전환 시 열려있는 화면을 전부 다 닫아줘야할 때
        if (!isCloseAll)
        {
            m_OnClose?.Invoke();
        }
        m_OnClose = null;

        PlayerUIManager.Instance.CloseUI(this);
    }

    public virtual void OnClickCloseButton()
    {
        //AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        CloseUI();
    }
}
