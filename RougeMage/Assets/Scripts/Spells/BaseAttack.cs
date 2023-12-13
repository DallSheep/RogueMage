using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    
    public IEnumerator Attack()
    {
        GameManager.Instance.playerScript.isShooting = true;
        GameManager.Instance.playerScript.playerAnim.SetBool("isAttacking", true);

        if (GameManager.Instance.playerScript.currMana >= GameManager.Instance.playerScript.manaCost)
        {
            GameManager.Instance.playerScript.currMana -= GameManager.Instance.playerScript.manaCost;
            Ray ray = new Ray(transform.position, transform.forward);
            Vector3 targetPoint;

            targetPoint = ray.GetPoint(50);

            Vector3 shootDir = targetPoint - GameManager.Instance.playerScript.shootPos.position;

            if (GameManager.Instance.playerScript.bulletMain != null)
            {
                GameObject currBullet = Instantiate(GameManager.Instance.playerScript.bulletMain, GameManager.Instance.playerScript.shootPos.position 
                    + GameManager.Instance.playerScript.playerVelocity, Quaternion.identity);
                currBullet.transform.forward = shootDir.normalized;
            }
            yield return new WaitForSeconds(.1f);
            GameManager.Instance.playerScript.playerAnim.SetBool("isAttacking", false);
            yield return new WaitForSeconds(GameManager.Instance.playerScript.shootRate);
        }
        GameManager.Instance.playerScript.isShooting = false;
        GameManager.Instance.playerScript.playerAnim.SetBool("isAttacking", false);
    }

    public void SetStats()
    {
        switch (GameManager.Instance.playerScript.finalMage.tag)
        {
            case "Fire Mage":
                GameManager.Instance.playerScript.bulletMain = GameManager.Instance.playerScript.bulletFire;
                GameManager.Instance.playerScript.shootRate = 0.8f;
                GameManager.Instance.playerScript.shootDamage = 8;
                GameManager.Instance.playerScript.shootDist = 5;
                GameManager.Instance.playerScript.playerSpeed = 8;
                GameManager.Instance.playerScript.manaCost = 1;
                break;
            case "Water Mage":
                GameManager.Instance.playerScript.bulletMain = GameManager.Instance.playerScript.bulletWater;
                GameManager.Instance.playerScript.shootRate = 1.2f;
                GameManager.Instance.playerScript.shootDamage = 12;
                GameManager.Instance.playerScript.shootDist = 15;
                GameManager.Instance.playerScript.playerSpeed = 8;
                GameManager.Instance.playerScript.manaCost = 1;
                break;
            case "Lightning Mage":
                GameManager.Instance.playerScript.bulletMain = GameManager.Instance.playerScript.bulletLightning;
                GameManager.Instance.playerScript.shootRate = 0.4f;
                GameManager.Instance.playerScript.shootDamage = 4;
                GameManager.Instance.playerScript.shootDist = 10;
                GameManager.Instance.playerScript.playerSpeed = 10;
                GameManager.Instance.playerScript.manaCost = 5;
                break;
            case "Earth Mage":
                GameManager.Instance.playerScript.bulletMain = GameManager.Instance.playerScript.bulletEarth;
                GameManager.Instance.playerScript.shootRate = 0.4f;
                GameManager.Instance.playerScript.shootDamage = 12;
                GameManager.Instance.playerScript.shootDist = 10;
                GameManager.Instance.playerScript.playerSpeed = 6;
                GameManager.Instance.playerScript.manaCost = 10;
                break;
        }
    }
}
