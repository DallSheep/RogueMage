using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] objectToSpawn;
    [SerializeField] int timeBetweenSpawns;
    [SerializeField] Transform[] spawnPos;

    int spawnCount;
    bool isSpawning;
    bool startSpawning;

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && !isSpawning && spawnCount < spawnPos.Length)
        {
            StartCoroutine(spawn());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }

    IEnumerator spawn() 
    {
        for (int i = 0; i < spawnPos.Length; i++)
        {
            isSpawning = true;

            Instantiate(objectToSpawn[Random.Range(0, objectToSpawn.Length)], spawnPos[i].position, spawnPos[i].rotation);
            spawnCount++;

            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        isSpawning = false;
    }
}
