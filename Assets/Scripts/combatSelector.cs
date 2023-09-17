using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combatSelector : MonoBehaviour
{
    private InputChecker inputChecker;

    private GameObject partner;
    private GameObject monster;
    private float refreshTimer = 1f; // Time between input refreshes
    private float currentTimer = 0f;
    private void Start()
    {
        inputChecker = FindObjectOfType<InputChecker>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Partner"))
        {
            if (other.gameObject != partner)
            {
                ChangeLayerRecursive(other.transform, 11);
            }
        }
        if (other.CompareTag("Monster"))
        {
            if (other.gameObject != monster)
            {
                ChangeLayerRecursive(other.transform, 11);
            }
        }
    }

    private void Update()
    {
        currentTimer -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Partner"))
        {
            if (inputChecker.fighSelect && currentTimer <= 0f)
            {
                if (other.gameObject.layer == 12)
                {
                    ChangeLayerRecursive(other.transform, 11);
                    partner = null;
                }
                else
                {
                    ChangeLayerRecursive(other.transform, 12);
                    if (partner != other.gameObject && partner != null)
                    {
                        ChangeLayerRecursive(partner.transform, 10);
                    }
                    partner = other.gameObject;
                    checKFight();
                }
                currentTimer = refreshTimer;
            }
        }
        if (other.CompareTag("Monster"))
        {
            if (inputChecker.fighSelect)
            {
                if (other.gameObject.layer == 12 && currentTimer <= 0f)
                {
                    ChangeLayerRecursive(other.transform, 11);
                    monster = null;
                }
                else
                {
                    ChangeLayerRecursive(other.transform, 12);
                    if (monster != other.gameObject && monster != null)
                    {
                        ChangeLayerRecursive(monster.transform, 10);
                    }
                    monster = other.gameObject;
                    checKFight();
                }
                currentTimer = refreshTimer;
                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Partner"))
        {
            if (other.gameObject != partner)
            {
                ChangeLayerRecursive(other.transform, 10);
            }
            
        }
        if (other.CompareTag("Monster"))
        {
            if (other.gameObject != monster)
            {
                ChangeLayerRecursive(other.transform, 10);
            }
        }
    }
    void ChangeLayerRecursive(Transform parent, int newLayer)
    {
        // Change the layer of the parent object
        parent.gameObject.layer = newLayer;

        // Change the layer of all child objects
        foreach (Transform child in parent)
        {
            if (child.name != "levelDisplay" && child.name != "LevelDisplay")
            {
                ChangeLayerRecursive(child, newLayer);
            }
            else
            {
                if (newLayer == 11 || newLayer == 12)
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
            
        }
    }
    public void checKFight()
    {
        if (partner != null && monster != null)
        {
            ChangeLayerRecursive(partner.transform, 10);
            ChangeLayerRecursive(monster.transform, 10);
            partner.GetComponent<FightSystem>().goFight(monster);
            partner = null;
            monster = null;
        }
    }
}
