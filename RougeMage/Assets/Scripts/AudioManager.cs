using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] AudioSource aud;

    public void PlayAudio(AudioClip clip, float vol)
    {
        //aud.Stop();
        aud.GetComponent<AudioSource>().clip = clip;
        aud.GetComponent<AudioSource>().volume = vol;
        aud.Play();
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }
}
