using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void LateUpdate()
    {
        // Get the direction from the level display to the camera
        Vector3 cameraDirection = Camera.main.transform.position - transform.position;

        // Rotate the level display to face the camera
        transform.rotation = Quaternion.LookRotation(cameraDirection, Vector3.up) * Quaternion.Euler(0, 180, 0);
    }
}
