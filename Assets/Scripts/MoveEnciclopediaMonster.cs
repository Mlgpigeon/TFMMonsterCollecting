using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnciclopediaMonster : MonoBehaviour
{
    public float moveSpeed = 10f; // Adjust this speed as desired

    private bool isMoving = false;

    private void Update()
    {
        if (isMoving)
        {
            transform.Translate(transform.InverseTransformDirection(Vector3.forward) * moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnciclopediaWalls"))
        {
            isMoving = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnciclopediaWalls"))
        {
            isMoving = false;
        }
    }
}
