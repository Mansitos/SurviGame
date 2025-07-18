using UnityEngine;
using System.Collections.Generic;

public class BuildingPlacer : MonoBehaviour
{
    [Header("Building Placer Variables")]

    [Tooltip("Building Prefab to build")]
    public GameObject buildingPrefab;
    [Tooltip("If True -> debug logs.")]
    public bool verboseLogging;
    [Tooltip("LayerMask for detecting the ground when raycasting.")]
    public LayerMask groundLayer;

    private GameManager gm;
    private GameObject previewObject;
    private InputHandler inputHandler;
    private Building<BuildingData> buildingComponent;
    private Camera mainCamera;
    private int currentRotation = 0; // Rotation angle (90� increments)
    private float xzPositioningOffset;
    private int buildingWidth = 1;
    private int buildingHeight = 1;

    private void Start()
    {
        gm = GameManager.Instance;
        mainCamera = gm.GetMainCameraGO().GetComponent<Camera>();
        inputHandler = gm.GetPlayerGO().GetComponent<InputHandler>();
        xzPositioningOffset = gm.GetTerrainGridSystemGO().GetComponent<Grid>().cellSize.x / 2;
    }

    private void Update()
    {
        VerifyChangeInStatus();

        if (gm.IsInBuildMode())
        {
            HandleBuildActions();
        }
    }

    private void HandleBuildActions()
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

    public void VerifyChangeInStatus()
    {
        if (gm.IsInBuildMode()) CreatePreviewObject();
        else DestroyPreviewObject();
    }

    private void RotateBuilding()
    {
        BuildingData data = buildingPrefab.GetComponent<WorldObjectBase>().GetWorldObjectData() as BuildingData;

        currentRotation += 90;
        if (currentRotation >= 360)
            currentRotation = 0;

        // Swap width & height when rotating
        if (currentRotation == 90 || currentRotation == 270)
        {
            (buildingWidth, buildingHeight) = (data.zdimension, data.xdimension);
        }

        if (currentRotation == 0 || currentRotation == 180)
        {
            (buildingWidth, buildingHeight) = (data.xdimension, data.zdimension);
        }

        if (previewObject != null)
        {
            previewObject.transform.rotation = Quaternion.Euler(0, currentRotation, 0);
        }

        if (verboseLogging)
        {
            Debug.Log($"[BuildingPlacer] Rotated to {currentRotation}� (New Size: {buildingWidth}x{buildingHeight})");
        }
    }

    public int getActualBuildingRotation()
    {
        return currentRotation;
    }

    public void SetBuilding(GameObject building)
    {
        buildingPrefab = building;
        BuildingData data = buildingPrefab.GetComponent<WorldObjectBase>().GetWorldObjectData() as BuildingData;
        buildingWidth = data.xdimension;
        buildingHeight = data.zdimension;
        currentRotation = 0;
    }

    private void TryPlaceBuilding(Vector3Int gridPos)
    {
        IBuildable building = buildingPrefab.GetComponent<IBuildable>();
        if (GridManager.Instance.CanPlaceBuilding(gridPos, buildingWidth, buildingHeight, currentRotation, checkAgainstPlayer:true, isWaterBuilding: building.IsWaterBuilding()))
        {
            Vector3 placementPos = GridManager.Instance.GridToWorld(gridPos);
            placementPos.x += xzPositioningOffset;
            placementPos.z += xzPositioningOffset;
            placementPos.y = 0;

            GameObject newBuilding = Instantiate(buildingPrefab, placementPos, Quaternion.Euler(0, currentRotation, 0));
            GridManager.Instance.PlaceObjectOnTiles(gridPos, buildingWidth, buildingHeight, newBuilding, currentRotation);

            if (verboseLogging)
            {
                Debug.Log($"[BuildingPlacer] Placed building at {gridPos} (Size: {buildingWidth}x{buildingHeight})");
            }

            gm.SetBuildMode(false);
        }
        else
        {
            if (verboseLogging)
            {
                Debug.Log($"[BuildingPlacer] Cannot place building at {gridPos} (This or some needed tiles are occupied!)");
            }
        }
    }

    private void TryRemoveBuilding(Vector3Int gridPos)
    {
        GameObject obj = GridManager.Instance.GetObjectOnTile(gridPos);
        if (obj != null)
        {
            Building<BuildingData> buildingComponent = obj.GetComponent<Building<BuildingData>>();
            if (buildingComponent != null)
            {
                GridManager.Instance.RemoveObjectFromTiles(gridPos);
                Destroy(obj);
                if (verboseLogging)
                {
                    Debug.Log($"[BuildingPlacer] Removed building at {gridPos}");
                }
            }
        }
        else
        {
            if (verboseLogging)
            {
                Debug.Log($"[BuildingPlacer] No building found at {gridPos}");
            }
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
