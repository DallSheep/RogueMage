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
        GameManager.Instance.playerScript.spawnPlayer();
    }

    public void BuyMe()
    {
       
        if (GameManager.Instance.playerScript.gold >= 25)
        {
           //GameManager.Instance.heroesHeart.GetComponentInChildren<TMP_Text>().enabled = true;
           GameManager.Instance.playerScript.gold -= 25;
           GameManager.Instance.playerScript.Hp += (GameManager.Instance.playerScript.Hp / 4);
            
        }
    }
}
