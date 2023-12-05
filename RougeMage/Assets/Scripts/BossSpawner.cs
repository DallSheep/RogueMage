using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] objectToSpawn;
    [SerializeField] int numberToSpawn;
    [SerializeField] int timeBetweenSpawns;
    [SerializeField] Transform[] spawnPos;

    [Header("----- Camera -----")]
    public GameObject mainCamera;
    public PlayerController playerController;

    int spawnCount;
    bool isSpawning;
    bool startSpawning;

    void Start()
    {
        playerController = GameManager.Instance.player.GetComponent<PlayerController>();
        mainCamera = GameObject.FindWithTag("MainCamera");
    }

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
        if(other.isTrigger)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            Vector3 targetPos = new Vector3(playerController.transform.position.x * playerController.playerVelocity.x, playerController.transform.position.y + 30, playerController.transform.position.z * playerController.playerVelocity.z);
            mainCamera.GetComponent<CameraPosition>().height = targetPos;

            startSpawning = true;
            GameManager.Instance.bossHPBackground.GetComponent<Image>().enabled = true;
            GameManager.Instance.bossHPBar.GetComponent<Image>().enabled = true;
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }

    IEnumerator spawn()
    {

        isSpawning = true;

        int randomNum = Random.Range(0, spawnPos.Length);

        Instantiate(objectToSpawn[Random.Range(0, objectToSpawn.Length)], spawnPos[randomNum].position, spawnPos[randomNum].rotation);
        spawnCount++;

        yield return new WaitForSeconds(timeBetweenSpawns);

        isSpawning = false;
    }
}
