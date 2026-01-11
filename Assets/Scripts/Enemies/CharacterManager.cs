using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Lock On Target Point")]
    public Transform lockOnTransform;

    private void Awake()
    {
        // If no specific lock-on point is assigned, use the character's own transform
        if (lockOnTransform == null)
            lockOnTransform = transform;
    }
}