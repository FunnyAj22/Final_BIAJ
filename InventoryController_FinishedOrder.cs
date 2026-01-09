using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryController_FinishedOrder : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;

    private List<Order_Slot> slots = new List<Order_Slot>();

    void Start()
    {
        // Create slots once at start
        for (int i = 0; i < slotCount; i++)
        {
            GameObject obj = Instantiate(slotPrefab, inventoryPanel.transform);
            Order_Slot slot = obj.GetComponent<Order_Slot>();

            slot.parentController = this;

            slots.Add(slot);
            slot.gameObject.SetActive(true);
        }
    }
    public void SetupCustomerOrder(string customerName, List<int> requiredFoodVals)
    {
        Debug.Log($"New order for {customerName} with {requiredFoodVals.Count} items.");

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < requiredFoodVals.Count)
            {
                slots[i].ShowRequiredItem(requiredFoodVals[i]);
                slots[i].SetVisible(true);
            }
            else
            {
                slots[i].SetVisible(false);
            }
        }
    }

    void Update()
    {
      //  if (Input.GetKeyDown(KeyCode.Space))
        //    CheckAllSlots();
    }

    public void CheckAllSlots()
    {
        bool allCorrect = true;

        foreach (Order_Slot slot in slots)
        {
            if (!slot.isActiveAndEnabled || slot.currentItem == null)
            {
                allCorrect = false;
                continue;
            }

            DragAndDrop item = slot.currentItem.GetComponent<DragAndDrop>();
            if (item == null || item.FoodVal != slot.slotVal)
            {
                allCorrect = false;
            }
        }

        if (allCorrect)
        {
            Debug.Log("All items correct!");
            foreach (Order_Slot slot in slots)
            {
                if (slot.currentItem != null)
                {
                    Destroy(slot.currentItem);
                    slot.currentItem = null;
                }
            }

            KeepScore.AddScore(Random.Range(5, 15));
            StartCoroutine(FadeOutSlotsThenNewOrder());
        }
        else
        {
            Debug.Log(" Not all items are correct yet.");
        }
    }

    private IEnumerator FadeOutSlotsThenNewOrder()
    {
        foreach (Order_Slot slot in slots)
            slot.FadeOutSlot();

        yield return new WaitForSeconds(1.0f);

        List<int> newVals = GetNewRandomOrder();
        SetupCustomerOrder("CustomerName", newVals);
    }

    private List<int> GetNewRandomOrder()
    {
        List<int> vals = new List<int>();
        int count = slotCount;
        for (int i = 0; i < count; i++)
        {
            vals.Add(Random.Range(1, 3)); // example range
        }
        return vals;
    }
    public Order_Slot GetOpenSlotForFood(int foodID)
    {
        foreach (Order_Slot slot in slots)
        {
      
            if (slot.isActiveAndEnabled && slot.slotVal == foodID && slot.currentItem == null)
            {
                return slot;
            }
        }
        return null; // No matching empty slot found
    }


}
