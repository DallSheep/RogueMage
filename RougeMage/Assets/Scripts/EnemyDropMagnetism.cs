using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDropMagnetism : MonoBehaviour
{

    [Range(0, 1)][SerializeField] float magnetizationSpeed;

    bool playerInRange;
    bool followPlayer;
    public PlayerController player;

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
