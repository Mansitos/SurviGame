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
        isRotatingStill = playerMovement.isRotatingStill;
    }

    private void UpdateAnimationsStatus()
    {
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isRotatingStill", isRotatingStill);

        //Debug.Log($"Running: {isRunning}, Walking: {isWalking}, RotatingStill {isRotatingStill}");

    }
}
