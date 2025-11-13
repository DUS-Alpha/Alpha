using UnityEngine;

namespace alpha
{
    public class WorldCameraManager : MonoBehaviour
    {
        public static WorldCameraManager Instance;
        public bool IsCursor { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        public void SetCursor(bool isCursor)
        {
            if (isCursor)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;

                Cursor.visible = false;
            }

            IsCursor = isCursor;
        }
    }
}