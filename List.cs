using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class List : MonoBehaviour
{
    List<string> _numbers = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        _numbers.Add("Edward");
        _numbers.Add("Don");
        _numbers.Add("Alex");

        _numbers.Remove("Edward");

        _numbers.Insert(1, "Fred");

        Debug.Log(_numbers[1]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
