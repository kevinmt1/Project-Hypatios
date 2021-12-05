using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class health : MonoBehaviour
{

    [SerializeField] float maxHealth = 100f;
    public float curHealth;
    public float healPerSecond;
    float healthAfterHeal = 0f;
    public Text healthPoint;
    public bool isDead;
    public SlowMotion slow;

    //Die
    public Volume postProcess;
    private DepthOfField dof;
    public characterScript character;
    Animator anim;

    public GameManager gameManager;
    float timeAfterDeath = 0f;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
        postProcess.sharedProfile.TryGet<DepthOfField>(out dof);
        dof.focalLength.value = 1f;
        anim = character.anim;
        slow.ReturnTime();
    }

    void Update()
    {
        healthPoint.text = "" + curHealth;
        if(Input.GetKeyDown(KeyCode.J))
        {
            curHealth -= 20;
        }
        if (curHealth <= 0f)
        {
            Die();
            isDead = true;
        }
    }

    public void Heal(int healNum)
    {
        curHealth = Mathf.Clamp(curHealth + healNum, 0f, 100f);
    }
    public void takeDamage(int damage)
    {
        if (healthAfterHeal > 0)
        {
            healthAfterHeal -= damage;
        }
        curHealth -= damage;
        
    }

    public void Die()
    {
        
        postProcess.sharedProfile.TryGet<DepthOfField>(out dof);
        slow.SlowMo();
        if (dof.focalLength.value < 150f)
        {
            dof.focalLength.value += Time.unscaledDeltaTime * 50f;
        }

        timeAfterDeath += Time.unscaledDeltaTime;
        if (timeAfterDeath > 4f)
        {  
            Debug.Log("isCalled");
            gameManager.RestartLevel();
        }
        
    }
}
