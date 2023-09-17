using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine.Utility;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public Species species;
    [Range(0, 1)]
    public float probability;
    [Range(0, 1)]
    public float shinyProbability;
    public float interval = 0f;
    public string targetTag = "SpawnRadius";
    public float checkRadius = 1f;
    public Type type;
    public float waterLevel;
    public float neededHeight;
    public float neededDepth;
    
    private int typeLayer;
    private GameObject prefab; // Prefab of the sphere to be spawned 
    private float timer = 0f;
    private float radius = 0f;
    private SphereCollider spawnCollider;
    private float lastRadius = 0f;
    private Terrain terrain;
    Vector3 position = new Vector3(0, 0, 0);

    private SpawnManager spawnManager;
    
    [Range(1, 100)] 
    public float minSize;
    [Range(1, 100)] 
    public float maxSize;

    [Range(1, 100)] // Set the range from 1 to 99
    public int minLevel = 1;
    
    [Range(1, 100)] // Set the range from 2 to 100
    public int maxLevel = 100;
    public enum Type
        {
            Water,
            Terrain
        }
    private void OnValidate()
    {
        // Clamp the minLevel and maxLevel values to ensure they don't overlap
        if (minLevel >= maxLevel)
        {
            minLevel = maxLevel;
        }
        if (maxLevel <= minLevel)
        {
            maxLevel = minLevel;
        }
        
        if (minSize >= maxSize)
        {
            minSize = maxSize;
        }
        if (maxSize <= minSize)
        {
            maxSize = minSize;
        }
    }
    

    

    private void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        spawnCollider = GetComponent<SphereCollider>();
        prefab = Resources.Load("Monsters/" + species.speciesId + "/" + species.speciesId) as GameObject;
    }

    private void Update()
    {
        if (type == Type.Terrain)
        {
            typeLayer= NavMesh.GetAreaFromName("Walkable");
        }

        if (type == Type.Water)
        {
            typeLayer= NavMesh.GetAreaFromName("Swim");
        }
        timer += Time.deltaTime;
        if (minSize < maxSize && minSize > 0 && minLevel < maxLevel && minLevel > 0 && maxLevel <= 100)
        {
            if (timer >= interval && !spawnManager.mobLimit())
            {
   
                updateRadius();
                float value = Random.value;
                if (value <= probability) // Check the probability
                {
                    Spawn();
                }
            
                timer = 0f;
            }
        }
        
    }

    public void updateRadius()
    {
        if (lastRadius != spawnCollider.radius)
        {
            lastRadius = spawnCollider.radius;
            // Get the radius of the sphere collider in world coordinates
            radius = spawnCollider.radius * Mathf.Max(transform.localScale.x,
                Mathf.Max(transform.localScale.y, transform.localScale.z));
        }
        
    }

    public void Spawn()
    {
        // Calculate a random point within the spawn radius
        Vector3 randomPoint = Random.insideUnitSphere* radius;
        randomPoint = transform.TransformPoint(randomPoint);
        if (Spawnable(randomPoint))
        {
            if (terrainCheck(randomPoint))
            {
                // Instantiate the sphere prefab at the random point
                generateMonster();
            }
        }
    }

    public void generateMonster()
    {
       
        if(checkStatus()){
            
            float randomScale = Random.Range(minSize, maxSize);
            int randomLevelValue = (int)Random.Range(minLevel, maxLevel + 1);
            float shinyValue = Random.value;
            bool shiny = false;
            if (shinyValue <= shinyProbability)
            {
                print("Shiny!");
                shiny = true;
            }
            Monster monster = new Monster(species,shiny,randomScale,randomLevelValue);
            GameObject spawnedObject = monster.spawn();
            spawnedObject.transform.position = position;
            Physics.SyncTransforms();
            spawnedObject.GetComponent<MonsterController>().generateMonster(monsterMode.Wild);
            if (spawnedObject.GetComponent<NavMeshAgent>().isOnNavMesh)
            {
                addToMobCap(spawnedObject);
                StartCoroutine(spawnedObject.GetComponent<MonsterController>().bBoxgeneration());
            }
            else
            {
                Destroy(spawnedObject);
            }
            
        }
    }

    public bool checkStatus()
    {
        if (type == Type.Water)
        { 
            if (position.y != waterLevel)
            {
                return false;
            }
            
            float raycastDistance = 99999f; // Adjust the distance as needed
            int layerToIgnore = LayerMask.NameToLayer("Water"); // Specify the layer you want to ignore

            // Create a layer mask that excludes the specified layer
            int layerMask = ~(1 << layerToIgnore);
            RaycastHit hit;
            if (Physics.Raycast(position, Vector3.down, out hit, raycastDistance, layerMask))
            {
                if (!hit.collider.CompareTag("Terrain"))
                {
                  return false;  
                }

                if (hit.distance < neededDepth)
                {
                    return false;
                }
                
            }
            else
            {
                return false;
            }
        }
        if (type == Type.Terrain)
        {
            if (position.y < waterLevel + neededHeight)
            {
                return false;
            }
        }

        return true;
    }
    public bool Spawnable(Vector3 randomPoint)
    {
        // Get all colliders within the check radius around the object
        Collider[] colliders = Physics.OverlapSphere(randomPoint, checkRadius);


        // Check if any of the colliders have the target tag
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(targetTag))
            {
                return true; // Exit the loop if inside any collider
            }
        }

        return false;
    }
    public bool terrainCheck(Vector3 randomPoint)
    {
        bool check = false;
        RaycastHit hit;
        if (Physics.Raycast(randomPoint, Vector3.down, out hit))
        {
            if (!hit.collider.CompareTag("Monster") && (hit.collider.CompareTag("Terrain") || hit.collider.CompareTag("Water")))
            {
                if (hit.collider.CompareTag("Terrain"))
                {
                    terrain = hit.collider.GetComponent<Terrain>();
                }
                if (hit.collider.CompareTag("Water"))
                {
                    waterLevel = hit.point.y;
                }
                position = hit.point;
                check = true;
            }
        }
        return check;
    }
    
    public void addToMobCap(GameObject spawnedObject)
    {
        spawnedObject.transform.SetParent(spawnManager.mobCap.transform);
    }
}


  
        
