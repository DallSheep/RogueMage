using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ButtonFunction : MonoBehaviour
{
    public void Resume()
    {
        GameManager.Instance.dungeonPrompt.GetComponentInChildren<TMP_Text>().enabled = false;
        GameManager.Instance.dungeonPrompt.GetComponent<Image>().enabled = false;
        GameManager.Instance.buttonYes.GetComponent<Image>().enabled = false;
        GameManager.Instance.buttonYes.GetComponentInChildren<TMP_Text>().enabled = false;
        GameManager.Instance.buttonNo.GetComponent<Image>().enabled = false;
        GameManager.Instance.buttonNo.GetComponentInChildren<TMP_Text>().enabled = false;
        GameManager.Instance.stateUnpause();
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.Instance.stateUnpause();
    }

    public void Respwan()
    {
        GameManager.Instance.playerScript.spawnPlayer();
        GameManager.Instance.stateUnpause();
        GameManager.Instance.doorScript.doorAnimation.SetInteger("doorStop", 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadNextScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Items
    public void HeroesHeart()
    {
        if (GameManager.Instance.playerScript.gold >= 25)
        {
           //GameManager.Instance.heroesHeart.GetComponentInChildren<TMP_Text>().enabled = true;
           GameManager.Instance.playerScript.gold -= 25;
           GameManager.Instance.playerScript.Hp += (int)(GameManager.Instance.playerScript.Hp / 4);
            
        }
    }

    public void PlasmaBall()
    {
        if (GameManager.Instance.playerScript.gold >= 25)
        {
            GameManager.Instance.playerScript.gold -= 25;
            GameManager.Instance.playerScript.currMana += (int)(GameManager.Instance.playerScript.currMana / 4);
        }
    }

    public void DragonsAle() 
    {
        if (GameManager.Instance.playerScript.gold >= 25)
        {
            GameManager.Instance.playerScript.gold -= 25;
            GameManager.Instance.playerScript.stamina += 100;
        }
    }

    public void DwarvenRing() 
    {
        if (GameManager.Instance.playerScript.gold >= 25)
        {
            GameManager.Instance.playerScript.gold -= 25;

        }
    }

    //Spells
    public void FireFlare()
    {
        if (GameManager.Instance.playerScript.gold >= 100)
        {
            GameManager.Instance.playerScript.gold -= 100;
        }
    }

    public void ElectricCharge()
    {
        if (GameManager.Instance.playerScript.gold >= 100)
        {
            GameManager.Instance.playerScript.gold -= 100;
        }
    } 
    
    public void RockCatapult()
    {
        if (GameManager.Instance.playerScript.gold >= 100)
        {
            GameManager.Instance.playerScript.gold -= 100;
        }
    }
    
    public void JetStream()
    {
        if (GameManager.Instance.playerScript.gold >= 100)
        {
            GameManager.Instance.playerScript.gold -= 100;
        }
    }
}
