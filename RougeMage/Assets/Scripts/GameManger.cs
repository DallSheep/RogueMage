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

    [Header("----- Components -----")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuInventory;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject playerDamageScreen;
    [SerializeField] TMP_Text enemyCountText;
    public float timescaleOrig;
    public GameObject interactPrompt;

    [Header("----- Player Components -----")]
    public Image playerHPBar;
    public Image playerManaBar;
    public Image playerStaminaBar;
    public GameObject goldCount;

    public GameObject playerSpawnPos;
    public GameObject player;
    public PlayerController playerScript;

    //Door Stuff
    [Header("----- Door Components -----")]
    public bool isPaused;
    public int enemiesRemaining;
    public GameObject door;
    Doors doorScript;

    [Header("----- Prompt Components -----")]
    public ColliderPrompts prompt;
    public GameObject promptObj;
    [SerializeField] public GameObject blockedTrigger;
    [SerializeField] public GameObject charTrigger;
    [SerializeField] public GameObject blockedPrompt;
    [SerializeField] public GameObject dungeonPrompt;
    [SerializeField] public GameObject charSelect;
    [SerializeField] public GameObject blockedWall;
    [SerializeField] public GameObject buttonYes;
    [SerializeField] public GameObject buttonNo;




    void Awake()
    {
        Instance = this;
        timescaleOrig = Time.timeScale;
        //StartCoroutine(PlayerSpawnDelay());
        playerSpawnPos = GameObject.FindWithTag("Respawn");
        blockedTrigger = GameObject.FindWithTag("Block Collider");
        blockedPrompt = GameObject.FindWithTag("Blocked Wall Prompt");
        blockedWall = GameObject.FindWithTag("Blocked Wall");
        charSelect = GameObject.FindWithTag("Character Select");
        dungeonPrompt = GameObject.FindWithTag("Dungeon");
        buttonYes = GameObject.FindWithTag("buttonYes");
        buttonNo = GameObject.FindWithTag("buttonNo");
        door = GameObject.FindWithTag("Door");
        doorScript = door.GetComponent<Doors>();
        promptObj = GameObject.FindWithTag("Prompt Obj");
        prompt = promptObj.GetComponent<ColliderPrompts>();
        interactPrompt = GameObject.FindWithTag("Interact Prompt");
        goldCount = GameObject.FindWithTag("Gold");
    }


    void Update()
    {
        if (Input.GetButtonDown("Cancel") && menuActive == null)
        {
            statePause();
            menuActive = menuPause;
            menuPause.SetActive(isPaused);
        }

        if (Input.GetButtonDown("Inventory") && menuActive == null)
        {
            statePause();
            menuActive = menuInventory;
            menuInventory.SetActive(isPaused);
        }
        

        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    //IEnumerator PlayerSpawnDelay()
    //{
    //    yield return new WaitForSeconds(0.5f);
        
    //}

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpause()
    {
        isPaused = !isPaused;
        playerScript.enabled = true;
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
            //StartCoroutine(youWin());
        }
    }

    //public IEnumerator youWin()
    //{
    //    yield return new WaitForSeconds(3);
    //    statePause();
    //    menuActive = menuWin;
    //    menuActive.SetActive(true);

    //}

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

}
