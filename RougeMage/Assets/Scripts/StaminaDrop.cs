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
            Mathf.Clamp(GameManager.Instance.playerScript.stamina += staminaAmount, 0, GameManager.Instance.playerScript.staminaOrig);
            GameManager.Instance.playerScript.updatePlayerStaminaUI();
            aud.PlayOneShot(audCollected, audCollectedVol);
            Destroy(gameObject);
        }
    }
}
