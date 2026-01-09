using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public PlayerControls inputActions; // Odkaz na vygenerovanú triedu

    [Header("Movement Input")]
    public Vector2 movementInput; // Surové dáta (x, y)
    public float vertical;        // Dopredu/Dozadu
    public float horizontal;      // Vlavo/Vpravo
    public float moveAmount;      // 0 až 1 (dôležité pre animácie)

    [Header("Camera Input")]
    public float mouseX;
    public float mouseY;

    [Header("Action Input")]
    public bool sprint_Input;
    public bool lockOn_Input;

    // Tato funkcia sa zavola, ked sa objekt (Player) aktivuje
    private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();

            // Movement
            inputActions.Player.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            inputActions.Player.Movement.canceled += i => movementInput = Vector2.zero;

            // Camera
            inputActions.Player.Camera.performed += i =>
            {
                Vector2 cameraInput = i.ReadValue<Vector2>();
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            };
            inputActions.Player.Camera.canceled += i =>
            {
                mouseX = 0;
                mouseY = 0;
            };

            // Sprint
            inputActions.Player.Sprint.performed += i => sprint_Input = true;
            inputActions.Player.Sprint.canceled += i => sprint_Input = false;

            // Lock-On
            inputActions.Player.LockOn.performed += i => lockOn_Input = true;
        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        MoveInput(delta);

    }

    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
    }
}