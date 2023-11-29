using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaDrop : MonoBehaviour
{
    [Header("----- Stats -----")]
    [SerializeField] int staminaAmount;
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
            Mathf.Clamp(player.stamina += staminaAmount, 0, player.staminaOrig);
            player.updatePlayerStaminaUI();
            aud.PlayOneShot(audCollected, audCollectedVol);
            Destroy(gameObject);
        }
    }
}
