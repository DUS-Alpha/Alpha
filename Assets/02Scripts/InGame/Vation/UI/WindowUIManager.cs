using System;
using System.Collections.Generic;
using UnityEngine;

namespace alpha
{
    public enum EWindowUITypes
    {
        None,
        Option,
        Inventory,
        Pause,
        Shop,
    }
    public class WindowUIManager : MonoBehaviour
    {
        public static WindowUIManager Instance;

        [SerializeField]
        private BaseUI[] m_baseUIs;

        private Dictionary<EWindowUITypes, BaseUI> m_uiDict = new();

        private BaseUI m_currentWindowUI;
        private readonly HashSet<EWindowUITypes> m_openedWindows = new(); // 현재 열려있는 UI들
        public bool IsWindowUI => m_openedWindows.Count > 0;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

                m_baseUIs = GetComponentsInChildren<BaseUI>();

            // BaseUI 배열에서 각 UI를 Enum 기반으로 등록
            foreach (var ui in m_baseUIs)
            {
                if (ui == null) continue;

                // BaseUI가 어떤 UI인지 지정할 수 있도록 Enum 프로퍼티 추가 필요 (아래 참고)
                if (!m_uiDict.ContainsKey(ui.UIType))
                    m_uiDict.Add(ui.UIType, ui);
            }
        }

        private void Start()
        {
            foreach (var ui in m_baseUIs)
            {
                ui.CloseUI();
            }
        }

        private void Update()
        {
            CheckInput();
        }

        private void CheckInput()
        {
            var uiInput = UIInputManager.Instance;
            if (uiInput == null) return;

            if (uiInput.IsOptionMenu)
                ToggleWindowUI(EWindowUITypes.Option);

            if (uiInput.IsInventory)
                ToggleWindowUI(EWindowUITypes.Inventory);
        }

        /// <summary>
        /// 특정 UI를 열거나 닫음 (토글)
        /// </summary>
        public void ToggleWindowUI(EWindowUITypes type)
        {
            if (m_openedWindows.Contains(type))
                CloseWindowUI(type);
            else
                OpenWindowUI(type);
        }
        /// <summary>
        /// UI 열기
        /// </summary>
        public void OpenWindowUI(EWindowUITypes type)
        {
            if (!m_uiDict.TryGetValue(type, out var targetUI) || targetUI == null)
                return;

            if (!m_openedWindows.Contains(type))
            {
                m_openedWindows.Add(type);
                targetUI.ShowUI();
                UpdateCursorState();
            }
        }

        /// <summary>
        /// UI 닫기
        /// </summary>
        public void CloseWindowUI(EWindowUITypes type)
        {
            if (!m_uiDict.TryGetValue(type, out var targetUI) || targetUI == null)
                return;

            if (m_openedWindows.Contains(type))
            {
                targetUI.CloseUI();
                m_openedWindows.Remove(type);
                UpdateCursorState();
            }
        }

        /// <summary>
        /// 전체 닫기 (씬 전환 시 등)
        /// </summary>
        public void CloseAllWindowUIs()
        {
            foreach (var ui in m_uiDict.Values)
                ui?.CloseUI(true);

            m_currentWindowUI = null;
            WorldCameraManager.Instance.SetCursor(false);
        }
        /// <summary>
        /// 열려 있는 창 개수에 따라 커서 상태 변경
        /// </summary>
        private void UpdateCursorState()
        {
            bool hasAnyOpen = m_openedWindows.Count > 0;
            WorldCameraManager.Instance.SetCursor(hasAnyOpen);
        }

        // Button 용
        public void CloseOption()
        {
            ToggleWindowUI(EWindowUITypes.Option);
        }
        public void CloseInventory()
        {
            ToggleWindowUI(EWindowUITypes.Inventory);
        }
    }
}