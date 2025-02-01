using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Settings")]
    [Tooltip("If True, camera will follow target in x and z plane")]
    [SerializeField] bool followTarget = true;

    [SerializeField] GameObject targetRef;

    [Tooltip("An offset applied to the target position when setting the lookAt point")]
    [SerializeField] Vector3 targetPosOffset;

    [SerializeField] float minOffset = 20.0f;  // Minimum height
    [SerializeField] float maxOffset = 60.0f;  // Maximum height
    [SerializeField] float zOffset = -10.0f;   // Initial Z offset
    [SerializeField] float zoomSensitivity = 1.0f; // Zoom speed

    private bool cameraSet = false;

    [SerializeField] float zoomDeadZone = 0.001f;

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Camera.Enable();
    }

    private void OnDisable()
    {
        controls.Camera.Disable();
    }

    private void Start()
    {
        targetPosOffset += new Vector3(0, 0, -zOffset);
    }

    private void Update()
    {
        float rawZoom = controls.Camera.Zoom.ReadValue<float>();

        // Apply dead zone to ignore tiny drift
        if (Mathf.Abs(rawZoom) < zoomDeadZone)
        {
            rawZoom = 0f;
        }

        float zoomDelta = rawZoom * zoomSensitivity;

        // Modify both Y (height) and Z (distance) for zoom effect
        targetPosOffset.y -= zoomDelta;  // Move camera up/down
        targetPosOffset.z += zoomDelta;  // Move camera forward/backward

        // Clamp Y and Z to avoid extreme zooming
        targetPosOffset.y = Mathf.Clamp(targetPosOffset.y, minOffset, maxOffset);
        targetPosOffset.z = Mathf.Clamp(targetPosOffset.z, -maxOffset, -minOffset);

        // Follow the target if enabled
        if (followTarget && targetRef != null)
        {
            Vector3 playerPos = targetRef.transform.position;
            playerPos.y = 0f; // Keep ground level

            transform.position = playerPos + targetPosOffset;
        }

        // Set camera look-at only once
        if (!cameraSet)
        {
            transform.LookAt(targetRef.transform);
            cameraSet = true;
        }

        // Ensure camera is always looking at the target
        transform.LookAt(targetRef.transform);
    }
}
