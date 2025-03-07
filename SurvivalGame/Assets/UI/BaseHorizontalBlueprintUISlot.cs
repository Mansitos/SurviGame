using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class BaseHorizontalBlueprintUISlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected Blueprint blueprint;

    [SerializeField] protected GameObject inventoryUISlotGO;
    [SerializeField] protected GameObject nameTextGO;
    [SerializeField] protected GameObject requirementsTextGO;
    [SerializeField] public GameObject inventoryUISlotCounterPrefab;

    protected TextMeshProUGUI nameText;
    protected TextMeshProUGUI requirementsText;
    protected InventorySystem inventory;
    protected Color baseColor;
    protected GameObject linkedGO;

    protected virtual void Awake()
    {
        inventory = GameManager.Instance.GetPlayerInventory();
        nameText = nameTextGO.GetComponent<TextMeshProUGUI>();
        requirementsText = requirementsTextGO.GetComponent<TextMeshProUGUI>();
        baseColor = inventoryUISlotGO.GetComponent<Image>().color;
    }

    public abstract void UpdateSlotUI();

    protected abstract void InitUI();

    public void SetBluePrint(Blueprint blueprint, bool skipUpdateUI = false)
    {
        this.blueprint = blueprint;
        if (!skipUpdateUI)
        {
            InitUI();
        }
    }

    public void SetLinkedGameObject(GameObject go, bool skipUpdateUI = false)
    {
        this.linkedGO = go;
        if (!skipUpdateUI)
        {
            InitUI();
        }
    }

    public abstract void OnPointerClick(PointerEventData eventData);

  }

