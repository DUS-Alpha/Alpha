using UnityEngine;

namespace alpha
{
    public abstract class BaseMenu : MonoBehaviour
    {
        //public BaseMenu CurrentMenu { get; private set; }
        public bool IsOpen { get; private set; }
        protected virtual void Awake()
        {
            //CurrentMenu = this;
        }
        public void OpenMenuUI()
        {
            this.gameObject.SetActive(true);
            this.IsOpen = true;
        }

        public void CloseMenuUI()
        {
            this.gameObject.SetActive(false);
            this.IsOpen = false;
        }
    }
}