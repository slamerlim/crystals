using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class Pickups : MonoBehaviour
{
    private FMOD.Studio.EventInstance pickups;

    public Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "SCORE: " + Scoring.totalScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gem"))
        {
            Scoring.totalScore++;
            scoreText.text = "SCORE: " + Scoring.totalScore;
            PlayPickupSound();
            Destroy(collision.gameObject);

        }
    }

    private void PlayPickupSound()
    {
        pickups = FMODUnity.RuntimeManager.CreateInstance("event:/Pickups");
        pickups.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        pickups.start();
        pickups.release();
    }
}
