using UnityEngine;

public class WeaponHolderSlot : MonoBehaviour
{
    public Transform parentOverride;
    public bool isLeftHandSlot;
    public bool isRightHandSlot;

    public GameObject currentWeaponModel;

    public void LoadWeaponModel(WeaponItem weaponItem)
    {
        // Destroy old weapon if alreadt equipec
        UnloadWeapon();

        if (weaponItem == null)
        {
            UnloadWeapon();
            return;
        }

        // Spawn model into correct position
        GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
        if (model != null)
        {
            if (parentOverride != null)
            {
                model.transform.parent = parentOverride;
            }
            else
            {
                model.transform.parent = transform;
            }

            // Reset position based on hand
            model.transform.localPosition = weaponItem.modelPosition;
            model.transform.localRotation = Quaternion.Euler(weaponItem.modelRotation);
            model.transform.localScale = weaponItem.modelScale;
        }

        currentWeaponModel = model;
    }

    public void UnloadWeapon()
    {
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }
}