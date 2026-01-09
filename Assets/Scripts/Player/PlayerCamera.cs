using UnityEngine;
using Unity.Cinemachine; // Nový namespace pre Unity 6

public class PlayerCamera : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerManager playerManager;

    // ZMENA 1: Už nie CinemachineFreeLook, ale CinemachineCamera
    public CinemachineCamera cmCamera;

    // Potrebujeme prístup k týmto komponentom na kamere:
    private CinemachineInputAxisController _inputAxisController; // Vypína myš
    private CinemachineOrbitalFollow _orbitalFollow; // Ovláda rotáciu (náhrada za m_XAxis)

    [Header("Lock On Settings")]
    public float lockOnRadius = 20f;
    public LayerMask targetLayers;
    public Transform currentLockOnTarget;
    public Transform cameraLookTarget;

    [Header("Lock On Movement")]
    public float followSpeed = 5f;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        playerManager = GetComponent<PlayerManager>();

        // ZMENA 2: Hľadáme CinemachineCamera
        if (cmCamera == null)
            cmCamera = FindFirstObjectByType<CinemachineCamera>(); // V Unity 6 je FindFirstObjectByType rýchlejšie ako FindObjectOfType

        if (cmCamera != null)
        {
            _inputAxisController = cmCamera.GetComponent<CinemachineInputAxisController>();

            // ZMENA 3: Získame Orbital Follow (to je to, čo robí FreeLook správanie)
            _orbitalFollow = cmCamera.GetComponent<CinemachineOrbitalFollow>();
        }

        cameraLookTarget = transform.Find("CameraTarget");
    }

    public void HandleAllCameraMovement()
    {
        if (inputHandler.lockOn_Input)
        {
            inputHandler.lockOn_Input = false;
            HandleLockOn();
        }

        if (cmCamera != null)
        {
            if (playerManager.isLockedOn && currentLockOnTarget != null)
            {
                // 1. Look At - pozeraj na nepriateľa
                cmCamera.LookAt = currentLockOnTarget;

                // 2. Vypni Input Controller (myš)
                if (_inputAxisController != null)
                    _inputAxisController.enabled = false;

                // 3. AUTO ROTÁCIA (Fix vybiehania zo záberu)
                if (_orbitalFollow != null)
                {
                    Vector3 dirToTarget = currentLockOnTarget.position - transform.position;
                    dirToTarget.Normalize();
                    dirToTarget.y = 0;

                    if (dirToTarget != Vector3.zero)
                    {
                        float targetAngle = Mathf.Atan2(dirToTarget.x, dirToTarget.z) * Mathf.Rad2Deg;

                        // ZMENA 4: Ovládame HorizontalAxis na OrbitalFollow komponente
                        float currentAngle = _orbitalFollow.HorizontalAxis.Value;
                        float smoothAngle = Mathf.LerpAngle(currentAngle, targetAngle, followSpeed * Time.deltaTime);

                        _orbitalFollow.HorizontalAxis.Value = smoothAngle;
                    }
                }
            }
            else
            {
                // Odomknuté
                cmCamera.LookAt = cameraLookTarget;

                if (_inputAxisController != null)
                    _inputAxisController.enabled = true;
            }
        }
    }

    private void HandleLockOn()
    {
        if (playerManager.isLockedOn)
        {
            playerManager.isLockedOn = false;
            currentLockOnTarget = null;
            playerManager.isStrafing = false;
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, lockOnRadius, targetLayers);
        float shortestDistance = Mathf.Infinity;
        Transform nearestTarget = null;

        foreach (var collider in colliders)
        {
            CharacterManager character = collider.GetComponent<CharacterManager>();
            if (character != null)
            {
                if (character.transform.root == transform.root) continue;

                float distanceFromTarget = Vector3.Distance(transform.position, character.transform.position);

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestTarget = character.lockOnTransform;
                }
            }
        }

        if (nearestTarget != null)
        {
            currentLockOnTarget = nearestTarget;
            playerManager.isLockedOn = true;
            playerManager.isStrafing = true;
        }
    }
}