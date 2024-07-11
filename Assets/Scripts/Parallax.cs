using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cam;
    public float relativeMovement = .3f;
    public bool lockY = false;


    // Update is called once per frame
    void Update()
    {
       
        if (lockY)
        {
            transform.position = new Vector2(cam.position.x * relativeMovement, transform.position.y);

        }
        else
        {
            transform.position = new Vector2(cam.position.x * relativeMovement, cam.position.y * relativeMovement);
        }
    }
}

