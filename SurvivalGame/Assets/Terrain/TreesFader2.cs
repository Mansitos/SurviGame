using System.Collections.Generic;
using UnityEngine;

public class TreesFader2 : MonoBehaviour
{
    public Transform player;                      // Assign the player
    public Camera camera;                         // Assign the main camera
    public LayerMask treeLayer;                   // Assign "Tree" layer
    public string opacityParameter = "_Opacity";  // Shader parameter for transparency
    public float targetTransparency = 0.3f;       // Maximum fade value (transparent)
    public float noTransparencyValue = 0f;        // Fully visible value

    // Parameters for front-facing trees:
    public float minTransparencyDistanceFront = 5f;  // Distance at which trees in front become fully transparent
    public float maxOpacityDistanceFront = 15f;      // Distance at which trees in front become fully opaque

    // Parameters for back-facing trees:
    public float minTransparencyDistanceBack = 5f;   // Distance at which trees behind become fully transparent
    public float maxOpacityDistanceBack = 15f;       // Distance at which trees behind become fully opaque

    public float detectionRadius = 20f;               // Radius around the player for tree detection

    // Add Z offset for the center of the overlap sphere
    public float zOffset = 2f;                       // Z offset to shift the sphere's detection center

    void Update()
    {
        // Reset all trees (set fully visible)
        ResetAllTrees();

        // Get the center position of the overlap sphere (player position + zOffset)
        Vector3 sphereCenter = player.position + new Vector3(0, 0, zOffset);

        // Use OverlapSphere to find all colliders in the detection radius around the player (with the offset)
        Collider[] colliders = Physics.OverlapSphere(sphereCenter, detectionRadius, treeLayer);

        // Loop through all detected colliders to check if they're trees
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Tree"))
            {
                Vector3 directionToTree = collider.transform.position - player.position + new Vector3(0, 0, zOffset);
                float distanceToTree = directionToTree.magnitude;  // Get the distance to the tree

                // Check if the tree is in front or behind the player (based on Z-value)
                if (directionToTree.z > 0)  // Tree is in front of the player
                {
                    // Only apply fading to trees within the detection radius
                    if (distanceToTree <= detectionRadius)
                    {
                        // Calculate opacity for trees in front of the player
                        float fadeValue = CalculateFadeValue(distanceToTree, false);
                        // Apply the calculated opacity to the tree
                        Renderer treeRenderer = collider.GetComponent<Renderer>();
                        if (treeRenderer != null)
                        {
                            SetOpacity(treeRenderer, fadeValue);
                        }
                    }
                }
                else if (directionToTree.z < 0)  // Tree is behind the player
                {
                    // Only apply fading to trees within the detection radius
                    if (distanceToTree <= detectionRadius)
                    {
                        // Calculate opacity for trees behind the player
                        float fadeValue = CalculateFadeValue(distanceToTree, true);
                        // Apply the calculated opacity to the tree
                        Renderer treeRenderer = collider.GetComponent<Renderer>();
                        if (treeRenderer != null)
                        {
                            SetOpacity(treeRenderer, fadeValue);
                        }
                    }
                }
            }
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
            }
        }
    }

    // Calculates the opacity value based on the distance to the tree
    float CalculateFadeValue(float distance, bool isFront)
    {
        float minDistance, maxDistance;

        if (isFront) // For trees in front of the player
        {
            minDistance = minTransparencyDistanceFront;
            maxDistance = maxOpacityDistanceFront;
        }
        else // For trees behind the player
        {
            minDistance = minTransparencyDistanceBack;
            maxDistance = maxOpacityDistanceBack;
        }

        // If the distance is closer than the minTransparencyDistance, it's fully transparent
        if (distance <= minDistance)
        {
            return targetTransparency;
        }

        // If the distance is farther than the maxOpacityDistance, it's fully opaque
        if (distance >= maxDistance)
        {
            return noTransparencyValue;
        }

        // Linearly interpolate between targetTransparency and noTransparencyValue based on the distance
        float t = (distance - minDistance) / (maxDistance - minDistance);
        return Mathf.Lerp(targetTransparency, noTransparencyValue, t);
    }

    // Applies the opacity value to each material of the given renderer.
    void SetOpacity(Renderer renderer, float value)
    {
        MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(propBlock);
        for (int i = 0; i < renderer.sharedMaterials.Length; i++)
        {
            propBlock.SetFloat(opacityParameter, value);
            renderer.SetPropertyBlock(propBlock, i);
        }
    }
}
