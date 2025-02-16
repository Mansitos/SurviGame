using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    private ItemInstance itemInstance;
    private Transform playerTransform; // Reference to the player's transform
    private GameManager gm;
    private bool isBeingCollected = false;
    private float speed = 1.0f; // Initial speed
    private float maxSpeed = 10.0f; // Maximum speed
    private float acceleration = 0.1f; // Speed increment per frame
    private float startCollectDistance = 5.0f; // Distance threshold for collection to start
    private float collectDistance = 0.5f; // Distance threshold for collection to end
    private static float ySpawnCoordinate = 0.25f;
    private float rotationSpeed = 45f;

    // Factory method to create a dropped item
    public static DroppedItem Spawn(ItemInstance itemInstance, Vector3 spawnPosition)
    {
        if (itemInstance.ItemData.worldPrefab == null)
        {
            Debug.LogError($"[DroppedItem] Missing prefab for {itemInstance.ItemData.itemName}.");
            return null;
        }

        // Generate a random angle for Y-axis rotation
        float randomYRotation = Random.Range(0f, 360f);
        Quaternion randomRotation = Quaternion.Euler(0, randomYRotation, 0);
        spawnPosition.y = ySpawnCoordinate;

        // Instantiate the item in the world with random Y rotation
        GameObject droppedItemGO = Instantiate(itemInstance.ItemData.worldPrefab, spawnPosition, randomRotation);


        // Ensure it has a DroppedItem component
        DroppedItem droppedItem = droppedItemGO.GetComponent<DroppedItem>();
        if (droppedItem == null)
        {
            droppedItem = droppedItemGO.AddComponent<DroppedItem>();
        }

        // Initialize it with the item instance
        droppedItem.Initialize(itemInstance);

        return droppedItem;
    }

    public void Initialize(ItemInstance item)
    {
        if (item.Quantity > 1)
        {
            Debug.LogError("[DroppedItem] Dropped item should be initialised only with quantity 1");
        }
        itemInstance = item;
        gm = GameManager.Instance;
        playerTransform = gm.GetPlayerGO().transform;
    }

    public ItemInstance GetItemInstance()
    {
        return itemInstance;
    }

    public void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (!isBeingCollected)
        {
            bool canCollect = CheckStartCollectConditions(distanceToPlayer);
            if (canCollect)
            {
                isBeingCollected = canCollect;
                // Item is added at animation start, not at the end, to avoid multiple animation to
                // start and fail to add a the end of animation for what happened meanwhile.
                // The "TryAdd" should not fail since the check has been just be done in CheckStartCollectConditions.

                bool esit = gm.GetInventorySystem().TryAddItem(itemInstance);
                if (!esit) { 
                    Debug.LogWarning("[DroppedItem] This should not happen.");
                }
            }
            else
            {
                RotateAnimation();
            }
        }
        else
        {
            if (!CheckEndCollectConditions(distanceToPlayer))
            {
                MoveTowardsPlayer();
            }
            else
            {
                    Destroy(this.gameObject);
            }
        }
    }


    private bool CheckStartCollectConditions(float distanceToPlayer)
    {
        if (distanceToPlayer < startCollectDistance)
        {
            if (gm.GetInventorySystem().CanAddItem(itemInstance))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private bool CheckEndCollectConditions(float distanceToPlayer)
    {
        if (distanceToPlayer < collectDistance)
        {
            return true;
        }
        return false;
    }

    private void MoveTowardsPlayer()
    {
        if (isBeingCollected)
        {
            float step = speed * Time.deltaTime; // Calculate the step size based on speed and frame time
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step); // Move towards the player

            if (speed < maxSpeed)
            {
                speed += acceleration; // Increase speed until reaching max speed
            }
        }
    }

    private void RotateAnimation()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0); // Rotate around the Y-axis
    }
}
