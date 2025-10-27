using UnityEngine;

public class WeaponInstantiationSlot : MonoBehaviour
{
    public WeaponSlotTypes WeaponSlotType; 
    public GameObject CurrentWeapon;

    public void UnLoadWeapon()
    {
        if(CurrentWeapon != null)
            Destroy(CurrentWeapon);
    }

    public void LoadWeapon(GameObject weaponModel)
    {
        CurrentWeapon = weaponModel;
        weaponModel.transform.parent = transform;

        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localRotation = Quaternion.identity;
        weaponModel.transform.localScale = Vector3.one;
    }
}
