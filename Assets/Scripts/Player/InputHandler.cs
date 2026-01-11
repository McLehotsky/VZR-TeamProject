using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public PlayerControls inputActions;

    [Header("Movement Input")]
    public Vector2 movementInput;
    public float vertical;
    public float horizontal;
    public float moveAmount;      // for animation (0, 1) range

    [Header("Camera Input")]
    public float mouseX;
    public float mouseY;

    [Header("Action Input")]
    public bool sprint_Input;
    public bool lockOn_Input;
    public bool a_Input;

    [Header("Combat Inputs")]
    public bool rb_Input; // Light Attack
    public bool rt_Input; // Heavy Attack

    [Header("Quick Slots Inputs")]
    public bool d_Pad_Right; // Switch to Right Weapon

    // Call when Player activates
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

            // Combat Inputs
            inputActions.Player.RB.performed += i => rb_Input = true;
            inputActions.Player.RT.performed += i => rt_Input = true;

            // Quick Slots Inputs
            inputActions.Player.SwitchRightWeapon.performed += i => d_Pad_Right = true;

            // Interact Input
            inputActions.Player.Interact.performed += i => a_Input = true;
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