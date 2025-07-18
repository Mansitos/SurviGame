using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 movementInputVector { get; private set; }
    public bool isRunning { get; private set; }
    public bool isWalking { get; private set; }
    public Vector2 mouseScreenPosition { get; private set; }

    private PlayerControls controls; // Reference to the auto-generated input actions class
    private GameManager gm;

    public static event Action OnInventoryKeyPressedEvent;
    public static event Action OnCraftingKeyPressedEvent;
    public static event Action OnBuildingKeyPressedEvent;
    public static event Action OnEscKeyPressedEvent;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void Start()
    {
        gm = GameManager.Instance;
    }

    private void Update()
    {
        UpdateActionMapActiveStatus();
        InvokeEvents();
    }

    private void OnEnable()
    {
        controls.UI.Enable();

        // Subscribe to movement and look actions
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
        controls.Player.Run.performed += OnRunPerformed;
        controls.Player.Run.canceled += OnRunCanceled;
        controls.Player.Look.performed += OnLookPerformed;
    }


    private void UpdateActionMapActiveStatus()
    {
        if (gm.IsInNormalMode() || gm.IsInBuildMode()) // improve
        {
            if (!controls.Player.enabled)
            {
                controls.Player.Enable();
            }
            if (!controls.QuickBar.enabled)
            {
                controls.QuickBar.Enable();
            }
        }
        else
        {
            controls.Player.Disable();
            controls.QuickBar.Disable();
        }
    }
   
    private void InvokeEvents()
    {
        if (WasInventoryModePressedThisFrame())
        {
            OnInventoryKeyPressedEvent?.Invoke();
        }

        if (WasEscInventoryPressedThisFrame())
        {
            OnEscKeyPressedEvent?.Invoke();
        }

        if (WasCraftingKeyPressedThisFrame())
        {
            OnCraftingKeyPressedEvent?.Invoke();
        }

        if (WasBuildingKeyPressedThisFrame())
        {
            OnBuildingKeyPressedEvent?.Invoke();
        }
    }

    // --- Player ---
    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        isWalking = true;
        movementInputVector = ctx.ReadValue<Vector2>();

        // If running button is already held, start running immediately
        if (controls.Player.Run.IsPressed() && movementInputVector.sqrMagnitude > 0.001f)
        {
            isRunning = true;
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

    public bool WasMainActionPressedThisFrame() => controls.Player.MainAction.WasPressedThisFrame();
    public bool WasSecondaryActionPressedThisFrame() => controls.Player.SecondaryAction.WasPressedThisFrame();
    public bool WasRotateActionPressedThisFrame() => controls.Player.RotateAction.WasPressedThisFrame();

    // --- UI ---
    public bool WasInventoryModePressedThisFrame() => controls.UI.Inventory.WasPressedThisFrame();
    public bool WasEscInventoryPressedThisFrame() => controls.UI.Close.WasPressedThisFrame();
    public bool WasCraftingKeyPressedThisFrame() => controls.UI.Crafting.WasPressedThisFrame();
    public bool WasBuildingKeyPressedThisFrame() => controls.UI.Building.WasPressedThisFrame();


    // --- QuickBar ---
    public bool WasQuickBarKeyPressedThisFrame(int key)
    {
        Dictionary<int, InputAction> quickBarActions = new Dictionary<int, InputAction>
        {
            { 1, controls.QuickBar._1 },
            { 2, controls.QuickBar._2 },
            { 3, controls.QuickBar._3 },
            { 4, controls.QuickBar._4 },
            { 5, controls.QuickBar._5 },
            { 6, controls.QuickBar._6 },
            { 7, controls.QuickBar._7 },
            { 8, controls.QuickBar._8 },
            { 9, controls.QuickBar._9 },
            { 0, controls.QuickBar._0 }
        };

        if (quickBarActions.TryGetValue(key, out InputAction action))
        {
            return action.WasPressedThisFrame();
        }
        return false;
    }
}
