using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    InputHandler inputHandler;
    Rigidbody rb;
    Transform cameraObject;

    [Header("Movement Stats")]
    [SerializeField] float walkingSpeed = 2f;
    [SerializeField] float runningSpeed = 5f;
    [SerializeField] float sprintingSpeed = 8f;
    [SerializeField] float rotationSpeed = 10f;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        inputHandler = GetComponent<InputHandler>();
        rb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        if (playerManager.isInteracting)
            return;

        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        if (inputHandler.moveAmount == 0)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            return;
        }

        // Movement direction
        Vector3 moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        // Determine speed
        float targetSpeed = 0f;

        if (playerManager.isSprinting)
        {
            targetSpeed = sprintingSpeed;
        }
        else
        {
            // Walking speed only able with gamepad
            if (inputHandler.moveAmount < 0.55f)
            {
                targetSpeed = walkingSpeed;
            }
            else
            {
                targetSpeed = runningSpeed;
            }
        }

        Vector3 movementVelocity = moveDirection * targetSpeed;
        rb.linearVelocity = new Vector3(movementVelocity.x, rb.linearVelocity.y, movementVelocity.z);
    }

    private void HandleRotation()
    {
        // Strafing / Lock on
        if (playerManager.isStrafing)
        {
            // Lock on mode we are looking towards target 
            Vector3 targetDir = cameraObject.forward;
            targetDir.y = 0;
            targetDir.Normalize();

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
        // Free Roam
        else
        {
            // Reseting target direction
            Vector3 targetDir = Vector3.zero;

            targetDir = cameraObject.forward * inputHandler.vertical;
            targetDir += cameraObject.right * inputHandler.horizontal;
            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = transform.forward;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

            transform.rotation = targetRotation;
        }
    }
}