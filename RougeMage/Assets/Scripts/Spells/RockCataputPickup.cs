using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCataputPickup : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] RockCatapultBullet bullet;
    [SerializeField] GameObject button;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.SetRockCatapultStats();
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
