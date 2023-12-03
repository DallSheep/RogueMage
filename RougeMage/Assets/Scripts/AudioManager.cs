using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audMainMusic;
    [Range(0, 1)][SerializeField] float audMainMusicVol;

    void Start()
    {
        aud.PlayOneShot(audMainMusic, audMainMusicVol);
    }
}
