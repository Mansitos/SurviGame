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

    protected TextMeshProUGUI nameText;
    protected TextMeshProUGUI requirementsText;
    protected InventorySystem inventory;
    protected Color baseColor;

    protected virtual void Awake()
    {
        inventory = GameManager.Instance.GetInventorySystem();
        nameText = nameTextGO.GetComponent<TextMeshProUGUI>();
        requirementsText = requirementsTextGO.GetComponent<TextMeshProUGUI>();
        baseColor = inventoryUISlotGO.GetComponent<Image>().color;
    }

    public abstract void UpdateSlotUI();

    protected abstract void InitUI();

    public void SetBluePrint(Blueprint blueprint)
    {
        this.blueprint = blueprint;
        InitUI();
    }

    public abstract void OnPointerClick(PointerEventData eventData);

  }

