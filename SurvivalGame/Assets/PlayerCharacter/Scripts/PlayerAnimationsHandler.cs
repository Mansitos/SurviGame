using System.Data;
using UnityEngine;

public class PlayerAnimationsHandler : MonoBehaviour
{

    private PlayerMovementInputHandler inputHandler;
    private PlayerMovement playerMovement;
    private Animator animator;

    private bool isRunning = false;
    private bool isWalking = false;
    private bool isRotatingStill = false;
    private bool isCollecting = false;
    private string collectionType = "";

    void Start()
    {
        inputHandler = GetComponent<PlayerMovementInputHandler>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerStatus();
        UpdateAnimationsStatus();
    }

    private void UpdatePlayerStatus()
    {
        isRunning = inputHandler.isRunning;
        isWalking = inputHandler.isWalking;
        isRotatingStill = playerMovement.IsRotatingStill();
        isCollecting = playerMovement.IsCollecting();
        collectionType = playerMovement.collectionType;
    }

    private void UpdateAnimationsStatus()
    {
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isRotatingStill", isRotatingStill);

        if (isCollecting)
        {
            animator.SetBool(collectionType, isCollecting);
        }
        else
        {
            collectionType = "";
        }
    }

    public void ResetCollectingAnimation(string collectionType)
    {
        animator.SetBool(collectionType, false);
    }
}
