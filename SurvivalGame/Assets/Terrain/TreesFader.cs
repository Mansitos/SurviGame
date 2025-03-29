using System.Collections.Generic;
using UnityEngine;

public class TreesFader : MonoBehaviour
{
    public Transform player;                      // Assign the player
    public Camera camera;                         // Assign the main camera
    public LayerMask treeLayer;                   // Assign "Tree" layer
    public string opacityParameter = "_Opacity";  // Shader parameter for transparency
    public float targetTransparency = 0.3f;       // Maximum fade value (transparent)
    public float noTransparencyValue = 0f;        // Fully visible value

    // Parameters for horizontal and vertical offsets:
    public float horizontalOffset = 1.5f;           // Maximum horizontal offset for target points
    public int horizontalRayCount = 3;              // Number of rays to cast horizontally
    public float verticalOffset = 1.0f;             // Maximum vertical offset for target points
    public float fixedYOffset = 0.5f;
    public int verticalRayCount = 3;                // Number of rays to cast vertically

    // New parameter: number of ray hits required to achieve full targetTransparency.
    public int raysCountForTargetTransparency = 5;

    // Boolean to enable/disable raycast continuation through all hits
    public bool continueRaycasting = true;

    // Smooth transition parameters
    public float fadeSpeed = 5f; // Speed at which the transparency fades

    // Dictionary to store the current opacity of each tree renderer
    private Dictionary<Renderer, float> currentOpacity = new Dictionary<Renderer, float>();

    void Update()
    {
        // Dictionary to count how many rays hit each tree renderer.
        Dictionary<Renderer, int> treeHits = new Dictionary<Renderer, int>();

        // Cast rays in a grid pattern around the player's position.
        for (int i = 0; i < horizontalRayCount; i++)
        {
            // Calculate normalized value between 0 and 1 for horizontal offset.
            float tX = (horizontalRayCount == 1) ? 0.5f : (float)i / (horizontalRayCount - 1);
            float offsetX = Mathf.Lerp(-horizontalOffset, horizontalOffset, tX);

            for (int j = 0; j < verticalRayCount; j++)
            {
                // Calculate normalized value for vertical offset.
                float tY = (verticalRayCount == 1) ? 0.5f : (float)j / (verticalRayCount - 1);
                float offsetY = Mathf.Lerp(-verticalOffset, verticalOffset, tY);

                // Define the target point as the player's position plus both offsets.
                Vector3 targetPos = player.position + new Vector3(offsetX, offsetY + fixedYOffset, 0);
                CastRayToTarget(targetPos, treeHits);
            }
        }

        // Create a list to store opacity updates after iterating
        List<KeyValuePair<Renderer, float>> opacityUpdates = new List<KeyValuePair<Renderer, float>>();

        // Apply transparency based on hits relative to raysCountForTargetTransparency.
        foreach (var kvp in treeHits)
        {
            Renderer treeRenderer = kvp.Key;
            int hitCount = kvp.Value;

            // Determine fraction of target transparency based on hit count.
            float fraction = Mathf.Clamp01(hitCount / (float)raysCountForTargetTransparency);
            // Calculate the final fade value.
            float targetOpacity = fraction * targetTransparency;

            // Smooth the transition between the current opacity and the target opacity.
            float currentOpacityValue = currentOpacity.ContainsKey(treeRenderer) ? currentOpacity[treeRenderer] : noTransparencyValue;

            // If no rays hit, we want to gradually fade back to full opacity.
            if (hitCount == 0)
            {
                targetOpacity = noTransparencyValue; // Fully opaque when not hit by rays
            }

            // Smooth transition to the target opacity
            float smoothedOpacity = Mathf.Lerp(currentOpacityValue, targetOpacity, Time.deltaTime * fadeSpeed);

            // Store the opacity update for later application
            opacityUpdates.Add(new KeyValuePair<Renderer, float>(treeRenderer, smoothedOpacity));
        }

        // Ensure opacity for trees that are not in treeHits is reset to fully visible.
        foreach (var renderer in currentOpacity.Keys)
        {
            if (!treeHits.ContainsKey(renderer))
            {
                // If a tree is not in the raycast hit list, reset its opacity to full (fully visible)
                float currentOpacityValue = currentOpacity[renderer];
                if (currentOpacityValue > noTransparencyValue)
                {
                    // Gradually fade back to full opacity
                    float smoothedOpacity = Mathf.Lerp(currentOpacityValue, noTransparencyValue, Time.deltaTime * fadeSpeed);
                    opacityUpdates.Add(new KeyValuePair<Renderer, float>(renderer, smoothedOpacity));
                }
            }
        }

        // Apply all opacity updates after the loop
        foreach (var update in opacityUpdates)
        {
            SetOpacity(update.Key, update.Value);
            currentOpacity[update.Key] = update.Value;
        }
    }

    // Resets the opacity for all trees tagged as "Tree" to be fully visible.
    void ResetAllTrees()
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        foreach (GameObject tree in trees)
        {
            Renderer renderer = tree.GetComponent<Renderer>();
            if (renderer != null)
            {
                SetOpacity(renderer, noTransparencyValue);
                currentOpacity[renderer] = noTransparencyValue;  // Reset stored opacity
            }
        }
    }

    // Casts a ray from the camera toward the specified target position and updates the hit count in treeHits.
    void CastRayToTarget(Vector3 targetPos, Dictionary<Renderer, int> treeHits)
    {
        Vector3 camPos = camera.transform.position;
        Vector3 direction = (targetPos - camPos).normalized;
        float distance = Vector3.Distance(camPos, targetPos);

        Debug.DrawRay(camPos, direction * distance, Color.red, 0.1f); // Visualize the ray

        // Use RaycastAll to get all hits along the ray's path.
        RaycastHit[] hits = Physics.RaycastAll(camPos, direction, distance, treeLayer);

        // If continueRaycasting is true, process all hits, otherwise just the first hit
        if (continueRaycasting)
        {
            foreach (RaycastHit hit in hits)
            {
                Renderer treeRenderer = hit.collider.GetComponent<Renderer>();
                if (treeRenderer != null)
                {
                    if (treeHits.ContainsKey(treeRenderer))
                    {
                        treeHits[treeRenderer]++;
                    }
                    else
                    {
                        treeHits[treeRenderer] = 1;
                    }
                }
            }
        }
        else
        {
            // Only process the first hit if continueRaycasting is false
            if (hits.Length > 0)
            {
                Renderer treeRenderer = hits[0].collider.GetComponent<Renderer>();
                if (treeRenderer != null)
                {
                    if (treeHits.ContainsKey(treeRenderer))
                    {
                        treeHits[treeRenderer]++;
                    }
                    else
                    {
                        treeHits[treeRenderer] = 1;
                    }
                }
            }
        }
    }

    // Applies the opacity value to each material of the given renderer.
    void SetOpacity(Renderer renderer, float value)
    {
        // Get all Renderer components in this game object and its children.
        Renderer[] renderers = renderer.gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
            rend.GetPropertyBlock(propBlock);
            for (int i = 0; i < rend.sharedMaterials.Length; i++)
            {
                propBlock.SetFloat(opacityParameter, value);
                rend.SetPropertyBlock(propBlock, i);
            }
        }
    }

}
