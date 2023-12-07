using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHallEntrance : MonoBehaviour
{
    [SerializeField] AudioClip audBossHallEnter;
    [Range(0, 1)][SerializeField] float audBossHallEnterVol;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            GameManager.Instance.audioScript.PlayAudio(audBossHallEnter, audBossHallEnterVol);
        }
    }
}
