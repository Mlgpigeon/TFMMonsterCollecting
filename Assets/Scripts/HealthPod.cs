using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPod : MonoBehaviour
{
    public TeambarManager teambarManager;
    private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Player entered the collider, activate the desired behavior
                Activate();
            }
        }
    private void Activate()
    {
        teambarManager.despawnAll();
        teambarManager.healAll();
    }
}
