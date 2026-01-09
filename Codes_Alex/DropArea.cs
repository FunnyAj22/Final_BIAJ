using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropArea : MonoBehaviour
{
    public InventoryController_FinishedOrder parentController;
    public Order_Slot GetTargetSlot(int foodVal)
    {
        if (parentController != null)
        {
            return parentController.GetOpenSlotForFood(foodVal);
        }
        return null;
    }
}
