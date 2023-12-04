using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidStatusEffect : MonoBehaviour
{
    [Header("----- Stats -----")]
    public int acidStatusEffectDamage = 2;
    public float acidDamageCount = 5;
    public float acidTimeIntervalStatusEffect = 1;

    [Header("----- Status Effect -----")]
    GameObject statusEffect;

    void Start()
    {
        statusEffect = GameObject.FindWithTag("Status Effect");
        //statusEffect.GetComponent<StatusEffect>().statusEffectDamage = acidStatusEffectDamage;
        statusEffect.GetComponent<StatusEffect>().damageCount = acidDamageCount;
        statusEffect.GetComponent<StatusEffect>().timeIntervalStatusEffect = acidTimeIntervalStatusEffect;
    }
}
