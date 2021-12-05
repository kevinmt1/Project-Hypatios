using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class B0MBScript : MonoBehaviour
{
    public cameraScript cam;
    public GameObject body;

    public float chaseDistance;
    public float distanceToExplode;
    public float curHealth;
    public float maxHealth;
    public float damage;
    public float moveSpeed;
    public float chaseSpeed;
    public float explosionRadius;
    public float explosionForce;

    public float highDamage = 50f;
    public float midDamage = 25f;
    public float lowDamage = 10f;
    public float highDistance = 2f;
    public float midDistance = 5f;
    public float lowDistance = 10f;

    NavMeshAgent enemyAI;
    GameObject player;
    Vector3 playerPos;

    public float attackRange;
    float distance;
    //public Renderer render;
    public Material mat1;
    float afterDeathTime = -1.5f;
    public ParticleSystem aura;
    public ParticleSystem spawnParticle;
    public ParticleSystem explosion;
    Animator anim;
    bool hasInstanced = false;
    bool gonnaExplode = false;
    float colorChange;
    public LayerMask playerMask;

    public List<Material> bombMat;
    GameObject[] bomb;
    public bool haveSeenPlayer;

    // Start is called before the first frame update
    void Start()
    {
        colorChange = 5f;
        chaseSpeed = moveSpeed + 3f;
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        enemyAI = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyAI.speed = moveSpeed;
        curHealth = maxHealth;
        bombMat = body.GetComponent<Renderer>().materials.ToList();
        foreach (Material m in bombMat)
        {
            m.SetFloat("_ColorChange", 5f);
        }
    }

    // Update is called once per frame
    void Update()
    {      
        playerPos = player.transform.position;
        distance = Vector3.Distance(transform.position, playerPos);
       

        if (distance <= attackRange && Physics.Raycast(transform.position, playerPos - transform.position, out RaycastHit hit, attackRange))
        {
            if (hit.transform.tag == "Player")
            {
                haveSeenPlayer = true;
            }    
        }

        

        if (haveSeenPlayer)
        {
            bomb = GameObject.FindGameObjectsWithTag("bomb");
            foreach (GameObject b in bomb)
            {
                if (Vector3.Distance(transform.position, b.transform.position) < attackRange)
                {
                    b.GetComponent<B0MBScript>().haveSeenPlayer = true;
                }
            }

            enemyAI.SetDestination(playerPos);

            if (distance < chaseDistance)
            {
                moveSpeed = chaseSpeed;
            }
            if (distance < distanceToExplode || curHealth <= 0f)
            {
                gonnaExplode = true;
            }

            if (gonnaExplode)
            {
                Explode();
            }
        }  
    }

    public void Attacked(float damage)
    {
        haveSeenPlayer = true;
        curHealth -= damage;
    }

    void Explode()
    {
        anim.SetBool("gonnaExplode", true);
        enemyAI.SetDestination(transform.position);
        if (colorChange >= -.5f)
        {
            colorChange -= Time.deltaTime * 8f;
            foreach (Material m in bombMat)
            {
                m.SetFloat("_ColorChange", colorChange);
            }
            
        }
        else
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider c in colliders)
            {
                Rigidbody obj = c.GetComponent<Rigidbody>();
                if (obj != null)
                {
                    obj.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }

                health character = c.GetComponent<health>();
                if (character != null)
                {
                    if (distance < highDistance)
                    {
                        damage = highDamage;
                        StartCoroutine(cam.Shake(.2f, .3f));
                    }
                    else if (distance < midDistance)
                    {
                        damage = midDamage;
                        StartCoroutine(cam.Shake(.2f, .2f));
                    }
                    else if (distance < lowDistance)
                    {
                        damage = lowDamage;
                        StartCoroutine(cam.Shake(.2f, .1f));
                    }
                    character.takeDamage((int)damage);
                }

            }

            Destroy(gameObject);
        }
    }
}
