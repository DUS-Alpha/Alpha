using UnityEngine;

public class PlayerWeaponHolder : MonoBehaviour
{
    [SerializeField]
    private Transform m_rightHandSwordAttach;
    [SerializeField]
    private Transform m_rightHandGunAttach;

    private GameObject m_currentWeaponObj;

    public void EquipWeapon(Weapon weapon)
    {
        // 기존 무기 제거
        if (m_currentWeaponObj != null)
            Destroy(m_currentWeaponObj);

        // 무기 프리팹 Instantiate
        if (weapon.WeaponData.ItemPrefab != null)
        {
            Transform _attach = null;

            switch (weapon.WeaponData.WeaponType)
            {
                case WeaponType.Melee:
                    _attach = m_rightHandSwordAttach;
                    break;
                case WeaponType.Range:
                    _attach = m_rightHandGunAttach;
                    break;
            }
            m_currentWeaponObj = Instantiate(weapon.WeaponData.ItemPrefab, _attach);
            m_currentWeaponObj.transform.localPosition = Vector3.zero;
            m_currentWeaponObj.transform.localRotation = Quaternion.identity;
        }
    }


}
