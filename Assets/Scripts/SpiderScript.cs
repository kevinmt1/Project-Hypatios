using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class SpiderScript : MonoBehaviour
{
    public float maxHealth;
    public float curHealth;
    public float damage;

    Transform player;
    NavMeshAgent enemyAI;
    Vector3 targetPos;

    bool isAttacking = false;
    bool isWalking = false;

    public ParticleSystem laserCharging;
    public GameObject laser;
    public GameObject eyeLocation;
    public ParticleSystem deadPS;
    public GameObject dissolveEffect;
    public GameObject body;

    public float attackRange = 30f;
    public float attackTime;
    public float attackRecharge;
    public float lockBeforeAttackTime;
    float curLockTime = 0f;
    float nextAttackTime = 0f;
    float count = 0f;
    float colorSet = 1f;
    float dissolve = -1f;

    bool canLookAtPlayer = false;
    bool isCharging = false;
    bool hasShot = false;
    bool hasTargetted = false;
    Vector3 lockPos;
    bool hasInstanced = false;
    Vector3 colorVector;
    bool isDie = false;
    public float dieHeight;
    float afterDeathTime = 0f;
    bool dissolved = false;
    public List<Material> spiderMat;
    bool haveSeenPlayer;

    public LayerMask playerMask;

    public OpenDoor door;

    public SpawnHeal spawnHeal;

    public SpawnIndicator spawn;


    // Start is called before the first frame update
    void Start()
    {
        colorSet = 1f;
        curHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyAI = GetComponent<NavMeshAgent>();
        colorVector = new Vector3(1f, 1f, 1f);
        spiderMat = body.GetComponent<Renderer>().materials.ToList();
        spiderMat[2].SetVector("_ColorSet", colorVector);
        foreach (Material m in spiderMat)
        {
            m.SetFloat("_dissolve", dissolve);
        }
        spawnHeal = GetComponent<SpawnHeal>();
        spawn = GameObject.FindGameObjectWithTag("SpawnIndicator").GetComponent<SpawnIndicator>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyAI.updateRotation = false;
        float distance = Vector3.Distance(transform.position, player.position);
        Debug.DrawLine(transform.position, player.position);
        if (!isDie && distance < attackRange)
        {
            if (Physics.Raycast(transform.position, player.position - transform.position, out RaycastHit hit, distance))
            {
                if (hit.transform.tag == "Player")
                {
                    haveSeenPlayer = true;
                    Debug.DrawLine(transform.position, player.position - transform.position);
                }
               
            }
        }

        if (haveSeenPlayer && !isDie)
        {
            
            //Vector3 targetPlayer = new Vector3(player.position.x, Mathf.Clamp(player.position.y, -15f, 15f), player.position.z);

            transform.LookAt(player.position);

            Ray ray = new Ray(eyeLocation.transform.position, player.position - eyeLocation.transform.position);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (hit.transform.tag == "Player")
                {
                    canLookAtPlayer = true;
                }
                else
                {
                    canLookAtPlayer = false;   
                }
            }


            if (!canLookAtPlayer)
            {
                FindPosition();
                isAttacking = false;
                var em = laserCharging.emission;
                em.enabled = false;
                count = 0;
                isCharging = false;
            }
            else
            {
                enemyAI.SetDestination(transform.position);
                isAttacking = true;
                isWalking = false;
            }

            if (isAttacking && !isDie)
            {
                Attack();
            }
        }

        if (curHealth <= 0f)
        {
            Die();
        }
     }

    void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            count += Time.deltaTime;
            if (!isCharging)
            {
                laserCharging.Emit(1);
                isCharging = true;
            }

            if (count <= attackTime - .15f)
            {
                targetPos = player.position;
            }

            if (count >= attackTime)
            {
                
                hasTargetted = false;
                var points = new Vector3[2];
                points[0] = eyeLocation.transform.position;

                GameObject laserLine = Instantiate(laser, eyeLocation.transform.position, Quaternion.identity);
                points[1] = targetPos;
                var lr = laserLine.GetComponent<LineRenderer>();
                lr.SetPositions(points);
                hasShot = true;

                Ray ray = new Ray(eyeLocation.transform.position, targetPos - eyeLocation.transform.position);
                if (Physics.SphereCast(ray, .2f, out RaycastHit hit, 100f))
                {
                    if (hit.transform.tag == "Player")
                    {
                        hit.transform.gameObject.GetComponent<health>().takeDamage((int)damage);
                        spawn.Spawn(transform);
                    }
                    
                }

                nextAttackTime = Time.time + attackRecharge;
                isCharging = false;
                count = 0f;

            }
         
        }   
    }

    void FindPosition()
    {
        isWalking = true;
        isAttacking = false;
        if (isWalking)
            enemyAI.SetDestination(player.position);
    }

    public void Attacked(float damage)
    {
        haveSeenPlayer = true;
        curHealth -= damage;
    }

    void Die()
    {
        isAttacking = false;
        Destroy(laserCharging);
        afterDeathTime += Time.deltaTime;
        if (enemyAI.baseOffset > dieHeight)
        {
            enemyAI.baseOffset -= Time.deltaTime * 3f;
        }
        if (!isDie)
        {
            spawnHeal.SpawnHealCapsule(6);
            door.enemyLeft--;
            isDie = true;
        }
        
        if (colorSet > 0f)
        {
            colorSet -= Time.deltaTime;
        }
        colorVector = new Vector3(colorSet, colorSet, colorSet);
        spiderMat[2].SetVector("_ColorVector", colorVector);
        if (!hasInstanced)
        {
            deadPS.Emit(1);
            deadPS.Play();
            hasInstanced = true;
        }
        
        if (dissolve < 1f && afterDeathTime >= 1.5f)
        {
            if (!dissolved)
            {
                Instantiate(dissolveEffect, transform.position, Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
                dissolved = true;
            }
            
            dissolve += Time.deltaTime;
        }
        foreach(Material m in spiderMat)
        {
            m.SetFloat("_dissolve", dissolve);
        }
        if (dissolve >= 1f)
        {
            
            Destroy(gameObject);
        }
        
    }
}
