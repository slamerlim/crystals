using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private GameObject player;

    public float detectingRange;
    
    public float timeBetweenAttacks = 0.5f;

    public LayerMask groundLayer;

    private Animator anim;

    public float moveSpeed;
    public float attackRange= 2f;

    private bool isAttacking = false;

    private bool isStuckedATWall = false;
    
    
    //health
    public int currentHealth;
    public int maxHealth;
    public int damageAmount;

 
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
 

        if (distance < detectingRange)
        {
            if (!isStuckedATWall)
            {
                anim.SetBool("PlayerInRange", true);
            }
            else
            {
                anim.SetBool("PlayerInRange", false);
            }
            if (distance < attackRange && !isAttacking)
            {
                StartCoroutine(AttackAfterDelay());
            }
            
            else if (!isAttacking)
            {
                MoveTowardsPlayer();
            }
        }
        else
        {
            anim.SetBool("PlayerInRange", false);
        }
    }

    //hacking the time of boss waits to attack again
    IEnumerator AttackAfterDelay()
    {
        isAttacking = true;
    

        
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
       

    }
    
    private void MoveTowardsPlayer()
    {
        //find Player's position (only X)
        Vector2 playerPosition = new Vector2(player.transform.position.x,transform.position.y);
        //calculate how far it is
        Vector2 direction = (playerPosition - (Vector2)transform.position).normalized;
        if (currentHealth > 0)
        {
            FlipSprite(direction.x);

            Collider2D[] hitWalls = Physics2D.OverlapCircleAll(gameObject.GetComponent<EnemyCombat>().attackPoint.position, gameObject.GetComponent<EnemyCombat>().attackRadius, groundLayer);

            if (hitWalls.Length < 1)
            {
                transform.Translate(direction * (moveSpeed * Time.deltaTime));
                isStuckedATWall = false;
            }
            else
            {
                isStuckedATWall = true;
            }


                
        }
        
    }


    private void FlipSprite(float directionX)
    {
        if (gameObject.CompareTag("SoilDemon")){
            if (directionX > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            else if (directionX < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        else
        {
            if (directionX > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
     
            }

            else if (directionX < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
        
            }
        }
    }

    public void TakeDamage()
    {
        currentHealth -= damageAmount;
    

        if (currentHealth <= 0)
        {
 
            Die();
        }
    }



    void Die()
    {
        anim.SetBool("isDead",true);
              
   
      
        Destroy(gameObject,2f);
       
    }
}
