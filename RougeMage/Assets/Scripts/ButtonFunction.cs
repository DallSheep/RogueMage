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
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadNextScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        GameManager.Instance.isPaused = !GameManager.Instance.isPaused;
        GameManager.Instance.playerScript.enabled = true;
        Time.timeScale = GameManager.Instance.timescaleOrig;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void BuyMe()
    {
        if(GameManager.Instance.playerScript.gold > 25)
        {
            GameManager.Instance.playerScript.gold -= 25;


        }
    }
}
