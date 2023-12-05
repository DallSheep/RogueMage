using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricChargePickup : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] ElectricBullet bullet;
    [SerializeField] GameObject button;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.setElectricChargeStats();
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
