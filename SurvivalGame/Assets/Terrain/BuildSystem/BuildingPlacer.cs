using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public GameObject buildingPrefab;
    private GameObject previewObject; // Ghost preview object
    private bool isBuildMode = false;
    private int currentRotation = 0; // Rotation angle (90° increments)
    private PlayerMovementInputHandler inputHandler;
    public LayerMask groundLayer; // LayerMask for detecting the ground when raycasting

    private Camera mainCamera;
    private float xzPositioningOffset;

    private void Awake()
    {
        mainCamera = GameManager.Instance.getMainCamera().GetComponent<Camera>();
        inputHandler = GameManager.Instance.getPlayer().GetComponent<PlayerMovementInputHandler>();
        xzPositioningOffset = GameManager.Instance.getTerrainGridSystem().GetComponent<Grid>().cellSize.x / 2; // assumes x and y size is equal...
    }

    private void Update()
    {
        // Toggle build mode only once per key press
        if (inputHandler.WasBuildModeToggledThisFrame())
        {
            isBuildMode = !isBuildMode;
            if (isBuildMode)
            {
                CreatePreviewObject();
            }
            else
            {
                DestroyPreviewObject();
            }
            Debug.Log(isBuildMode ? "Build Mode Enabled" : "Build Mode Disabled");
        }

        if (isBuildMode)
        {
            Vector3Int gridPos = GridManager.Instance.WorldToGrid(GetMouseWorldPosition());
            UpdatePreviewObject(gridPos);

            // Place building once per left-click
            if (inputHandler.WasMainActionPressedThisFrame())
            {
                TryPlaceBuilding(gridPos);
            }

            // Remove building once per right-click
            if (inputHandler.WasSecondaryActionPressedThisFrame())
            {
                TryRemoveBuilding(gridPos);
            }

            // Rotate building once per key press
            if (inputHandler.WasRotateActionPressedThisFrame())
            {
                RotateBuilding();
            }
        }
    }

    private void RotateBuilding()
    {
        currentRotation += 90;
        if (currentRotation >= 360)
            currentRotation = 0;

        if (previewObject != null)
        {
            previewObject.transform.rotation = Quaternion.Euler(0, currentRotation, 0);
        }

        Debug.Log($"Rotated to {currentRotation}°");
    }

    private void TryPlaceBuilding(Vector3Int gridPos)
    {
        if (GridManager.Instance.CanPlaceBuilding(gridPos))
        {
            Vector3 placementPos = GridManager.Instance.GridToWorld(gridPos);
            placementPos.x += xzPositioningOffset;
            placementPos.z += xzPositioningOffset;
            placementPos.y = 0;

            GameObject newBuilding = Instantiate(buildingPrefab, placementPos, Quaternion.Euler(0, currentRotation, 0));
            GridManager.Instance.PlaceObjectOnTile(gridPos, newBuilding);

            Debug.Log($"Placed building at {gridPos} with rotation {currentRotation}°");
        }
        else
        {
            Debug.Log($"Cannot place building at {gridPos} (Tile occupied)");
        }
    }

    private void TryRemoveBuilding(Vector3Int gridPos)
    {
        GameObject obj = GridManager.Instance.GetObjectOnTile(gridPos);
        if (obj != null)
        {
            Destroy(obj);
            GridManager.Instance.RemoveObjectFromTile(gridPos);
            Debug.Log($"Removed building at {gridPos}");
        }
        else
        {
            Debug.Log($"No building found at {gridPos}");
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(inputHandler.mouseScreenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private void CreatePreviewObject()
    {
        if (previewObject == null)
        {
            previewObject = Instantiate(buildingPrefab);
            SetPreviewMaterial(previewObject);
        }
    }

    private void UpdatePreviewObject(Vector3Int gridPos)
    {
        if (previewObject != null)
        {
            Vector3 previewPos = GridManager.Instance.GridToWorld(gridPos);
            previewPos.x += xzPositioningOffset;
            previewPos.z += xzPositioningOffset;
            previewPos.y = 0;

            previewObject.transform.position = previewPos;
            previewObject.transform.rotation = Quaternion.Euler(0, currentRotation, 0);
        }
    }

    private void DestroyPreviewObject()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
        }
    }

    private void SetPreviewMaterial(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material previewMaterial = new Material(renderer.material);
            previewMaterial.color = new Color(1, 1, 1, 0.2f); // Transparent white
            renderer.material = previewMaterial;
        }
    }
}
