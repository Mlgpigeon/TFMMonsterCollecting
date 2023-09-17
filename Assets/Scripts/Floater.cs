using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public ThirdPersonPlayer thirdPersonPlayer;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the floater object enters the water trigger
        if (other.CompareTag("Water"))
        {
            thirdPersonPlayer.setSwimming(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Check if the floater object enters the water trigger
        if (other.CompareTag("Water"))
        {
            thirdPersonPlayer.setSwimming(false);
        }
    }
}
