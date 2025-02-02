using UnityEngine;
using System.Collections.Generic;

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
    private int buildingWidth = 1;
    private int buildingHeight = 1;
    private Building buildingComponent;

    private void Awake()
    {
        mainCamera = GameManager.Instance.getMainCamera().GetComponent<Camera>();
        inputHandler = GameManager.Instance.getPlayer().GetComponent<PlayerMovementInputHandler>();
        xzPositioningOffset = GameManager.Instance.getTerrainGridSystem().GetComponent<Grid>().cellSize.x / 2;

        // Read building dimensions from prefab
        buildingComponent = buildingPrefab.GetComponent<Building>();
        if (buildingComponent != null)
        {
            buildingWidth = buildingComponent.xdimension;
            buildingHeight = buildingComponent.zdimension;
        }
    }

    private void Update()
    {
        // Toggle build mode
        if (inputHandler.WasBuildModeToggledThisFrame())
        {
            isBuildMode = !isBuildMode;
            if (isBuildMode) CreatePreviewObject();
            else DestroyPreviewObject();
        }

        if (isBuildMode)
        {
            Vector3Int gridPos = GridManager.Instance.WorldToGrid(GetMouseWorldPosition());
            UpdatePreviewObject(gridPos);

            if (inputHandler.WasMainActionPressedThisFrame())
            {
                TryPlaceBuilding(gridPos);
            }

            if (inputHandler.WasSecondaryActionPressedThisFrame())
            {
                TryRemoveBuilding(gridPos);
            }

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

        // Swap width & height when rotating
        if (currentRotation == 90 || currentRotation == 270)
        {
            (buildingWidth, buildingHeight) = (buildingComponent.zdimension, buildingComponent.xdimension);
        }

        if (currentRotation == 0 || currentRotation == 180)
        {
            (buildingWidth, buildingHeight) = (buildingComponent.xdimension, buildingComponent.zdimension);
        }

        if (previewObject != null)
        {
            previewObject.transform.rotation = Quaternion.Euler(0, currentRotation, 0);
        }

        Debug.Log($"Rotated to {currentRotation}° (New Size: {buildingWidth}x{buildingHeight})");
    }

    public int getActualBuildingRotation()
    {
        return currentRotation;
    }

    private void TryPlaceBuilding(Vector3Int gridPos)
    {
        if (GridManager.Instance.CanPlaceBuilding(gridPos, buildingWidth, buildingHeight, currentRotation))
        {
            Vector3 placementPos = GridManager.Instance.GridToWorld(gridPos);
            placementPos.x += xzPositioningOffset;
            placementPos.z += xzPositioningOffset;
            placementPos.y = 0;

            GameObject newBuilding = Instantiate(buildingPrefab, placementPos, Quaternion.Euler(0, currentRotation, 0));
            GridManager.Instance.PlaceObjectOnTiles(gridPos, buildingWidth, buildingHeight, newBuilding, currentRotation);

            Debug.Log($"Placed building at {gridPos} (Size: {buildingWidth}x{buildingHeight})");
        }
        else
        {
            Debug.Log($"Cannot place building at {gridPos} (This or some needed tiles are occupied!)");
        }
    }

    private void TryRemoveBuilding(Vector3Int gridPos)
    {
        GameObject obj = GridManager.Instance.GetObjectOnTile(gridPos);
        if (obj != null)
        {
            Building buildingComponent = obj.GetComponent<Building>();
            if (buildingComponent != null)
            {
                GridManager.Instance.RemoveObjectFromTiles(gridPos);
            }
            Destroy(obj);
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
