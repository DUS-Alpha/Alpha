using System;
using UnityEngine;


namespace alpha
{
    public class BaseUIData
    {
        public Action OnShow;
        public Action OnClose;
    }
    public class BaseUI : MonoBehaviour
    {
        public Animation UIOpenAnim;

        public EWindowUITypes UIType;
        private Action m_onShow;
        private Action m_onClose;

        public virtual void Init(Transform anchor, EWindowUITypes type)
        {
            m_onShow = null;
            m_onClose = null;

            UIType = type;
            transform.SetParent(anchor);

            var rectTransform = GetComponent<RectTransform>();
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localScale = Vector3.one;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
        }

        public virtual void SetInfo(BaseUIData uiData)
        {
            // 각 UI클래스에서 액션들을 받아 실행
            m_onShow = uiData.OnShow;
            m_onClose = uiData.OnClose;
        }
        public virtual void ShowUI()
        {
            gameObject.SetActive(true);

            if (UIOpenAnim)
            {
                UIOpenAnim.Play();
            }

            m_onShow?.Invoke();
            m_onShow = null;


            WorldAudioManager.Instance.PlaySFXUI(1, SFX_UITypes.Open);
        }
        public virtual void CloseUI(bool isCloseAll = false)
        {
            // 씬 전환 시 열려있는 화면을 전부 다 닫아줘야할 때
            if (!isCloseAll)
            {
                m_onClose?.Invoke();
            }
            m_onClose = null;
            gameObject.SetActive(false);
            WorldAudioManager.Instance.PlaySFXUI(1, SFX_UITypes.Close);
        }

        public virtual void OnClickCloseButton()
        {
            //AudioManager.Instance.PlaySFX(SFX.ui_button_click);
            CloseUI();
        }
    }
}