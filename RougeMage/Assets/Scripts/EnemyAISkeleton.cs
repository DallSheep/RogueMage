using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;



public class EnemyAISkeleton : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
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

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audAttack;
    [Range(0, 1)][SerializeField] float audAttackVol;
    [SerializeField] AudioClip[] audFootSteps;
    [Range(0, 1)][SerializeField] float audFootStepsVol;
    [Range(0, 3)][SerializeField] float timeBetweenSteps;
    [SerializeField] AudioClip[] audHurt;
    [Range(0,1)][SerializeField] float audHurtVol;
    [SerializeField] AudioClip[] audDeath;
    [Range(0, 1)][SerializeField] float audDeathVol;

    [Header("----- Sword Stuff -----")]
    [Range(1, 5)][SerializeField] float timeBetweenSwings;
    [SerializeField] public GameObject Sword1;
    [SerializeField] public GameObject Sword2;

    [Header("----- Drop on Death -----")]
    [SerializeField] List<GameObject> groundItems;

    Vector3 playerDir;
    Vector3 startingPos;
    bool playerInRange;
    bool destinationChosen;
    bool isAttacking;
    bool dead;
    public bool isPlayingSteps;
    float angleToPlayer;
    float stoppingDistOrig;
    float HPOrig;
    PlayerController player;

    void Start()
    {
        HPOrig = HP;
        GameManager.Instance.UpdateGameGoal(1);
        stoppingDistOrig = agent.stoppingDistance;
        startingPos = transform.position;
    }

    void Update()
    {
        if (!dead)
        {
            if (agent.remainingDistance > agent.stoppingDistance && !isPlayingSteps)
            {
                StartCoroutine(playSteps());
            }

            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);

            if (playerInRange && !canSeePlayer())
            {
                //StartCoroutine(roam());
            }
            else if (!playerInRange)
            {
                //StartCoroutine(roam());
            }
        }
    }

    IEnumerator roam()
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

    bool canSeePlayer()
    {
        if (!dead)
        {
            playerDir = (GameManager.Instance.player.transform.position - headPos.position);
            angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

            Debug.DrawRay(headPos.position, playerDir);
            //Debug.Log(angleToPlayer);

            RaycastHit hit;

            if (Physics.Raycast(headPos.position, playerDir, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    //agent.stoppingDistance = stoppingDistOrig;

                    if (angleToPlayer <= viewCone)
                    {
                        agent.SetDestination(GameManager.Instance.player.transform.position);

                        if (angleToPlayer <= shootCone && !isAttacking && agent.remainingDistance <= agent.stoppingDistance)
                        {
                            StartCoroutine(attack());
                        }

                        agent.SetDestination(GameManager.Instance.player.transform.position);

                        return true;
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
            //agent.stoppingDistance = 0;
            playerInRange = false;
        }
    }

    IEnumerator attack()
    {
        isAttacking = true;

        //aud.PlayOneShot(audAttack[Random.Range(0, audAttack.Length)], audAttackVol);

        anim.SetTrigger("isAttacking");

        yield return new WaitForSeconds(timeBetweenSwings);

        isAttacking = false;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            dead = true;
            damageCol.enabled = false;
            agent.enabled = false;
            Sword1.GetComponent<CapsuleCollider>().enabled = false;
            Sword2.GetComponent<CapsuleCollider>().enabled = false;
            GameManager.Instance.UpdateGameGoal(-1);
            SpawnDrops();
            aud.PlayOneShot(audDeath[Random.Range(0, audDeath.Length)], audDeathVol);
            anim.SetBool("isDead", true);
            StartCoroutine(DestroyDeadBody());
        }
        else
        {
            aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
            agent.SetDestination(GameManager.Instance.player.transform.position);
        }

        StartCoroutine(flashRed());        
    }
    public IEnumerator DestroyDeadBody()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    public void SpawnDrops()
    {
        int dropCounter = 0;
        int dropsToSpawn = Random.Range(1, 3);

        while (dropCounter != dropsToSpawn)
        {
            dropCounter++;
            Instantiate(groundItems[Random.Range(0, groundItems.Count)], new Vector3(Random.Range(transform.position.x + 1, transform.position.x + 3), transform.position.y, Random.Range(transform.position.z + 1, transform.position.z + 3)), transform.rotation);
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }
}
