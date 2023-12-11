using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ButtonFunction : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    GoldDrop golddrop;

    public void Resume()
    {
        GameManager.Instance.stateUnpause();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.Instance.stateUnpause();
    }

    public void dungeonRespawn()
    {
        SceneManager.LoadScene("Character Select");
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
            GameManager.Instance.playerScript.maxMana += (int)(GameManager.Instance.playerScript.maxMana / 4);
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
            golddrop.goldAmount *= 2;
        }
    }

    //Spells
    public void FireFlare()
    {
        if (GameManager.Instance.playerScript.gold >= 100)
        {
            GameManager.Instance.playerScript.gold -= 100;
            playerController.setFireFlareStats();
        }
    }

    public void ElectricCharge()
    {
        if (GameManager.Instance.playerScript.gold >= 100)
        {
            GameManager.Instance.playerScript.gold -= 100;
            playerController.setElectricChargeStats();
        }
    } 
    
    public void RockCatapult()
    {
        if (GameManager.Instance.playerScript.gold >= 100)
        {
            GameManager.Instance.playerScript.gold -= 100;
            playerController.SetRockCatapultStats();
        }
    }
    
    public void JetStream()
    {
        if (GameManager.Instance.playerScript.gold >= 100)
        {
            GameManager.Instance.playerScript.gold -= 100;
            playerController.SetJetStreamStats();
        }
    }

    public void SettingsUI()
    {
        GameManager.Instance.settingsUI.SetActive(true);
    }

    public void Done()
    {
        GameManager.Instance.settingsUI.SetActive(false);
    }

    public void CloseDungeon()
    {
        GameManager.Instance.dungeonPrompt.GetComponentInChildren<TMP_Text>().enabled = false;
        GameManager.Instance.dungeonPrompt.GetComponent<Image>().enabled = false;
        GameManager.Instance.buttonYes.GetComponent<Image>().enabled = false;
        GameManager.Instance.buttonYes.GetComponentInChildren<TMP_Text>().enabled = false;
        GameManager.Instance.buttonNo.GetComponent<Image>().enabled = false;
        GameManager.Instance.buttonNo.GetComponentInChildren<TMP_Text>().enabled = false;
        GameManager.Instance.stateUnpause();
    }
    
    public void CreditOpen()
    {
        GameManager.Instance.statePause();
        GameManager.Instance.CreditOpen();
    }

    public void CreditNext()
    {
        GameManager.Instance.creditNext();
    }

    public void CreditPrevious()
    {
        GameManager.Instance.creditPrev();
    }

    public void CreditClose()
    {
        GameManager.Instance.stateUnpause();
    }    
}
