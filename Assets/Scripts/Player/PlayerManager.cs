using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerLocomotion playerLocomotion;

    [Header("Player Flags")]
    public bool isInteracting; // Kľúčová premenná pre Souls-like (ak true, nemôžeš sa hýbať)

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        // Každý frame čítame inputy
        inputHandler.TickInput(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        // Fyzikálny pohyb riešime vo FixedUpdate
        playerLocomotion.HandleAllMovement();
    }
}