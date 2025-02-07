using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementInputHandler : MonoBehaviour
{
    public Vector2 movementInputVector { get; private set; } // Movement vector from the Input System
    public bool isRunning { get; private set; } // Indicates if player is holding the Run action (e.g., Shift)
    public bool isWalking { get; private set; } // Indicates if player is walking
    public Vector2 mouseScreenPosition { get; private set; } // Mouse position on screen

    private PlayerControls controls; // Reference to the auto-generated input actions class

    private void Awake()
    {
        controls = new PlayerControls();
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
        isWalking = true;
        movementInputVector = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        isWalking = false;
        movementInputVector = Vector2.zero;
    }

    private void OnRunPerformed(InputAction.CallbackContext ctx)
    {
        isRunning = true;
    }

    private void OnRunCanceled(InputAction.CallbackContext ctx)
    {
        isRunning = false;
    }

    private void OnLookPerformed(InputAction.CallbackContext ctx)
    {
        mouseScreenPosition = ctx.ReadValue<Vector2>();
    }

    public bool WasBuildModePressedThisFrame() => controls.Player.BuildMode.WasPressedThisFrame();
    public bool WasMainActionPressedThisFrame() => controls.Player.MainAction.WasPressedThisFrame();
    public bool WasSecondaryActionPressedThisFrame() => controls.Player.SecondaryAction.WasPressedThisFrame();
    public bool WasRotateActionPressedThisFrame() => controls.Player.RotateAction.WasPressedThisFrame();
}
