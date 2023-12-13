using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] objectToSpawn;
    [SerializeField] int numberToSpawn;
    [SerializeField] int timeBetweenSpawns;
    [SerializeField] Transform[] spawnPos;

    int prevSpawnPos;
    int spawnCount;
    int randomNum;
    bool isSpawning;
    bool startSpawning;

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && !isSpawning && spawnCount < numberToSpawn)
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
        isSpawning = true;

        int randomNum = Random.Range(0, spawnPos.Length);

        for (int i = 0; i < spawnPos.Length; i++)
        {
            Instantiate(objectToSpawn[Random.Range(0, objectToSpawn.Length)], spawnPos[i].position, spawnPos[i].rotation);

            spawnCount++;

            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        isSpawning = false;
    }
}
