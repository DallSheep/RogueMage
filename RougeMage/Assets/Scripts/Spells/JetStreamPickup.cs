using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetStreamPickup : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] JetStreamBullet bullet;
    [SerializeField] GameObject button;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.SetJetStreamStats();
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            button.SetActive(false);
        }
    }
}
