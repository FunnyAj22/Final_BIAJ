using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Customer Settings")]
    public GameObject customerPrefab;       // What customer to spawn
    public Transform spawnPoint;           // Where they appear
    public float minDelay = 15f;
    public float maxDelay = 30f;

    private bool gameStarted = false;

    void Start()
    {
        StartCoroutine(StartCustomerFlow());
    }

    IEnumerator StartCustomerFlow()
    {
        // Spawn FIRST customer immediately
        SpawnCustomer();

        gameStarted = true;

        // Keep spawning customers forever
        while (gameStarted)
        {
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);

            SpawnCustomer();
        }
    }

    void SpawnCustomer()
    {
        Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log("Customer spawned!");
    }
}
