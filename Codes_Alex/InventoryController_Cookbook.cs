using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryController_Cookbook : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount = 10;
    public GameObject[] itemPrefabs;
    public Canvas cookbookCanvas;

    [Header("Key Binding for this Cookbook")]
    public KeyCode openKey = KeyCode.G;  // assign unique key in Inspector

    private List<CookbookSlot> slots = new List<CookbookSlot>();
    private static List<InventoryController_Cookbook> allCookbooks = new List<InventoryController_Cookbook>();
    private bool isOpen = false;

    public bool IsOpen => isOpen;  // allow other scripts to read state

    void Awake()
    {
        if (!allCookbooks.Contains(this))
            allCookbooks.Add(this);
    }

    void OnDestroy()
    {
        allCookbooks.Remove(this);
    }

    void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            CookbookSlot newSlot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<CookbookSlot>();
            slots.Add(newSlot);
        }

        HideItemsInstant();
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(openKey))
        {
            Debug.Log("Key pressed: " + openKey);
            if (isOpen)
                CloseCookbook();
            else
                OpenCookbook();
        }
    }

    public void OpenCookbook()
    {
        // Close all other cookbooks instantly
        foreach (var cb in allCookbooks)
        {
            if (cb != this)
                cb.CloseCookbook(true);
        }

        isOpen = true;
        inventoryPanel.SetActive(true);

        // Clear old items and respawn new ones
        foreach (var slot in slots)
        {
            if (slot.currentItem != null)
            {
                // Only destroy if it is a cookbook item not an order slot item
                DragAndDrop dd = slot.currentItem.GetComponent<DragAndDrop>();
                if (dd != null && dd.currentSlot == null) // not placed in an order slot
                {
                    Destroy(slot.currentItem);
                    slot.currentItem = null;
                }
            }
        }

        GenerateItems();
        FadeInItems();
    }

    public void CloseCookbook(bool instant = false)
    {
        if (!isOpen) return;
        isOpen = false;

        if (instant)
        {
            HideItemsInstant();
            inventoryPanel.SetActive(false);
        }
        else
        {
            StartCoroutine(FadeOutItems());
        }
    }

    // ---------------- Item Handling ----------------

    private void GenerateItems()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            CookbookSlot slot = slots[i];

            if (itemPrefabs.Length > 0)
            {
                GameObject prefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
                GameObject newItem = Instantiate(prefab, slot.transform);


                RectTransform rect = newItem.GetComponent<RectTransform>();
                if (rect != null) rect.anchoredPosition = Vector2.zero;

                CanvasGroup cg = newItem.GetComponent<CanvasGroup>();
                if (cg == null) cg = newItem.AddComponent<CanvasGroup>();
                cg.alpha = 0f;

                slot.currentItem = newItem;
            }
        }
    }

    private IEnumerator FadeOutItems(float duration = 0.4f)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / duration);
            foreach (var slot in slots)
            {
                if (slot.currentItem != null && slot.currentItem.transform.parent == slot.transform)
                {
                    SetAlpha(slot.currentItem, alpha);
                }
            }
            yield return null;
        }

        foreach (var slot in slots)
        { 
            Debug.Log("Changing slot object: ", slot.currentItem);

            if (slot.currentItem != null && slot.currentItem.transform.parent == slot.transform)
            SetAlpha(slot.currentItem, 0f);

        }
    inventoryPanel.SetActive(false);
    }

    private void FadeInItems(float duration = 0.4f)
    {
        StartCoroutine(FadeInRoutine(duration));
    }

    private IEnumerator FadeInRoutine(float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / duration);
            foreach (var slot in slots)
            {
                if (slot.currentItem != null && slot.currentItem.transform.parent == slot.transform)
                {
                    SetAlpha(slot.currentItem, alpha);
                }
            }
            yield return null;
        }

        foreach (var slot in slots)
            if (slot.currentItem != null)
                SetAlpha(slot.currentItem, 1f);
    }

    private void HideItemsInstant()
    {
        Debug.Log("hideiteminstant");

        foreach (var slot in slots)
        {
            if (slot.currentItem != null)
            {
               
                if (slot.currentItem != null && slot.currentItem.transform.parent == slot.transform)
                {
                    Debug.Log("Changing slot object: ", slot.currentItem);
                    SetAlpha(slot.currentItem, 0f);
                }
            }
        }

    }
    private void SetAlpha(GameObject obj, float alpha)
    {
       
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null) cg = obj.AddComponent<CanvasGroup>();
        cg.alpha = alpha;
       
    }      
}
