using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    [Header("Food Sprites by ID (0–9)")]
    public Sprite Baguette_Concept;
    public Sprite Coke_Concept;
    public Sprite Fish_Concept;
    public Sprite Hambone_Concept;
    public Sprite Pancake_Concept;
    public Sprite Pie_Concept;
    public Sprite Pizza_Concept;
    public Sprite Splurg_Concept;
    public Sprite Squid_Stew_Concept;
    public Sprite Taco_Concept;

    private List<Sprite> spriteList = new List<Sprite>();

    private void Awake()
    {
        // Fill list so we can fetch sprites by index
        spriteList.AddRange(new Sprite[]
        {
            Baguette_Concept,
            Coke_Concept,
            Fish_Concept,
            Hambone_Concept,
            Pancake_Concept,
            Pie_Concept,
            Pizza_Concept,
            Splurg_Concept,
            Squid_Stew_Concept,
            Taco_Concept
        });
        spriteList.RemoveAll(s => s == null);
    }

    /// <summary>
    /// Returns the sprite for a given food value ID.
    /// </summary>
    public Sprite GetFoodSprite(int foodVal)
    {
        if (foodVal < 0 || foodVal >= spriteList.Count)
        {
            Debug.LogWarning($"Food: Invalid foodVal {foodVal}");
            return null;
        }
        return spriteList[foodVal];
    }

    /// <summary>
    /// Assigns sprites directly to given slot images to show the order.
    /// </summary>
    public void DisplayOrder(List<int> foodValues, List<Image> slotImages)
    {
        for (int i = 0; i < slotImages.Count; i++)
        {
            if (i < foodValues.Count)
            {
                int val = foodValues[i];
                Sprite icon = GetFoodSprite(val);
                slotImages[i].sprite = icon;
                slotImages[i].color = icon ? Color.white : new Color(1, 1, 1, 0); // transparent if missing
            }
            else
            {
                slotImages[i].sprite = null;
                slotImages[i].color = new Color(1, 1, 1, 0);
            }
        }
    }
}
