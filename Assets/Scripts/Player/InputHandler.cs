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
    public bool b_Input;          // Roll/Sprint tlačidlo

    // Tato funkcia sa zavola, ked sa objekt (Player) aktivuje
    private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();

            // 1. POHYB (Movement)
            // Keď stlačíš/pohneš:
            inputActions.Player.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            // NOVÉ: Keď pustíš (vráti sa do nuly):
            inputActions.Player.Movement.canceled += i => movementInput = Vector2.zero;

            // 2. KAMERA (Camera)
            // Keď hýbeš myšou/páčkou:
            inputActions.Player.Camera.performed += i => 
            {
                Vector2 cameraInput = i.ReadValue<Vector2>();
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            };
            // NOVÉ: Keď prestaneš hýbať (nutné hlavne pre gamepady, myš sa zastaví sama):
            inputActions.Player.Camera.canceled += i => 
            {
                mouseX = 0;
                mouseY = 0;
            };
            
            // 3. Roll / Sprint Input
            // inputActions.Player.BInput.performed += i => b_Input = true;
            // inputActions.Player.BInput.canceled += i => b_Input = false;
        }

        inputActions.Enable();
    }

    // Ked objekt vypneme, musíme vypnúť aj inputy, aby nevznikali chyby
    private void OnDisable()
    {
        inputActions.Disable();
    }

    // Túto funkciu budeme volať z Update slučky v PlayerManageri
    public void TickInput(float delta)
    {
        MoveInput(delta);
        // Tu neskôr pridáme HandleAttackInput, HandleRollInput atď.
    }

    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;

        // Vypočítame "moveAmount" (či sa hýbeme trošku alebo naplno)
        // Toto budeme posielať do Animátora
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
    }
}