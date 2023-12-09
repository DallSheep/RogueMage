using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    void Start()
    {
        slider.onValueChanged.AddListener(val => GameManager.Instance.audioScript.ChangeMasterVolume(val));
    }
}
