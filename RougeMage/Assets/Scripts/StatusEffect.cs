using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    [Header("===== Status Effect =====")]
    public int statusEffectDamage;
    public float damageCount;
    public float timeIntervalStatusEffect;
    FireStatusEffect fireStatusEffect;
    AcidStatusEffect acidStatusEffect;

    public int statusEffectCount;

    private void Update()
    {
        fireStatusEffect = GameManager.Instance.player.GetComponent<FireStatusEffect>();
        acidStatusEffect = GameManager.Instance.player.GetComponent<AcidStatusEffect>();
    }

    public void StartDamage()
    {
        StartCoroutine(Damage());
    }

    public IEnumerator Damage()
    {
        while (true)
        {
            if (statusEffectCount == damageCount || GameManager.Instance.player.GetComponent<PlayerController>().Hp == 1)
            {
                Destroy(GameManager.Instance.player.GetComponent<StatusEffect>());
                break;
            }

            statusEffectCount++;

            Mathf.Clamp(GameManager.Instance.player.GetComponent<PlayerController>().Hp - statusEffectDamage, 1, GameManager.Instance.player.GetComponent<PlayerController>().HPOrig);
            GameManager.Instance.player.GetComponent<PlayerController>().updatePlayerHealthUI();
            yield return new WaitForSeconds(timeIntervalStatusEffect);
        }
    }
}
