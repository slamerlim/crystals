using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using FMODUnity;
using FMOD.Studio;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D rb;
    private float horizontalInput;

    public float jumpForce;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayers;
    private bool isGrounded;

    private SpriteRenderer sr;

    private Animator anim;


    public Transform attackPoint;
    public float attackRadius;
    public LayerMask enemyLayers;

    public float attackCoolDown = 1f;
    private float nextAttackTime = 0f;
    private int attackId;
    private enum CURRENT_ATTACK { ATTACK0, ATTACK1, ATTACK2, ATTACK3, ATTACK4, ATTACK5 };

    private CURRENT_ATTACK currentAttack;
    private FMOD.Studio.EventInstance whooshes;
    private FMOD.Studio.EventInstance jumps;
    private FMOD.Studio.EventInstance rips; 
    private FMOD.Studio.EventInstance dashes;

    public LayerMask bossLayers;

    //dash
    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 20f;
    public float dashingTime = 0.2f;
    public float dashingCoolDown = 1f;
    public TrailRenderer tr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }

        PlayerHealth playerhealth = gameObject.GetComponent<PlayerHealth>();

        if (!PauseMenu.Instance.isPaused && !playerhealth.isDead)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers);
            horizontalInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.y, jumpForce);
                PlayJumpSound();
            }

            if (canDash)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    StartCoroutine("Dash");
                }
            }

            //changing directions of Player
            if (rb.velocity.x < 0)
            {
                //flipping the sprite, but not the Attack point
                //sr.flipX = true;

                //flipping the sprite and Attack point as well
                sr.transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x > 0)
            {
                //flipping the sprite, but not the Attack point
                //sr.flipX = false;

                //flipping the sprite and Attack point as well
                sr.transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
            }

            //attacking enemies
            if (Input.GetKeyDown(KeyCode.F) | Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
            {
                anim.SetInteger("AttackIndex", Random.Range(0,5));
                
                attackId = anim.GetInteger("AttackIndex");

                if(attackId == 0)
                {
                    currentAttack = CURRENT_ATTACK.ATTACK0;
                }
                else if (attackId == 1)
                {
                    currentAttack = CURRENT_ATTACK.ATTACK1;
                }
                else if (attackId == 2)
                {
                    currentAttack = CURRENT_ATTACK.ATTACK2;
                }
                else if (attackId == 3)
                {
                    currentAttack = CURRENT_ATTACK.ATTACK3;
                }
                else if (attackId == 4)
                {
                    currentAttack = CURRENT_ATTACK.ATTACK4;
                }
                Attack();
                BossAttack();
                nextAttackTime = Time.time + 1f / attackCoolDown;
            }

            


        }

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    void Attack()
    {
        anim.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {

            enemy.GetComponent<Enemy>().TakeDamage();
            PlayRipSound();
        }
        
    }

    void BossAttack()
    {
        anim.SetTrigger("Attack");
        Collider2D[] hitBossEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, bossLayers);
        foreach (Collider2D bossenemy in hitBossEnemies)
        {
            bossenemy.GetComponent<Boss>().TakeDamage();
            PlayRipSound();
        }
        
    }

    public void SelectAndPlayWoosh()
    {
        switch (currentAttack)
        {
            case CURRENT_ATTACK.ATTACK0:
                PlayWhooshSound(0);
                break;

            case CURRENT_ATTACK.ATTACK1:
                PlayWhooshSound(1);
                break;

            case CURRENT_ATTACK.ATTACK2:
                PlayWhooshSound(2);
                break;

            case CURRENT_ATTACK.ATTACK3:
                PlayWhooshSound(3);
                break;

            case CURRENT_ATTACK.ATTACK4:
                PlayWhooshSound(4);
                break;

            default:
                PlayWhooshSound(0);
                break;
        }
    }

    private void PlayWhooshSound(int whoosh)
    {
        whooshes = FMODUnity.RuntimeManager.CreateInstance("event:/WhooshesPlayer");
        whooshes.setParameterByName("Whooshes", whoosh);
        whooshes.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        whooshes.start();
        whooshes.release();
    }

    private void PlayRipSound()
    {
        rips = FMODUnity.RuntimeManager.CreateInstance("event:/Rips");
        rips.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        rips.start();
        rips.release();
    }


    private void PlayJumpSound()
    {
        jumps = FMODUnity.RuntimeManager.CreateInstance("event:/JumpsPlayer");
        jumps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        jumps.start();
        jumps.release();
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        float dashingDirection = Mathf.Sign(transform.localScale.x);
        rb.velocity = new Vector2(dashingDirection * dashingPower, 0f);
        tr.emitting = true;
        PlayDashSound();
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;

        //dashing is over, let's return things back to normal
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCoolDown);
        canDash = true;
    }

    private void PlayDashSound()
    {
        dashes = FMODUnity.RuntimeManager.CreateInstance("event:/Dash");
        dashes.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        dashes.start();
        dashes.release();
    }

    //show attackRadius in Scene preveiw
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}