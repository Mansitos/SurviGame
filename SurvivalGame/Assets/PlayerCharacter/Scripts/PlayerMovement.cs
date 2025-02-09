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

    // Private references
    private PlayerMovementInputHandler inputHandler;
    private CharacterController controller;
    private GameManager gm;
    private Animator animator;
    private Camera mainCamera;
    private Coroutine collectionCoroutine;
    private PlayerAnimationsHandler playerAnimationsHandler;

    // Private status
    private bool isRotatingStill = false;
    private bool isCollecting = false;
    private string collectionType = ""; //TODO: it sucks?

    private void Start()
    {
        gm = GameManager.Instance;
        mainCamera = gm.getMainCamera().GetComponent<Camera>();
        inputHandler = gm.getPlayer().GetComponent<PlayerMovementInputHandler>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerAnimationsHandler = gm.getPlayerAnimationHandler();
    }

    private void Update()
    {
        HandlePlayerMovement();
        HandleCollecting(); // TODO: in future maybe more general "handle action"? not hardcoded for collecting?
    }

    private void HandleCollecting()
    {
        // 1. Interrupt collection if the player moves
        if (isCollecting && inputHandler.isWalking)
        {
            StopCollectingResource();
            Log("[PlayerMovement] Collecting resource stopped since movement detected!");
        }

        // 2. Check for main action
        if (inputHandler.WasMainActionPressedThisFrame())
        {
            if (!gm.isBuildMode)
            {
                gm.getPlayerQickBar().GetSelectedItemInstance().PerformMainAction(gm);
            }

        }
    }

    private void HandlePlayerMovement()
    {
        // 1. Read movement input
        Vector2 input = inputHandler.movementInputVector;
        Vector3 move = new Vector3(input.x, 0f, input.y);

        // 2. Check if Running
        bool isRunning = inputHandler.isRunning;

        // 3. Handle Rotation
        if (move.sqrMagnitude > 0.001f)
        {
            RotateTowardMovementVector(move, isRunning); // Rotate based on movement direction
            isRotatingStill = false;
        }
        else
        {
            if (!IsPlayerPerformingAction()) // Rotate towards mouse when not moving and not performing action
            {
                RotateTowardMouse();
                isRotatingStill = true;
            }
        }

        // 4. Adjust move speed if running
        float currentSpeed = moveSpeed;
        if (isRunning)
        {
            currentSpeed *= runSpeedMultiplier;
        }

        // 5. Apply movement
        controller.Move(move * currentSpeed * Time.deltaTime);
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
            target.GetComponent<ResourceObject>().Collect();
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
            default:
                return ""; // TODO: how to handle case that should never happen??
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
