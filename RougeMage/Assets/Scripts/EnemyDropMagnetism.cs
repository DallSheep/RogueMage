using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDropMagnetism : MonoBehaviour
{

    [Range(0, 1)][SerializeField] float magnetizationSpeed;

    bool finished;
    bool playerInRange;
    bool followPlayer;
    public PlayerController player;
    public GameObject skeletonEnemy;
    public GameObject goblinEnemy;
    public GameObject spiderEnemy;

    void Start()
    {
        finished = false;
    }


    void Update()
    {
        if (gameObject.transform.childCount == 0)
        {
            Destroy(gameObject);
        }
        else if(playerInRange && !followPlayer)
        {
            
            StartCoroutine(move(player.transform.position, magnetizationSpeed));

        }
        else if (GameManager.Instance.isBossDead == true)
        {
            skeletonEnemy = GameObject.FindWithTag("Skeleton Enemy");
            goblinEnemy = GameObject.FindWithTag("Goblin Enemy");
            spiderEnemy = GameObject.FindWithTag("Spider Enemy");
            Destroy(skeletonEnemy);
            Destroy(goblinEnemy);
            Destroy(spiderEnemy);
            GameManager.Instance.isPaused = true;
            transform.position = Vector3.Lerp(transform.position, GameManager.Instance.player.transform.position, 0.005f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.GetComponent<PlayerController>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }


    IEnumerator move(Vector3 destination, float duration)
    {
        followPlayer = true;
        Vector3 start = transform.position;
        float speed = 1f / duration;

        for(float i = 0; i < 1; i += Time.deltaTime * speed)
        {
            transform.position = Vector3.Lerp(start, destination, i);
            yield return null;
        }
        transform.position = destination;
        followPlayer = false;
    }

}
