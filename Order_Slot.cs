using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Order_Slot : MonoBehaviour, IDropHandler
{
    public int slotVal;
    public GameObject currentItem;
    [SerializeField] private Image requiredIcon;
    public InventoryController_FinishedOrder parentController;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void ShowRequiredItem(int foodVal)
    {
        slotVal = foodVal;

        Food foodDatabase = FindObjectOfType<Food>();
        if (foodDatabase != null && requiredIcon != null)
        {
            Sprite icon = foodDatabase.GetFoodSprite(foodVal);
            requiredIcon.sprite = icon;
            requiredIcon.enabled = icon != null;
            requiredIcon.color = icon ? Color.white : new Color(1, 1, 1, 0);
        }
    }

    public void SetVisible(bool visible)
    {
        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }

    public void FadeOutSlot(float duration = 0.5f)
    {
        StartCoroutine(FadeOutRoutine(duration));
    }
    public void OnDrop(PointerEventData eventData)
    {
        DragAndDrop draggedItem = eventData.pointerDrag?.GetComponent<DragAndDrop>();
        if (draggedItem != null)
        {
            if (draggedItem.FoodVal == slotVal)
            {
                draggedItem.transform.SetParent(transform, false);
                draggedItem.transform.localPosition = Vector3.zero;

                if (currentItem != null)
                    currentItem.transform.SetParent(draggedItem.currentSlot?.transform);

                currentItem = draggedItem.gameObject;
                draggedItem.currentSlot = this;

                
                if (parentController != null)
                {
                    parentController.CheckAllSlots();
                }
            }
        }
    }

    private IEnumerator FadeOutRoutine(float duration)
    {
        float t = 0f;
        float startAlpha = canvasGroup.alpha;

        while (t < duration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
