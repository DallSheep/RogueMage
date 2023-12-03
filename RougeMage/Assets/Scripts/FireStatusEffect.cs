using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStatusEffect : MonoBehaviour
{
    [Header("----- Stats -----")]
    public int fireStatusEffectDamage = 5;
    public float fireDamageCount = 3;
    public float fireTimeIntervalStatusEffect = 1;

    [Header("----- Status Effect -----")]
    GameObject statusEffect;

    void Start()
    {
        statusEffect = GameObject.FindWithTag("Status Effect");
        statusEffect.GetComponent<StatusEffect>().statusEffectDamage = fireStatusEffectDamage;
        statusEffect.GetComponent<StatusEffect>().damageCount = fireDamageCount;
        statusEffect.GetComponent<StatusEffect>().timeIntervalStatusEffect = fireTimeIntervalStatusEffect;
    }
}
