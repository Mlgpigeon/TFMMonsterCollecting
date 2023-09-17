using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class RoamingAI : MonoBehaviour
{
    public float roamingRadius = 100f; // The radius within which the agent can roam
    public float timer = 0f;
    public float roamingInterval = 30f; // The time interval between roaming to a new destination
    
    private FollowController followController;
    private Vector3 originalPosition;
    private NavMeshSurface navMeshSurface;
    private GameObject destinationPoint;

    void Start()
    {
        followController = GetComponent<FollowController>();
        timer = 30f;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= roamingInterval)
        {
            if (destinationPoint == null)
            {
                destinationPoint = new GameObject("destinationPoint");
                destinationPoint.transform.position = transform.position;
                originalPosition = destinationPoint.transform.position;
            }
            if (followController == null)
            {
                followController = GetComponent<FollowController>();
            }
            Roam();
            timer = 0f;
        }
    }

    void Roam()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamingRadius;
        randomDirection += originalPosition;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, roamingRadius, NavMesh.AllAreas))
        {
            destinationPoint.transform.position = hit.position;
            followController.target = destinationPoint.transform;
        }
    }

    private void OnDestroy()
    {
        Destroy(destinationPoint);
    }
}
