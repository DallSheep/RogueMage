using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpellStats : ScriptableObject
{
    public float cooldown;
    public int damage;
    public int distance;
    public int manaCost;

    public ParticleSystem hitEffect;
    public AudioClip castSound;
    [Range(0, 1)] public float castSoundVol;
    public GameObject bullet;
}
