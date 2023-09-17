using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallProjectile : MonoBehaviour
{   
    public Rigidbody rb;
    public float destructionDelay = 5f;
    //Script that describes the physics of a ball thrown by the player
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
    }
    public void Throw(float distance)
    {
        rb.velocity = transform.forward * distance * 2.5f;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Monster"))
        {
            rb.velocity = rb.velocity * 0.3f;
            Invoke("destroyObject", destructionDelay);
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            if (GetComponent<MonsterCapture>().initiateCapture(collision.gameObject))
            {
                Destroy(collision.gameObject);
            }
            destroyObject();
            
        }
    }
    private void destroyObject()
    {
        Destroy(gameObject);
    }
}
