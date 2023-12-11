using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("----- Components -----")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] public GameObject menuInventory;
    [SerializeField] GameObject menuShop;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject creditsOne;
    [SerializeField] GameObject creditsTwo;
    [SerializeField] GameObject creditsThree;
    [SerializeField] GameObject creditsFour;
    [SerializeField] GameObject creditsFive;
    [SerializeField] GameObject creditsSix;
    //[SerializeField] GameObject playerDamageScreen;
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] public GameObject settingsUI;
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
    public CharacterSelection selectChar;
    public GameObject mages;

    [Header("----- Boss Components -----")]
    [SerializeField] public Image bossHPBackground;
    public Image bossHPBar;
    public bool isBossDead;
    public GameObject boss;

    //Door Stuff
    [Header("----- Door Components -----")]
    public bool isPaused;
    public int enemiesRemaining;
    public GameObject doors;
    public Doors doorScript;
    

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
    [SerializeField] public GameObject oldMage;

    [Header("----- Audio -----")]
    public GameObject audioM;
    public AudioManager audioScript;

    //[Header("----- Items -----")]
    //[SerializeField] public GameObject heroesHeart;
    //[SerializeField] public TMP_Text heart;

    void Awake()
    {
        isBossDead = false;
        Instance = this;
        timescaleOrig = Time.timeScale;

        playerSpawnPos = GameObject.FindWithTag("Respawn");
        blockedTrigger = GameObject.FindWithTag("Block Collider");
        blockedPrompt = GameObject.FindWithTag("Blocked Wall Prompt");
        blockedWall = GameObject.FindWithTag("Blocked Wall");
        charSelect = GameObject.FindWithTag("Character Select");
        charTrigger = GameObject.FindWithTag("Character Trigger");
        dungeonPrompt = GameObject.FindWithTag("Dungeon");
        buttonYes = GameObject.FindWithTag("buttonYes");
        buttonNo = GameObject.FindWithTag("buttonNo");
        doors = GameObject.FindWithTag("Door Parent");
        doorScript = doors.GetComponentInChildren<Doors>();
        promptObj = GameObject.FindWithTag("Prompt Obj");
        prompt = promptObj.GetComponent<ColliderPrompts>();
        interactPrompt = GameObject.FindWithTag("Interact Prompt");
        goldCount = GameObject.FindWithTag("Gold");
        audioM = GameObject.FindWithTag("Audio Manager");
        audioScript = audioM.GetComponent<AudioManager>();
        //heroesHeart = GameObject.FindWithTag("HeroesHeart");
        //heart = heroesHeart.GetComponentInChildren<TMP_Text>();


    }


    void Update()
    {
        playerSpawnPos = GameObject.FindWithTag("Respawn");

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
        if (menuActive != null)
        {
            menuActive.SetActive(false);
            menuActive = null;
        }
        
    }

    public void UpdateGameGoal(int amount)
    {
        enemiesRemaining += amount;
        //enemyCountText.text = enemiesRemaining.ToString("0");

        if (enemiesRemaining <= 0)
        {
            //We can make a short and simple UI pop up to notify the player to move to the next room
        }
        else if(enemiesRemaining <= 0 && isBossDead)
        {

        }
    }

    public void youWin()
    {
        StartCoroutine(YouWin());
    }

    public IEnumerator YouWin()
    {
        yield return new WaitForSeconds(5);
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);
    }

    public void UpdateWinCondition(bool dead)
    {
        isBossDead = dead;
        if(dead == true)
        {
            youWin();
        }
    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    //public IEnumerator playerFlashDamage()
    //{
    //    playerDamageScreen.SetActive(true);
    //    yield return new WaitForSeconds(0.1f);
    //    playerDamageScreen.SetActive(false);
    //}

    public void ShopScreen()
    {
        statePause();
        menuActive = menuShop;
        menuActive.SetActive(true);
    }

    public void CreditOpen()
    {
        menuActive = creditsOne;
        menuActive.SetActive(true);
    }

    public void creditNext()
    {
        if(menuActive == creditsOne)
        {
            //menuActive = null;
            menuActive.SetActive(false);
            menuActive = creditsTwo;
            menuActive.SetActive(true);
        }
        else if(menuActive == creditsTwo)
        {
            menuActive.SetActive(false);
            menuActive = creditsThree;
            menuActive.SetActive(true);
        }
        else if (menuActive == creditsThree)
        {
            menuActive.SetActive(false);
            menuActive = creditsFour;
            menuActive.SetActive(true);
        }
        else if (menuActive == creditsFour)
        {
            menuActive.SetActive(false);
            menuActive = creditsFive;
            menuActive.SetActive(true);
        }
        else if (menuActive == creditsFive)
        {
            menuActive.SetActive(false);
            menuActive = creditsSix;
            menuActive.SetActive(true);
        }
        
    }

    public void creditPrev()
    {
        if (menuActive == creditsSix)
        {
            menuActive.SetActive(false);
            menuActive = creditsFive;
            menuActive.SetActive(true);
        }
        else if (menuActive == creditsFive)
        {
            menuActive.SetActive(false);
            menuActive = creditsFour;
            menuActive.SetActive(true);
        }
        else if (menuActive == creditsFour)
        {
            menuActive.SetActive(false);
            menuActive = creditsThree;
            menuActive.SetActive(true);
        }
        else if (menuActive == creditsThree)
        {
            menuActive.SetActive(false);
            menuActive = creditsTwo;
            menuActive.SetActive(true);
        }
        else if (menuActive == creditsTwo)
        {
            menuActive.SetActive(false);
            menuActive = creditsOne;
            menuActive.SetActive(true);
        }
    }

    public void creditClose()
    {
        
        menuActive.SetActive(false);
        menuActive = null;
        
    }
}
