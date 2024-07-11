using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using TMPro;
using Cinemachine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    public HealthBar healthbar;

    public int currentHealth;

    public int maxHealth;

    //public int damageAmount;

    public float immortalTime;
    private float immortalCounter;

    private SpriteRenderer sr;

    private Vector2 checkPoint;

    private FMOD.Studio.EventInstance damages;
    private FMOD.Studio.EventInstance dead;
    private FMOD.Studio.EventInstance respawn;

    public float timeToRespawn = 1f;
    public bool isDead;

    private Animator anim;

    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        Instance = this;
    }



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isDead = false;
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(currentHealth);
        sr = GetComponent<SpriteRenderer>();
        checkPoint = transform.position;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (immortalCounter > 0)
        {
            immortalCounter -= Time.deltaTime;
            
        }
        else
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        }
    }

    public void DealDamage(int damageAmount)
    {
        

        //Debug.Log("immortal counter = " + immortalCounter);

        if (immortalCounter <= 0) 
        {
            currentHealth -= damageAmount;
            PlayDamagedSound();
            CameraShakeManager.instance.CameraShake(impulseSource);
            healthbar.SetHealth(currentHealth);


            if (currentHealth <= 0 && !isDead)
            {

                isDead = true;
                anim.SetBool("isDead", isDead);
                //Debug.Log(isDead);

                StartCoroutine(RespawnCounter());
                StartCoroutine(CorpseActiveCounter());
                PlayDeadSound();

                
            }
            else
            {
                immortalCounter = immortalTime;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.6f);
                //Debug.Log("immortal counter = " + immortalCounter);
            }
        }
        
    }

    IEnumerator RespawnCounter()
    {
        if (isDead)
        {
          
            yield return new WaitForSeconds(timeToRespawn);

            Respawn();

        }
  
    }

    IEnumerator CorpseActiveCounter()
    {
        if (isDead)
        {
          
            yield return new WaitForSeconds(2f);

            //gameObject.SetActive(false);

        }
    }

        public void UpdatedCheckPoint(Vector2 pos)
    {
        checkPoint = pos;
    }
    void Respawn()
    {
        if (isDead)
        {
            //gameObject.SetActive(true);
            transform.position = checkPoint;
            currentHealth = maxHealth;
            healthbar.SetHealth(maxHealth);
            PlayRespawnSound();
        }
        isDead = false;
        anim.SetBool("isDead", isDead);

     



    }

    private void PlayDamagedSound()
    {
        damages = FMODUnity.RuntimeManager.CreateInstance("event:/DamageToPlayer");
        damages.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        damages.start();
        damages.release();
    }

    private void PlayDeadSound()
    {
        dead = FMODUnity.RuntimeManager.CreateInstance("event:/DeathToPlayer");
        dead.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        dead.start();
        dead.release();
    }

    private void PlayRespawnSound()
    {
        respawn = FMODUnity.RuntimeManager.CreateInstance("event:/Respawn");
        respawn.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        respawn.start();
        respawn.release();
    }
}