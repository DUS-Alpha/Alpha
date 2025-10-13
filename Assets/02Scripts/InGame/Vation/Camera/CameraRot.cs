using UnityEngine;

public class CameraRot : MonoBehaviour
{
    [Header("Camera Target")]
    [SerializeField] 
    private Transform m_cameraRoot;   // 회전 중심
    [SerializeField]
    private float m_yMinRot;
    [SerializeField]
    private float m_yMaxRot;
    [SerializeField] 
    private float m_sensitivity = 100f; //  감도

    private float yaw;   // 좌우 회전
    private float pitch; // 상하 회전

    // Update is called once per frame
    private void OnEnable()
    {
        Vector3 euler = m_cameraRoot.rotation.eulerAngles;
        yaw = euler.y;
        pitch = euler.x;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") ;
        float mouseY = Input.GetAxis("Mouse Y") ;

        yaw += mouseX * m_sensitivity * Time.deltaTime;
        pitch -= mouseY * m_sensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, m_yMinRot, m_yMaxRot);

        m_cameraRoot.rotation = Quaternion.Euler(pitch, yaw, 0f);   // X축에 MouseY값, Y축에 MouseX값
    }
}
