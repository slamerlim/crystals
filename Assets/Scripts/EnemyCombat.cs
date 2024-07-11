using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using FMODUnity;
using FMOD.Studio;

public class EnemyCombat : MonoBehaviour
{
    public Transform attackPoint;

    public float attackRadius = 1f;
    public float attackCoolDown = 1f;
    private float nextAttackTime = 0f;

    public LayerMask playerLayer;
    public int playerDamageAmount = 2;
    private FMOD.Studio.EventInstance rips;
    private FMOD.Studio.EventInstance whooshes;
    public Animator anim;
    private int attackId;
    private enum CURRENT_ATTACK
    {
        ATTACK0, ATTACK1, ATTACK2
    };
    private CURRENT_ATTACK currentAttack;

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerLayer);
        foreach (Collider2D player in hitPlayers)
        {
            PlayerHealth playerhealth = player.GetComponent<PlayerHealth>();
            
            //check if script attached
            if (playerhealth != null && !playerhealth.isDead && Time.time >= nextAttackTime) 
            {
                anim.SetTrigger("Attack");

                if (gameObject.CompareTag("Lancer") || gameObject.CompareTag("Swordsman"))
                {
                    anim.SetInteger("AttackIndex", Random.Range(0, 2));
                    attackId = anim.GetInteger("AttackIndex");


                    //Debug.Log(attackId);

                    if (attackId == 0)
                    {
                        currentAttack = CURRENT_ATTACK.ATTACK0;
                    }
                    else if (attackId == 1)
                    {
                        currentAttack = CURRENT_ATTACK.ATTACK1;
                    }
                }

                else if (gameObject.CompareTag("SoilDemon"))
                {
                    anim.SetInteger("AttackIndex", Random.Range(0, 3));
                    attackId = anim.GetInteger("AttackIndex");


                    Debug.Log(attackId);

                    if (attackId == 0)
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
                }

                if (!anim.GetBool("isDead"))
                {
                    playerhealth.DealDamage(playerDamageAmount);
                    PlayRipSound();
                    //nextAttackTime = Time.time + 1f / attackCoolDown;
                    nextAttackTime = Time.time + attackCoolDown;
                }
            }
            
        }
    }

    private void PlayRipSound()
    {
        rips = FMODUnity.RuntimeManager.CreateInstance("event:/Rips");
        rips.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        rips.start();
        rips.release();
    }

    public void SelectAndPlayWoosh()
    {
        if (gameObject.CompareTag("SoilDemon"))
        {
            switch (currentAttack)
            {
                case CURRENT_ATTACK.ATTACK0:
                    PlayWhooshSoilSound(0);
                    break;

                case CURRENT_ATTACK.ATTACK1:
                    PlayWhooshSoilSound(1);
                    break;

                case CURRENT_ATTACK.ATTACK2:
                    PlayWhooshSoilSound(2);
                    break;
            }
        }

        else {
            switch (currentAttack)
            {
                case CURRENT_ATTACK.ATTACK0:
                    PlayWhooshSound(0);
                    break;

                case CURRENT_ATTACK.ATTACK1:
                    PlayWhooshSound(1);
                    break;
            }
        }
    }

    private void PlayWhooshSoilSound(int whoosh)
    {
        whooshes = FMODUnity.RuntimeManager.CreateInstance("event:/WhooshesSoilDemon");
        whooshes.setParameterByName("Whooshes", whoosh);
        whooshes.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        whooshes.start();
        whooshes.release();
    }

    private void PlayWhooshSound(int whoosh)
    {
        whooshes = FMODUnity.RuntimeManager.CreateInstance("event:/WhooshesPlayer");
        whooshes.setParameterByName("Whooshes", whoosh);
        whooshes.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        whooshes.start();
        whooshes.release();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position,attackRadius);
    }
}
