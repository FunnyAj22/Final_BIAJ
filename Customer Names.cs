using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomerNames: MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private InventoryController_FinishedOrder orderController;

    private static readonly string[] Names = { "Daniel", "Edward", "Alex" };
    private static int currentNameIndex = 0;
    private string _currentName;

    private void Start()
    {
        AssignUniqueName();
        RefreshGui();
        GenerateOrderForCustomer();
    }

    private void AssignUniqueName()
    {
        int index = currentNameIndex % Names.Length;
        currentNameIndex++;
        _currentName = Names[index];
    }

    private void RefreshGui()
    {
        textMeshProUGUI.text = _currentName;
    }

    private void GenerateOrderForCustomer()
    {
        // Example: random order size
        int itemCount = 3;

       // Random.Range(2, 3);

        List<int> randomVals = new List<int>();
        for (int i = 0; i < itemCount; i++)
        {
            randomVals.Add(Random.Range(1, 3)); // assuming 0–4 possible food types
           
        }

        // Send order to InventoryController
        orderController.SetupCustomerOrder(_currentName, randomVals);
    }
}
