using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunction : MonoBehaviour
{
    public void Resume()
    {
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
        if(GameManager.Instance.playerScript.gold >= 25)
        {
            GameManager.Instance.playerScript.gold -= 25;
            GameManager.Instance.playerScript.Hp += (GameManager.Instance.playerScript.Hp/4);
        }
    }
}
