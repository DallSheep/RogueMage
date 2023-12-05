using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlarePickup : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] FireFlareBullet bullet;
    [SerializeField] GameObject button;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.setFireFlareStats();
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
