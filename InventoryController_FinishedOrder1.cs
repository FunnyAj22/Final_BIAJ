using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class InventoryController_FinishedOrder1 : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;

    private List<Order_Slot> slots = new List<Order_Slot>(); // Store all slots

    void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            Order_Slot newSlot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Order_Slot>();
            slots.Add(newSlot);
        }
    }
    public void SetupCustomerOrder(string customerName, List<int> requiredFoodVals)
    {
        Debug.Log($"New order for {customerName} with {requiredFoodVals.Count} items.");

        // Clear old slots
        foreach (Transform child in inventoryPanel.transform)
            Destroy(child.gameObject);

        slots.Clear();

        // Create new slots matching the requiredFoodVals
        foreach (int val in requiredFoodVals)
        {
            Order_Slot newSlot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Order_Slot>();
            newSlot.slotVal = val; // expected value
            slots.Add(newSlot);

        }
    }

    //  Spawns slots
    public void GenerateSlots()
    {
        // Clear any existing slots first
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        slots.Clear();

        // Create new slots
        for (int i = 0; i < slotCount; i++)
        {
            Order_Slot newSlot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Order_Slot>();
            slots.Add(newSlot);
        }

        Debug.Log(" Generated new slots with random values!");
    }

    void Update()
    {
        // Press Space to manually check all slots
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckAllSlots();
        }
    }

    public void CheckAllSlots()
    {
        bool allCorrect = true;

        foreach (Order_Slot slot in slots)
        {
            if (slot.currentItem == null)
            {
                allCorrect = false;
                Debug.Log($" Slot {slot.name} is empty.");
                continue;
            }

            DragAndDrop item = slot.currentItem.GetComponent<DragAndDrop>();

            if (item == null)
            {
                allCorrect = false;
                Debug.Log($" Slot {slot.name} has no DragAndDrop component.");
                continue;
            }

            if (item.FoodVal != slot.slotVal)
            {
                allCorrect = false;
                Debug.Log($"Mismatch in {slot.name}: FoodVal={item.FoodVal}, SlotVal={slot.slotVal}");
            }
            else
            {
                Debug.Log($" Match! {slot.name}: FoodVal={item.FoodVal}");
            }
        }

        if (allCorrect)
        {
            
            Debug.Log(" All items are correctly placed!");
            foreach (Order_Slot slot in slots)
            {
                if (slot.currentItem != null)
                {
                    Destroy(slot.currentItem);
                    slot.currentItem = null;
                }
            }

            KeepScore.AddScore(Random.Range(5, 15)); // random bonus

            ResetInventory();
            
        }
        else
        {
            Debug.Log(" Not all items are correct yet.");
        }
    }

    //  Removes all slots and regenerates new ones
    public void ResetInventory()
    {
        // Destroy slots immediately
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }
        slots.Clear();

        // Optionally add a small delay before regenerating
        Invoke(nameof(GenerateSlots), 1.0f);
    }
}



