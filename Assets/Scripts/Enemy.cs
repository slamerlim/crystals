using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Transform pointA, pointB;
    public int speed;
    private Vector3 currentTarget;
    private SpriteRenderer sr;

    public int currentHealth;
    public int maxHealth;
    public int damageAmount;

    private Animator anim;


    
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == pointA.position)
        {
            currentTarget = pointB.position;
            sr.flipX = false;
        }
        else if (transform.position == pointB.position)
        {
            currentTarget = pointA.position;
            sr.flipX = true;
        }
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
        
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
        anim.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Debug.Log("Enemy down");
    }
}
