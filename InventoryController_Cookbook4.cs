using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController_Cookbook4: MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount = 10; // total number of slots to make


    public GameObject[] itemPrefabs;

    private List<Slot> slots = new List<Slot>(); // store the slots

    void Start()
    {
        //  Create the slots ONCE
        for (int i = 0; i < slotCount; i++)
        {
            Slot newSlot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            slots.Add(newSlot);
        }

    }

    void Update()
    {
        // Press L to respawn new items in the same slots
        if (Input.GetKeyDown(KeyCode.L))
        {
            GenerateItems();
        }
    }

    //  This only replaces or respawns items, not slots
    private void GenerateItems()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Slot slot = slots[i];

            // Destroy any existing item first
            if (slot.currentItem != null)
            {
                Destroy(slot.currentItem);
                slot.currentItem = null;
            }

            // Pick a random item prefab (or cycle through)
            if (itemPrefabs.Length > 0)
            {
                GameObject itemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
                GameObject newItem = Instantiate(itemPrefab, slot.transform);

                // Center it inside the slot
                RectTransform rect = newItem.GetComponent<RectTransform>();
                if (rect != null) rect.anchoredPosition = Vector2.zero;

                newItem.tag = "Food";
                slot.currentItem = newItem;
            }
        }

        Debug.Log($" Reused {slots.Count} slots, generated new items!");
    }
}
