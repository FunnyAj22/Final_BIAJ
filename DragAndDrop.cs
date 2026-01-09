using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int FoodVal;
    public Order_Slot currentSlot;

    private Transform originalParent;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private CanvasGroup canvasGroup;
    public CookbookSlot previousCookbookSlot;

    private static Transform dragLayer;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // Cache DragLayer reference once
        if (dragLayer == null)
        {
            GameObject layer = GameObject.Find("DragLayer");
            if (layer != null) dragLayer = layer.transform;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = transform.localPosition;
        originalScale = transform.localScale;

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        previousCookbookSlot = transform.parent.GetComponent<CookbookSlot>();

        if (dragLayer != null)
            transform.SetParent(dragLayer, false); // parent to DragLayer
        transform.localScale = originalScale;      // keep original scale
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;
        Order_Slot dropSlot = null;

        if (hitObject != null)
        {
            
            dropSlot = hitObject.GetComponentInParent<Order_Slot>();

          
            if (dropSlot == null)
            {
                DropArea zone = hitObject.GetComponent<DropArea>();

                // If we hit the zone, ask it to find the correct slot for this food
                if (zone != null)
                {
                    dropSlot = zone.GetTargetSlot(FoodVal);
                }
            }
        }

     

        // Now simply check if 'dropSlot' was found 
        if (dropSlot != null && dropSlot.slotVal == FoodVal)
        {
            // Snap this item into the order slot
            transform.SetParent(dropSlot.transform, false);
            transform.localPosition = Vector3.zero;
            transform.localScale = originalScale;

            originalParent = dropSlot.transform;

            // Update references
            currentSlot = dropSlot;
            dropSlot.currentItem = gameObject;

            // Remove from cookbook slot
            CookbookSlot cookbookSlot = originalParent?.GetComponent<CookbookSlot>();
            if (cookbookSlot != null && cookbookSlot.currentItem == gameObject)
            {
                cookbookSlot.currentItem = null;
            }

            this.enabled = false;

            // Use the fix from the previous step
            if (dropSlot.parentController != null)
            {
                dropSlot.parentController.CheckAllSlots();
            }
        }
        else
        {
          
            if (currentSlot != null)
            {
                // Return to assigned order slot
                transform.SetParent(currentSlot.transform, false);
                transform.localPosition = Vector3.zero;
            }
            else
            {
                // only return to original parent if it's still active
                if (originalParent != null && originalParent.gameObject.activeInHierarchy)
                {
                    transform.SetParent(originalParent, false);
                    transform.localPosition = originalPosition;
                    if (previousCookbookSlot != null)
                        previousCookbookSlot.currentItem = gameObject;
                }
            }

            transform.localScale = originalScale;
        }
    }

}
