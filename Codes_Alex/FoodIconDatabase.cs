using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodIconDatabase", menuName = "Game/Food Icon Database")]
public class FoodIconDatabase : ScriptableObject
{
    [System.Serializable]
    public class FoodIconEntry
    {
        public int foodVal;          // e.g. 0 = burger, 1 = fries, etc.
        public Sprite icon;          // the picture to show
    }

    public List<FoodIconEntry> icons = new List<FoodIconEntry>();

    private static FoodIconDatabase _instance;
    public static FoodIconDatabase Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<FoodIconDatabase>("FoodIconDatabase");
            return _instance;
        }
    }

    public Sprite GetIcon(int foodVal)
    {
        foreach (var entry in icons)
            if (entry.foodVal == foodVal)
                return entry.icon;
        return null;
    }
}
