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
    [SerializeField] GameObject playerDamageScreen;
    [SerializeField] TMP_Text enemyCountText;

    [Header("----- Player Components -----")]
    public Image playerHPBar;
    public Image playerManaBar;
    public Image playerDashBar;

    public GameObject playerSpawnPos;
    public GameObject player;
    public PlayerController playerScript;
    //public Vector3 playerTransform;

    //Door Stuff
    [Header("----- Door Components -----")]
    public Doors doors;
    public GameObject doorWithColliders;
    public BoxCollider noTriggerCollider;

    public bool isPaused;
    float timescaleOrig;
    public int enemiesRemaining;

    [Header("----- Character Components -----")]
    public CharacterSelection characterSelection;
    [SerializeField] public GameObject selectMage;



    void Awake()
    {
        Instance = this;
        timescaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        playerSpawnPos = GameObject.FindWithTag("Respawn");
        doorWithColliders = GameObject.FindWithTag("Door");
        noTriggerCollider = doorWithColliders.GetComponentInChildren<BoxCollider>();
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
        //Debug.Log(noTriggerCollider.tag);
        if (noTriggerCollider.CompareTag("Start Collider"))
        {
            //Debug.Log("uhhhhh");
            statePause();
            //Debug.Log(menuActive);
            menuActive = menuDungeon;
            menuActive.SetActive(true);
        }
        else
        {
            //Debug.Log("okkkkk");
            //Debug.Log("paused?");
            doors.isLocked = false;
            doors.unlockedDoor.gameObject.SetActive(true);

            //This is for making the door interactable and letting you walk through by destroying the collider
            if (Input.GetButtonDown("Interact") && 
                noTriggerCollider.CompareTag("Door Collider"))
            {
                doors.doorAnimation.Play("DoorOpen", 0, 0.0f);
                noTriggerCollider.GetComponentInChildren<BoxCollider>().enabled = false;
            }
        }
    }

    public void CharacterEntry()
    {
        menuActive = menuInteract;
        menuActive.SetActive(true);
    }
    public void CharacterExit()
    {
        menuActive.SetActive(false);
    }

    public void ChangeModel()
    {
        //Vector3 newPlayerTransform = new Vector3(playerTransform.x, 1.0f, playerTransform.y);
        GameObject thisModel = Instantiate(selectMage, player.transform.position/*,newPlayerTransform*/, player.transform.rotation);
        thisModel.GetComponent<SphereCollider>().enabled = false;
        thisModel.transform.parent = transform;
        selectMage = thisModel;

        playerScript.soulOrb.SetActive(false);

        //player.GetComponent<Animator>().enabled = true;
        //player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;

        //player.GetComponent<Animator>().runtimeAnimatorController = 
        //    selectMage.GetComponent<Animator>().runtimeAnimatorController;

        //player.GetComponentInChildren<SkinnedMeshRenderer>().material =
        //    selectMage.GetComponentInChildren<SkinnedMeshRenderer>().material;

        //player.

        //if (player.GetComponentInChildren<MeshRenderer>().gameObject.CompareTag("Weapon") 
        //    && selectMage.GetComponentInChildren<MeshRenderer>().gameObject.CompareTag("Weapon"))
        //{
        //    player.GetComponentInChildren<MeshRenderer>().material =
        //    selectMage.GetComponentInChildren<MeshRenderer>().material;
        //}
    }
}
