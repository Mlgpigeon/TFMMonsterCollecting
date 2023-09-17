using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCollider : MonoBehaviour
{
    public string targetTag = "SpawnRadius"; // Tag to check against
    public MonsterController monsterController;
    public TeambarManager teambarManager;
    private void Start()
    {
        monsterController = GetComponent<MonsterController>();
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log("Object with tag '" + targetTag + "' exited the collider: " + other.gameObject.name);
            // Access the GameObject that triggered the exit event
            if (monsterController.monsterMode == monsterMode.Partner)
            {
                monsterController.despawn();
            }
            else
            {
                Destroy(this.gameObject);
            }
            
        }
    }
}
