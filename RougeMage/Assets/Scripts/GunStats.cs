using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class GunStats : ScriptableObject
{
    public float shootDamage;
    public float shootDist;
    public float shootRate;
    public int ammoCurr;
    public int ammoMax;
    public bool Electric;

    public GameObject model;
    public ParticleSystem hitEffect;
    public AudioClip shootSound;
    [Range(0f, 1f)] public float shootSoundVol;

    void Start()
    {
        ammoCurr = ammoMax;
        shootDamage = 1;
    }
}
