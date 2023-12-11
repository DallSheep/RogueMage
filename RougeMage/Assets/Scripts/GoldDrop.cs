using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDrop : MonoBehaviour
{
    [Header("----- Stats -----")]
    [SerializeField] int goldAmount;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audCollected;
    [Range(0, 1)][SerializeField] float audCollectedVol;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            goldAmount = 10;
            player.updatePlayerGoldUI(goldAmount);
            aud.PlayOneShot(audCollected, audCollectedVol);
            Destroy(gameObject);
        }
    }
}
