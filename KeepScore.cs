using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class KeepScore : MonoBehaviour
{

    public static int score;
    public TextMeshProUGUI scoreDisplay;


    // Start is called before the first frame update
    void Start()
    {
        score = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
        scoreDisplay.text = score.ToString("$: " + score);



    }


    public static void AddScore(int amount)
    {
        score += amount;
    }
}
