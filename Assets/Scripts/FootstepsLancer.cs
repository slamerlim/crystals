using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class FootstepsLancer : MonoBehaviour
{

    private Rigidbody2D rb;
    private enum CURRENT_TERRAIN { STONE, WATER, SAND, DIRT, WOOD };

    [SerializeField]
    private CURRENT_TERRAIN currentTerrain;
    private FMOD.Studio.EventInstance foosteps;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //MaterialCheck();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Stone"))
        {
            currentTerrain = CURRENT_TERRAIN.STONE;
        }
        else if (other.gameObject.CompareTag("Water"))
        {
            currentTerrain = CURRENT_TERRAIN.WATER;
        }
        else if (other.gameObject.CompareTag("Sand"))
        {
            currentTerrain = CURRENT_TERRAIN.SAND;
        }
        else if (other.gameObject.CompareTag("Dirt"))
        {
            currentTerrain = CURRENT_TERRAIN.DIRT;
        }
        else if (other.gameObject.CompareTag("Wood"))
        {
            currentTerrain = CURRENT_TERRAIN.WOOD;
        }
    }

    public void SelectAndPlayFootstep()
    {
        switch (currentTerrain)
        {
            case CURRENT_TERRAIN.STONE:
                PlayFootstep(0);
                break;

            case CURRENT_TERRAIN.WATER:
                PlayFootstep(1);
                break;

            case CURRENT_TERRAIN.SAND:
                PlayFootstep(2);
                break;

            case CURRENT_TERRAIN.DIRT:
                PlayFootstep(3);
                break;

            case CURRENT_TERRAIN.WOOD:
                PlayFootstep(4);
                break;

            default:
                PlayFootstep(0);
                break;
        }
    }


    // finally make em sound runing

    private void PlayFootstep(int terrain)
    {
        foosteps = FMODUnity.RuntimeManager.CreateInstance("event:/FootstepsLancer");
        foosteps.setParameterByName("Terrain", terrain);
        foosteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        foosteps.start();
        foosteps.release();
    }

}