using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FootstepsSoilDemon : MonoBehaviour
{
    private FMOD.Studio.EventInstance foosteps;

  

    private void PlayFootstep()
    {
        foosteps = FMODUnity.RuntimeManager.CreateInstance("event:/FootstepsSoilDemon");
        foosteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        foosteps.start();
        foosteps.release();
    }
}
