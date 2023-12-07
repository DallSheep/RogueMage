using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    [Header("----- Status Effect -----")]
    public int statusEffectDamage;
    public float damageCount;
    public float timeIntervalStatusEffect;

    [Header("----- Particles -----")]
    public ParticleSystem statusEffectParticles;

    FireStatusEffect fireStatusEffect;
    AcidStatusEffect acidStatusEffect;

    int statusEffectCount;
    IDamage damageable;

    private void Start()
    {
        damageable = GameManager.Instance.player.GetComponent<IDamage>();
        fireStatusEffect = GameManager.Instance.player.GetComponent<FireStatusEffect>();
        acidStatusEffect = GameManager.Instance.player.GetComponent<AcidStatusEffect>();
    }

    public void StartDamage()
    {
        //Instantiate(statusEffectParticles, transform.position, transform.rotation);
        StartCoroutine(Damage());
        //Destroy(statusEffectParticles);
    }

    public IEnumerator Damage()
    {
        statusEffectCount = 0;
        while (true)
        {
            if (statusEffectCount == damageCount)
            {
                statusEffectDamage = 0;
                damageCount = 0;
                timeIntervalStatusEffect = 0;
                Destroy(GameManager.Instance.player.GetComponent<FireStatusEffect>());
                Destroy(GameManager.Instance.player.GetComponent<AcidStatusEffect>());
                break;
            }

            statusEffectCount++;

            damageable.takeDamage(statusEffectDamage);
            GameManager.Instance.playerScript.updatePlayerHealthUI();
            yield return new WaitForSeconds(timeIntervalStatusEffect);
        }
    }
}
