using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float runSpeedMultiplier = 1.5f;   // Extra speed while running

    [Header("Rotation Settings")]
    public float rotationSpeed = 360f;
    public float runningRotationMultiplier = 1.5f; // Faster turn while running
    public float mouseRotationSpeed = 5f; // Speed when rotating toward mouse

    [Header("Mouse Rotation Settings")]
    public LayerMask groundLayer; // LayerMask for detecting the ground when raycasting

    // Reference to our input script (be sure to assign in Inspector)
    private PlayerMovementInputHandler inputHandler;
    public bool isRotatingStill { get; private set; } = false;

    public Camera mainCamera;

    private CharacterController controller;
    private GameManager gm;


    private void Start()
    {
        gm = GameManager.Instance;
        inputHandler = gm.getPlayer().GetComponent<PlayerMovementInputHandler>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // 1. Read movement input
        Vector2 input = inputHandler.movementInputVector;
        Vector3 move = new Vector3(input.x, 0f, input.y);

        // 2. Check if Running
        bool isRunning = inputHandler.isRunning;

        // 3. Handle Rotation
        if (move.sqrMagnitude > 0.001f)
        {
            // Rotate based on movement direction
            RotateTowardMovementVector(move, isRunning);
            isRotatingStill = false;

        }
        else
        {
            // Rotate towards mouse when not moving
            RotateTowardMouse();
            isRotatingStill = true;
        }

        // 4. Adjust move speed if running
        float currentSpeed = moveSpeed;
        if (isRunning)
        {
            currentSpeed *= runSpeedMultiplier;
        }

        // 5. Apply movement
        controller.Move(move * currentSpeed * Time.deltaTime);

        // 6. Check for main action
        if (inputHandler.WasMainActionPressedThisFrame())
        {
            if (!gm.isBuildMode)
            {
                bool esit = gm.getPlayerQickBar().selectedItemScript.PerformMainAction();
            }

        }

    }

    private void RotateTowardMovementVector(Vector3 movementDirection, bool isRunning)
    {
        // Avoid zero magnitude
        if (movementDirection.sqrMagnitude < 0.001f)
        {
            return;
        }

        // Calculate the direction we want to face
        Quaternion targetRotation = Quaternion.LookRotation(movementDirection);

        // If running, we rotate faster
        float currentRotationSpeed = rotationSpeed;
        if (isRunning)
        {
            currentRotationSpeed *= runningRotationMultiplier;
        }

        // Smoothly rotate to face that direction
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            currentRotationSpeed * Time.deltaTime
        );
    }

    private void RotateTowardMouse()
    {
        Vector2 screenMousePos = inputHandler.mouseScreenPosition; // Get mouse position from input handler

        Ray ray = mainCamera.ScreenPointToRay(screenMousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 lookAtPoint = hit.point;
            lookAtPoint.y = transform.position.y; // Keep rotation on the Y-axis only

            Quaternion targetRotation = Quaternion.LookRotation(lookAtPoint - transform.position);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                mouseRotationSpeed * Time.deltaTime
            );
        }
    }

}
