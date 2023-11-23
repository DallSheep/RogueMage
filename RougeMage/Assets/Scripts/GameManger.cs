using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuDungeon; 
    [SerializeField] GameObject playerDamageScreen;
    [SerializeField] TMP_Text enemyCountText;

    public Image playerHPBar;

    public GameObject playerSpawnPos;
    public GameObject player;
    public PlayerController playerScript;

    //Door Stuff
    public Doors doors;
    public GameObject startDoorCollider;
    public GameObject regDoorCollider;

    public bool isPaused;
    float timescaleOrig;
    public int enemiesRemaining;


    void Awake()
    {
        Instance = this;
        timescaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        playerSpawnPos = GameObject.FindWithTag("Respawn");
        startDoorCollider = GameObject.FindWithTag("Start Collider");
        regDoorCollider = GameObject.FindWithTag("Door Collider");
    }


    void Update()
    {
        if (Input.GetButtonDown("Cancel") && menuActive == null)
        {
            statePause();
            menuActive = menuPause;
            menuPause.SetActive(isPaused);
        }
    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpause()
    {
        playerScript.enabled = true;
        isPaused = !isPaused;
        Time.timeScale = timescaleOrig;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void UpdateGameGoal(int amount)
    {
        enemiesRemaining += amount;
        enemyCountText.text = enemiesRemaining.ToString("0");

        if (enemiesRemaining <= 0)
        {
            StartCoroutine(youWin());
        }
    }

    public IEnumerator youWin()
    {
        yield return new WaitForSeconds(3);
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);

    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    public IEnumerator playerFlashDamage()
    {
        playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerDamageScreen.SetActive(false);
    }

    internal void ExitDoorCondition()
    {
       
    }

    public void DoorMenus()
    {
        if (startDoorCollider)
        {
            statePause();
            menuActive = menuDungeon;
            menuActive.SetActive(true);
        }
        else
        {
            stateUnpause();
            doors.isLocked = false;
            doors.unlockedDoor.gameObject.SetActive(true);

            //This is for making the door interactable and letting you walk through by destroying the collider
            if (Input.GetButtonDown("Interact") && regDoorCollider)
            {
                doors.doorAnimation.Play("DoorOpen", 0, 0.0f);
                Destroy(gameObject.GetComponentInChildren<BoxCollider>());
            }
        }
    }
}
