using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointParticles : MonoBehaviour
{
    private ParticleSystem particles;
    
    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        particles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            particles.Play();
        }
    }
}
