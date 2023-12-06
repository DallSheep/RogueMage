using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Rigidbody rb;
    [SerializeField] AudioSource aud;
    [SerializeField] Camera cam;
    [SerializeField] GameObject mousePos;
    [SerializeField] public Transform shootPos;
    [SerializeField] public GameObject soulOrb;
    [SerializeField] public GameObject selectMage;
    [SerializeField] public GameObject finalMage;
    [SerializeField] public GameObject model;
    [SerializeField] public GameObject root;

    [Header("----- Player Stats -----")]
    [Range(1, 10)][SerializeField] public int playerSpeed;
    [Range(3, 10)][SerializeField] int dashMod;
    [SerializeField] float dashCooldown;
    [Range(-10, -40)][SerializeField] float gravityValue;
    [SerializeField] public int Hp;
    [SerializeField] public float maxMana;
    [SerializeField] public float currMana; //Used just to see current mana, remove when done
    [SerializeField] public int stamina;
    [SerializeField] public int gold;
    [Range(0,5)] [SerializeField] float manaRegenSpeed;
    public bool isCharSlected;
    public bool isModelChanged;
    public Vector3 playerVelocity;
    private bool groundedPlayer;
    public bool isShooting;
    bool isPlayingSteps;
    bool isDashing;
    bool manaRegen;
    private float shootRateOrig;
    private Camera camOrig;
    public int HPOrig;
    public float manaOrig;
    public int staminaOrig;

    [Header("----- Spell Stats -----")]
    [SerializeField] SpellStats defaultSpell;

    [SerializeField] GameObject SpecialBullet;
    
    [SerializeField] float specialManaCost;
    [SerializeField] float specialCooldown;


    public BaseAttack baseAttacks;
    [SerializeField] public GameObject bulletMain;
    [SerializeField] public GameObject bulletFire;
    [SerializeField] public GameObject bulletWater;
    [SerializeField] public GameObject bulletLightning;
    [SerializeField] public GameObject bulletEarth;

    public float cooldown;
    public float manaCost;
    public int shootDamage;
    public float shootDist;
    public float shootRate;

    [SerializeField] public GameObject weapon;

    [Header("===== FireFlare Stats =====")]
    [SerializeField] GameObject FireFlareBullet;
    [SerializeField] float FireFlareManaCost;
    [SerializeField] float FireFlareCooldown;

    [Header("===== ElectricCharge Stats =====")]
    [SerializeField] GameObject ElectricChargeBullet;
    [SerializeField] float ElectricChargeManaCost;
    [SerializeField] float ElectricChargeCooldown;

    [Header("===== JetStream Stats =====")]
    [SerializeField] GameObject JetStreamBullet;
    [SerializeField] float JetStreamManaCost;
    [SerializeField] float JetStreamCooldown;

    [Header("===== EarthCatapult Stats =====")]
    [SerializeField] GameObject RockCatapultBullet;
    [SerializeField] float RockCatapultManaCost;
    [SerializeField] float RockCatapultCooldown;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audSteps;
    [Range(0, 1)][SerializeField] float audStepsVol;
    [SerializeField] AudioClip[] audDamage;
    [Range(0, 1)][SerializeField] float audDamageVol;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;

    [Header("----- Animation -----")]
    [SerializeField] public Animator playerAnim;
   

    public Vector3 move;
    public int isStarted;
    Vector3 newPlayerY;

    private void Start()
    {
        DontDestroyOnLoad(mousePos);
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(cam);

        //sets mana to full from start
        currMana = maxMana;
        manaOrig = currMana;
        

        isCharSlected = false;
        isModelChanged = false;
        staminaOrig = stamina;
        //goldOrig = gold;
        HPOrig = Hp;
        staminaOrig = stamina;
        //setSpellStats(defaultSpell);
        //shootRateOrig = shootRate;
        isStarted = 1;

        if (GameManager.Instance.playerSpawnPos != null)
        {
            spawnPlayer();
        }
    }

    void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            GameManager.Instance.playerSpawnPos = GameObject.FindWithTag("Respawn");
            transform.position = GameManager.Instance.playerSpawnPos.transform.position;

            movement();
            cameraMovement();

            if(Input.GetButton("Fire2") && !isShooting)
            {
                
                StartCoroutine(specialAttack());
            }

            if (Input.GetButton("Shoot") && !isShooting && isCharSlected)
            {
                StartCoroutine(baseAttacks.Attack());
            }

            updatePlayerManaUI();
            updatePlayerHealthUI();
            updatePlayerGoldUI(0);
            updatePlayerStaminaUI();

            //Regens mana when mana is not full
            if ((currMana < maxMana) && !manaRegen)
            {
                StartCoroutine(regenMana());
            }

            if (model.activeInHierarchy && isModelChanged)
            {
                baseAttacks.SetStats();
                isModelChanged = false;
            }

        }

        newPlayerY = new Vector3(transform.position.x, 0, transform.position.z);
    }

    void movement()
    {
        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

        if (stamina >= 100 && !isDashing)
        {
            StartCoroutine(Dash());
        }

        if (groundedPlayer && move.normalized.magnitude > 0.3 && !isPlayingSteps)
        {
            StartCoroutine(playSteps());
        }

        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            //jumpedTimes = 0;
        }

        move.z = Input.GetAxis("Vertical");
        move.x = Input.GetAxis("Horizontal");

        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (move.x > 0 || move.z > 0 || move.x < 0 || move.z < 0)
        {
            playerAnim.SetBool("isMoving", true);
        }
        else
        {
            playerAnim.SetBool("isMoving", false);
        }
    }
 

    void cameraMovement()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;


        //finds vector between the player and the mouse position
        Vector3 relative =  mousePos.transform.position;

        relative.y = transform.position.y;
        //looks at mouse position
        transform.LookAt(relative);

        
    }

    IEnumerator regenMana()
    {
        manaRegen = true;
        //Lower the faster regen speed
        yield return new WaitForSeconds(manaRegenSpeed);

        currMana++;

        manaRegen = false;
    }

    IEnumerator specialAttack()
    {
        isShooting = true;
        playerAnim.SetBool("isAttacking", true);

        if (currMana >= specialManaCost)
        {
            currMana -= manaCost;
            Ray ray = new Ray(transform.position, transform.forward);
            Vector3 targetPoint;

            targetPoint = ray.GetPoint(50);

            Vector3 shootDir = targetPoint - shootPos.position;

            if (SpecialBullet != null)
            {
                GameObject currBullet = Instantiate(SpecialBullet, shootPos.position, Quaternion.identity);
                currBullet.transform.forward = shootDir.normalized;
            }

            yield return new WaitForSeconds(specialCooldown);
        }
        isShooting = false;
        playerAnim.SetBool("isAttacking", false);
    }
    
    IEnumerator Dash()
    {
        if (stamina > 0)
        {
            if (Input.GetButton("Sprint"))
            {
                isDashing = true;
                //tracking original speed
                int ps = playerSpeed;
                playerSpeed *= dashMod;
                stamina = stamina - 100;

                yield return new WaitForSeconds(0.2f);
                playerSpeed = ps;

                yield return new WaitForSeconds(dashCooldown);
                isDashing = false;
            }
        }
    }

    IEnumerator playSteps()
    {

        isPlayingSteps = true;

        if (!isDashing)
            yield return new WaitForSeconds(0.5f);
        else if (isDashing)
            yield return new WaitForSeconds(0.3f);
        isPlayingSteps = false;
    }

    public void setFireFlareStats()
    {
        SpecialBullet = FireFlareBullet;
        specialCooldown = FireFlareCooldown;
        specialManaCost = FireFlareManaCost;
    }

    public void setElectricChargeStats()
    {
        SpecialBullet = ElectricChargeBullet;
        specialCooldown = ElectricChargeCooldown;
        specialManaCost = ElectricChargeManaCost;
    }

    public void SetJetStreamStats()
    {
        SpecialBullet = JetStreamBullet;
        specialCooldown = JetStreamCooldown;
        specialManaCost = JetStreamManaCost;
    }

    public void SetRockCatapultStats()
    {
        SpecialBullet = RockCatapultBullet;
        specialCooldown = RockCatapultCooldown;
        specialManaCost = RockCatapultManaCost;
    }
    public void setSpellStats(GameObject spellBullet)
    {
        switch (spellBullet.name)
        {
            case "FireFlareBullet":
                {
                    SpecialBullet = FireFlareBullet;
                    //specialCooldown = FireFlareBull;
                    Debug.Log("Fire");
                    break;
                }
            case "ElectricChargeBullet":
                {
                    SpecialBullet = ElectricChargeBullet;
                    Debug.Log("Lightning");
                    break;
                }
            case "JetStreamBullet":
                {
                    SpecialBullet = JetStreamBullet;
                    Debug.Log("Water");
                    break;
                }
            case "RockCatapultBullet":
                {
                    SpecialBullet = RockCatapultBullet;
                    Debug.Log("Earth");
                    break;
                }

        }
    }

    public void takeDamage(int amount)
    {
        playerAnim.SetBool("isHit", true);
        Hp -= amount;
        updatePlayerHealthUI();
        //aud.PlayOneShot(audDamage[Random.Range(0, audDamage.Length)], audDamageVol);
        //StartCoroutine(GameManager.Instance.playerFlashDamage());

        if (Hp <= 0)
        {
            playerAnim.SetBool("isDead", true);
            GameManager.Instance.youLose();
        }
        playerAnim.SetBool("isHit", false);
    }

    public void spawnPlayer()
    {
        playerAnim.SetBool("isDead", false);
        //controller.enabled = false;
        Hp = HPOrig;
        currMana = manaOrig;
        stamina = staminaOrig;
        
        updatePlayerHealthUI();
        updatePlayerManaUI();
        updatePlayerStaminaUI();
        //updatePlayerGoldUI(0);
        camOrig = cam;
        
        controller.enabled = false;
        
        transform.position = GameManager.Instance.playerSpawnPos.transform.position;
       
        controller.enabled = true;
        
        Time.timeScale = 1;
    }

    public void updatePlayerHealthUI()
    {
        GameManager.Instance.playerHPBar.fillAmount = (float)Hp / HPOrig;
    }

    public void updatePlayerManaUI()
    {
        GameManager.Instance.playerManaBar.fillAmount = (float)currMana / manaOrig;
    }

    public void updatePlayerStaminaUI()
    {
        GameManager.Instance.playerStaminaBar.fillAmount = (float)stamina / staminaOrig;
    }


    public void updatePlayerGoldUI(int amount)
    {
        gold += amount;
        
        GameManager.Instance.goldCount.GetComponent<TMP_Text>().text = gold.ToString("0");
    }


    public void ChangeModel()
    {

        isCharSlected = true;
        isModelChanged = true;

        soulOrb.SetActive(false);
        root.SetActive(true);
        model.SetActive(true);

        root.GetComponentInChildren<MeshRenderer>().material = selectMage.GetComponentInChildren<MeshRenderer>().material;

         model.GetComponent<SkinnedMeshRenderer>().rootBone = 
            selectMage.GetComponentInChildren<SkinnedMeshRenderer>().rootBone;

        model.GetComponent<SkinnedMeshRenderer>().sharedMesh =
            selectMage.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;

        model.GetComponent<SkinnedMeshRenderer>().material =
            selectMage.GetComponentInChildren<SkinnedMeshRenderer>().material;
        
        transform.position = newPlayerY;
        Debug.Log(isCharSlected);
        if(isCharSlected)
        {
            GameManager.Instance.blockedTrigger.SetActive(false);
            GameManager.Instance.charTrigger.SetActive(false);
            GameManager.Instance.blockedWall.SetActive(false);
            GameManager.Instance.oldMage.SetActive(false);
        }
    }
}