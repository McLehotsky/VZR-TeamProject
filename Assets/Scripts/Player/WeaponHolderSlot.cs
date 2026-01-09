using UnityEngine;

public class WeaponHolderSlot : MonoBehaviour
{
    public Transform parentOverride; // Ak chceme upravit poziciu
    public bool isLeftHandSlot;
    public bool isRightHandSlot;

    public GameObject currentWeaponModel;

    public void LoadWeaponModel(WeaponItem weaponItem)
    {
        // 1. Znicime staru zbran ak nejaka je
        UnloadWeapon();

        if (weaponItem == null)
        {
            UnloadWeapon();
            return;
        }

        // 2. Spawneme novy model
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

            // Resetneme poziciu a rotaciu voci ruke
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;
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