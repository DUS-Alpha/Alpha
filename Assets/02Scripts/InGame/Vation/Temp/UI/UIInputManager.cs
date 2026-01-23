using System;
using UnityEngine;

public class UIInputManager : MonoBehaviour
{
    public static UIInputManager Instance;

    private UIInputControls m_uiControls;
    
    private int m_inOptionFrame;
    private int m_inInventoryFrame;
    public bool IsInventory => m_inInventoryFrame == Time.frameCount;
    public bool IsOptionMenu => m_inOptionFrame == Time.frameCount;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        if (m_uiControls == null)
        {
            m_uiControls = new UIInputControls();
            //  Time.frameCount 다음 프레임에서 false(입력 확인후 false를 시키려고하니 Update에서 같은 프레임이라 다음 프레임에서 false로)
            m_uiControls.Window.Option.performed += i => m_inOptionFrame = Time.frameCount;
            m_uiControls.Window.Inventory.performed += i => m_inInventoryFrame = Time.frameCount;
        }
        m_uiControls.Enable();   //Enable해야 m_playerControl의 인풋시스템 입력처리가 활성화됨
    }

    private void Update()
    {

    }
}
