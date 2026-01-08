using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    InputHandler inputHandler;
    Rigidbody rb;
    Transform cameraObject; // Potrebujeme vedieť, kde je kamera

    [Header("Movement Stats")]
    public float movementSpeed = 5f;
    public float rotationSpeed = 10f;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        inputHandler = GetComponent<InputHandler>();
        rb = GetComponent<Rigidbody>();
        
        // Získame referenciu na Main Camera
        cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        // Ak práve prebieha animácia (útok/roll), zastavíme fyzický pohyb
        // Toto zatiaľ nerobí nič, ale je to príprava na neskôr
        if (playerManager.isInteracting)
            return;

        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        // 1. Získame smer pohybu podľa KAMERY
        Vector3 moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        
        // 2. Vyčistíme Y os (aby sme nechceli ísť do zeme alebo do neba)
        moveDirection.Normalize();
        moveDirection.y = 0;

        // 3. Aplikujeme rýchlosť
        Vector3 movementVelocity = moveDirection * movementSpeed;

        // 4. Nastavíme Velocity Rigidbody (zachováme gravitáciu na Y)
        rb.linearVelocity = new Vector3(movementVelocity.x, rb.linearVelocity.y, movementVelocity.z);
    }

    private void HandleRotation()
    {
        Vector3 targetDir = Vector3.zero;

        // Opäť vypočítame smer podľa kamery
        targetDir = cameraObject.forward * inputHandler.vertical;
        targetDir += cameraObject.right * inputHandler.horizontal;
        targetDir.Normalize();
        targetDir.y = 0;

        // Ak sa nehýbeme (input je 0), nechceme sa otočiť späť na nulu, ostaneme tak ako sme
        if (targetDir == Vector3.zero)
            targetDir = transform.forward;

        // Plynulá rotácia (Slerp) k novému smeru
        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

        transform.rotation = targetRotation;
    }
}