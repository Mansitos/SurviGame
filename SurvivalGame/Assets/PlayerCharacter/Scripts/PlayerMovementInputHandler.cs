using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementInputHandler : MonoBehaviour
{
    public Vector2 movementInputVector { get; private set; } // Movement vector from the Input System
    public bool isRunning { get; private set; } // Indicates if player is holding the Run action (e.g., Shift)
    public bool isWalking { get; private set; } // Indicates if player is walking
    public Vector2 mouseScreenPosition { get; private set; } // Mouse position on screen

    private PlayerControls controls; // Reference to the auto-generated input actions class
    private GameManager gm;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void Start()
    {
        gm = GameManager.Instance;
    }

    private void OnEnable()
    {
        controls.Player.Enable();

        // Subscribe to movement and look actions
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
        controls.Player.Run.performed += OnRunPerformed;
        controls.Player.Run.canceled += OnRunCanceled;
        controls.Player.Look.performed += OnLookPerformed;
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= OnMovePerformed;
        controls.Player.Move.canceled -= OnMoveCanceled;
        controls.Player.Run.performed -= OnRunPerformed;
        controls.Player.Run.canceled -= OnRunCanceled;
        controls.Player.Look.performed -= OnLookPerformed;

        controls.Player.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        if (gm.IsInNormalMode())
        {
            isWalking = true;
            movementInputVector = ctx.ReadValue<Vector2>();

            // If running button is already held, start running immediately
            if (controls.Player.Run.IsPressed() && movementInputVector.sqrMagnitude > 0.001f)
            {
                isRunning = true;
            }
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        isWalking = false;
        isRunning = false;
        movementInputVector = Vector2.zero;
    }

    private void OnRunPerformed(InputAction.CallbackContext ctx)
    {
        // Set running state but ensure movement vector is non-zero
        isRunning = movementInputVector.sqrMagnitude > 0.001f;
    }

    private void OnRunCanceled(InputAction.CallbackContext ctx)
    {
        isRunning = false;
    }

    private void OnLookPerformed(InputAction.CallbackContext ctx)
    {
        mouseScreenPosition = ctx.ReadValue<Vector2>();
    }

    public bool WasInventoryModePressedThisFrame() => controls.Player.Inventory.WasPressedThisFrame();
    public bool WasMainActionPressedThisFrame() => controls.Player.MainAction.WasPressedThisFrame();
    public bool WasSecondaryActionPressedThisFrame() => controls.Player.SecondaryAction.WasPressedThisFrame();
    public bool WasRotateActionPressedThisFrame() => controls.Player.RotateAction.WasPressedThisFrame();
}
