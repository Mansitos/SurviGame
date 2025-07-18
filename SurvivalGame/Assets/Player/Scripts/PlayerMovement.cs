using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float runSpeedMultiplier = 1.5f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 360f;
    public float runningRotationMultiplier = 1.5f;
    public float mouseRotationSpeed = 5f;

    [Header("Generic Settings")]
    public LayerMask groundLayer; // LayerMask for detecting the ground when raycasting
    public bool debugLogs = false;
    public float groundStickForce;

    // Private references
    private InputHandler inputHandler;
    private CharacterController controller;
    private GameManager gm;
    private Animator animator;
    private Camera mainCamera;
    private Coroutine collectionCoroutine;
    private PlayerAnimationsHandler playerAnimationsHandler;

    // Private status
    private bool isRotatingStill = false;
    private bool isCollecting = false;
    private string collectionType = "";
    private float verticalVelocity = 0f;
    private float gravity = -9.81f;

    private void Start()
    {
        gm = GameManager.Instance;
        mainCamera = gm.GetMainCameraGO().GetComponent<Camera>();
        inputHandler = gm.GetPlayerGO().GetComponent<InputHandler>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerAnimationsHandler = gm.GetPlayerAnimationHandler();
    }

    private void Update()
    {
        if (gm.IsInNormalMode() || gm.IsInBuildMode()) { 
            HandlePlayerMovement();
        }
        HandleCollecting(); // TODO: in future maybe more general "handle action"? not hardcoded for collecting?
        CheckMainAction();
        CheckSecondaryAction();
    }

    private void CheckMainAction()
    {
        if (inputHandler.WasMainActionPressedThisFrame())
        {
            if (gm.IsInNormalMode())
            {
                gm.GetPlayerQuickBar().GetSelectedItemInstance().PerformMainAction(gm);
            }

        }
    }

    private void CheckSecondaryAction()
    {
        if (inputHandler.WasSecondaryActionPressedThisFrame())
        {
            if (gm.IsInNormalMode())
            {
                gm.GetPlayerQuickBar().GetSelectedItemInstance().PerformSecondaryAction(gm);
            }

        }
    }

    private void HandleCollecting()
    {
        // 1. Interrupt collection if the player moves
        if (isCollecting && inputHandler.isWalking)
        {
            StopCollectingResource();
            Log("[PlayerMovement] Collecting resource stopped since movement detected!");
        }
    }

    private void HandlePlayerMovement()
    {
        // 1. Read movement input
        Vector2 input = inputHandler.movementInputVector;
        Vector3 move = new Vector3(input.x, 0f, input.y);
        bool isRunning = inputHandler.isRunning;

        // 2. Handle Rotation
        if (move.sqrMagnitude > 0.001f)
        {
            RotateTowardMovementVector(move, isRunning);
            isRotatingStill = false;
        }
        else
        {
            if (!IsPlayerPerformingAction())
            {
                RotateTowardMouse();
                isRotatingStill = true;
            }
        }

        // 3. Adjust move speed if running
        float currentSpeed = moveSpeed;
        if (isRunning)
        {
            currentSpeed *= runSpeedMultiplier;
        }

        // 4. Handle Gravity
        if (controller.isGrounded)
        {
            // Ensure the player sticks to the ground
            verticalVelocity = -groundStickForce;
        }
        else
        {
            // Apply gravity over time
            verticalVelocity += gravity * Time.deltaTime;
        }

        // 5. Combine horizontal movement with vertical velocity
        Vector3 velocity = move * currentSpeed;
        velocity.y = verticalVelocity;

        // 6. Apply movement
        controller.Move(velocity * Time.deltaTime);
    }

    public bool IsRotatingStill()
    {
        return isRotatingStill;
    }

    public bool IsCollecting()
    {
        return isCollecting;
    }

    public void Log(string message)
    {
        if (debugLogs)
        {
            Debug.Log(message);
        }
    }

    public void StartCollectingResource(GameObject target, float duration, ResourceObjectType resourceType)
    {
        if (isCollecting)
        {
            Log("[PlayerMovement] Cannot start anoter collect resource since already doing so!");
            return;
        }

        isCollecting = true;
        collectionType = GetCollectionTypeAnimatorStatus(resourceType);
        collectionCoroutine = StartCoroutine(CollectResourceRoutine(target, duration));
    }

    private IEnumerator CollectResourceRoutine(GameObject target, float duration)
    {
        yield return new WaitForSeconds(duration);

        if (isCollecting) // Ensure collection wasn't interrupted
        {
            target.GetComponent<Resource>().Collect();
            StopCollectingResource();
            Log("[PlayerMovement] Collect resource finished!");
        }
    }

    public void StopCollectingResource()
    {
        if (collectionCoroutine != null)
        {
            StopCoroutine(collectionCoroutine);
            collectionCoroutine = null;
        }

        isCollecting = false;
        playerAnimationsHandler.ResetCollectingAnimation(collectionType);
        collectionType = "";
    }

    private string GetCollectionTypeAnimatorStatus(ResourceObjectType resourceType)
    {
        switch (resourceType)
        {
            case ResourceObjectType.Rock:
                return "isMining";
            case ResourceObjectType.Tree:
                return "isChopping";
            case ResourceObjectType.PickUp:
                return "isPickingUp";
            default:
                return "NO COLLECTION ANIMATOR STATUS FOR THIS COLLECTION TYPE"; // TODO: how to handle case that should never happen??
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

    public bool IsPlayerPerformingAction()
    {
        return isCollecting;
    }

    public string GetCollectionType()
    {
        return collectionType;
    }

}
