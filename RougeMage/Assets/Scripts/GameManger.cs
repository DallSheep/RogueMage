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
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuDungeon;
    [SerializeField] GameObject menuInteract;
    [SerializeField] GameObject charSelect;
    [SerializeField] GameObject blockedWall;
    [SerializeField] GameObject playerDamageScreen;
    [SerializeField] TMP_Text enemyCountText;
    public float timescaleOrig;

    [Header("----- Player Components -----")]
    public Image playerHPBar;
    public Image playerManaBar;
    public Image playerStaminaBar;
    public GameObject GoldCount;

    public GameObject playerSpawnPos;
    public GameObject player;
    public PlayerController playerScript;

    //Door Stuff
    [Header("----- Door Components -----")]
    
    public ColliderPrompts prompt;


    public bool isPaused;
    public int enemiesRemaining;

    [Header("----- Character Components -----")]
    //public CharacterSelection characterSelection;
    [SerializeField] public BoxCollider blockedPath;
    [SerializeField] public BoxCollider blockedTrigger;
    [SerializeField] public BoxCollider charTrigger;
    public bool charSelected;
    public bool isPrompt;



    void Awake()
    {
        Instance = this;
        timescaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        playerSpawnPos = GameObject.FindWithTag("Respawn");
        
        prompt = blockedTrigger.GetComponent<ColliderPrompts>();
        charSelected = false;
        //characterSelection = characterSelection.fireMage.GetComponent<CharacterSelection>();
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
        //enemyCountText.text = enemiesRemaining.ToString("0");

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
        
    }

    public void CharacterEntry()
    {
        menuActive = menuInteract;
        menuActive.SetActive(true);
    }

    public void CharPrompts()
    {
        Debug.Log("Got here");
        if(prompt.gameObject.CompareTag("Block Collider") && isPrompt)
        {
            menuActive = blockedWall;
            menuActive.SetActive(true);
        }
        else
        {
            Debug.Log(prompt.gameObject.tag);
            menuActive = charSelect;
            menuActive.SetActive(true);
        }
    }
    public void CharacterExit()
    {
        menuActive.SetActive(false);
    }
}
