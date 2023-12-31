using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;



public class EnemyAIBoss : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] Animator anim;
    [SerializeField] Collider damageCol;
    [SerializeField] AudioSource aud;

    [Header("----- Enemy Stats -----")]
    [Range(0, 100)][SerializeField] int HP;
    [Range(1, 20)][SerializeField] int playerFaceSpeed;
    [SerializeField] int viewCone;
    [SerializeField] int shootCone;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;
    public bool isBossDefeated;
    public bool dead;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audAttack;
    [Range(0, 1)][SerializeField] float audAttackVol;
    [SerializeField] AudioClip audDragonAwakens;
    [Range(0, 1)][SerializeField] float audDragonAwakensVol;
    [SerializeField] AudioClip[] audFootSteps;
    [Range(0, 1)][SerializeField] float audFootStepsVol;
    [Range(0, 3)][SerializeField] float timeBetweenSteps;
    [SerializeField] AudioClip[] audHurt;
    [Range(0,1)][SerializeField] float audHurtVol;
    [SerializeField] AudioClip audDragonRageEnter;
    [Range(0, 1)][SerializeField] float audDragonRageEnterVol;
    [SerializeField] AudioClip[] audDeath;
    [Range(0, 1)][SerializeField] float audDeathVol;
    [SerializeField] AudioClip audPlayerWins;
    [Range(0, 1)][SerializeField] float audPlayerWinsVol;

    [Header("----- Gun Stats -----")]
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [Range(0, 4)][SerializeField] float shootRateMod;
    [SerializeField] GameObject Bullet1;
    [Range(0, 4)][SerializeField] float shootRate1;
    [Range(0, 5)][SerializeField] float randomAttackIntervalTime;

    [Header("----- Drop on Death -----")]
    [SerializeField] List<GameObject> groundItems;

    Vector3 playerDir;
    Vector3 startingPos;
    bool isShooting;
    bool playerInRange;
    bool destinationChosen;
    bool isAttacking;
    bool inRageMode;
    bool inRageTransition;
    public bool isPlayingSteps;
    float angleToPlayer;
    float stoppingDistOrig;
    float HPOrig;
    float halfHP;
    PlayerController player;

    void Start()
    {
        dead = false;
        HPOrig = HP;
        halfHP = HPOrig / 2;

        updateBossHealthUI();

        aud.PlayOneShot(audDragonAwakens, audDragonAwakensVol);

        GameManager.Instance.UpdateGameGoal(1);
        stoppingDistOrig = agent.stoppingDistance;
        startingPos = transform.position;
    }

    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            if (agent.remainingDistance > agent.stoppingDistance && !isPlayingSteps)
            {
                StartCoroutine(playSteps());
            }

            if (agent.isActiveAndEnabled)
            {
                anim.SetFloat("Speed", agent.velocity.normalized.magnitude);

                if (playerInRange && !canSeePlayer())
                {
                    StartCoroutine(roam());
                }
                else if (!playerInRange)
                {
                    StartCoroutine(roam());
                }
            }
        }
    }

    IEnumerator roam()
    {
        if (!inRageTransition)
        {
            if (agent.remainingDistance < 0.05f && !destinationChosen)
            {
                destinationChosen = true;
                agent.stoppingDistance = 0;

                yield return new WaitForSeconds(roamPauseTime);

                Vector3 randomPos = Random.insideUnitSphere * roamDist;
                randomPos += startingPos;

                NavMeshHit hit;
                NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
                agent.SetDestination(hit.position);

                destinationChosen = false;
            }
        }
    }

    bool canSeePlayer()
    {
        if (agent.isActiveAndEnabled)
        {
            if (!inRageTransition)
            {
                playerDir = (GameManager.Instance.player.transform.position - headPos.position).normalized;
                angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

                Debug.DrawRay(headPos.position, playerDir);
               
                RaycastHit hit;

                if (Physics.Raycast(headPos.position, playerDir, out hit))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        agent.stoppingDistance = stoppingDistOrig;

                        if (angleToPlayer <= viewCone)
                        {
                            if (angleToPlayer <= shootCone && !isShooting)
                            {
                                StartCoroutine(shoot());
                            }

                            if (agent.remainingDistance < agent.stoppingDistance)
                            {
                                faceTarget();
                            }

                            agent.SetDestination(GameManager.Instance.player.transform.position);

                            return true;
                        }
                    }
                }
            }
        }
        return false;

    }

    IEnumerator playSteps()
    {
        isPlayingSteps = true;

        aud.PlayOneShot(audFootSteps[Random.Range(0, audFootSteps.Length)], audFootStepsVol);

        yield return new WaitForSeconds(timeBetweenSteps);

        isPlayingSteps = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            agent.stoppingDistance = 0;
            playerInRange = false;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        anim.SetTrigger("isShooting");

        aud.PlayOneShot(audAttack[Random.Range(0, audAttack.Length)], audAttackVol);
        CreateBullet();

        //using transform.rotation will shoot the bullet wherever the enemy is pointing
        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }

    public void CreateBullet()
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            aud.PlayOneShot(audDeath[Random.Range(0, audDeath.Length)], audDeathVol);
            damageCol.enabled = false;
            agent.enabled = false;
            GameManager.Instance.UpdateGameGoal(-1);
            GameManager.Instance.bossHPBackground.GetComponent<Image>().enabled = false;
            GameManager.Instance.bossHPBar.GetComponent<Image>().enabled = false;
            dead = true;
            anim.SetBool("isDead", true);
            GameManager.Instance.UpdateWinCondition(dead);
            StartCoroutine(DestroyDeadBody());
            SpawnDrops();
        }
        else
        {
            aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
            agent.SetDestination(GameManager.Instance.player.transform.position);
        }

        if (HP <= halfHP && !inRageMode)
        {
            inRageMode = true;

            anim.SetBool("isEnteringRage", true);

            StartCoroutine(DragonRageTransition());

            shootRate /= shootRateMod;
        }

        StartCoroutine(flashRed());
        updateBossHealthUI();
    }

    public void SpawnDrops()
    {
        if (tag == "Dragon Boss")
        {
            int dropCounter = 0;
            int dropsToSpawn = Random.Range(5, 10);

            while (dropCounter != dropsToSpawn)
            {
                dropCounter++;
                Instantiate(groundItems[Random.Range(0, groundItems.Count)], new Vector3(Random.Range(transform.position.x + 1, transform.position.x + 5), transform.position.y, Random.Range(transform.position.z + 1, transform.position.z + 5)), transform.rotation);
            }
        }
    }

    IEnumerator DragonRageTransition()
    {
        inRageTransition = true;

        aud.PlayOneShot(audDragonRageEnter, audDragonRageEnterVol);

        yield return new WaitForSeconds(4);

        anim.SetBool("isEnteringRage", false);

        inRageTransition = false;
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    public IEnumerator DestroyDeadBody()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
        GameManager.Instance.audioScript.PlayAudio(audPlayerWins, audPlayerWinsVol);
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    public void updateBossHealthUI()
    {
        GameManager.Instance.bossHPBar.fillAmount = (float)HP / HPOrig;
    }
}
