using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class BoundingBox : MonoBehaviour
{
    private GameObject levelDisplay;
    private BoxCollider boxCollider;

    private void Start()
    {
        levelDisplay = gameObject.transform.Find("LevelDisplay")?.gameObject;
        // Check if the LevelDisplay prefab is assigned
        if (levelDisplay == null)
        {
            // Fetch the LevelDisplay prefab from Resources/Prefabs
            GameObject levelDisplayPrefab = Resources.Load<GameObject>("Prefabs/LevelDisplay");
            
            // Instantiate the LevelDisplay prefab as a child of this object
            if (levelDisplayPrefab != null)
            {
                levelDisplay = Instantiate(levelDisplayPrefab, transform);
                levelDisplay.SetActive(true);
                levelDisplay.name = "LevelDisplay";
            }
            else
            {
                Debug.LogWarning("LevelDisplay prefab not found in Resources/Prefabs.");
            }
        }
        
        boxCollider = GetComponent<BoxCollider>();
    }

    public void generateBoundingBox()
    {
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();

        if (childRenderers.Length > 0)
        {
            Bounds combinedBounds = CalculateCombinedBounds(childRenderers);
            
            if (boxCollider == null)
            {
                boxCollider = gameObject.AddComponent<BoxCollider>();
            }

            Vector3 combinedCenter = combinedBounds.center;
            MonsterController controller = GetComponent<MonsterController>();
            Vector3 combinedSize = combinedBounds.size / controller.monster.size;

            // Apply the scale and rotation of the parent object
            combinedCenter = transform.InverseTransformPoint(combinedCenter);
            boxCollider.center = combinedCenter;
            boxCollider.size = combinedSize;
        }
        else
        {
            Debug.LogWarning("No child objects with Renderer found.");
        }

        if (levelDisplay != null)
        {
            levelDisplay.SetActive(true);
            Vector3 offsetText = new Vector3(boxCollider.bounds.center.x,boxCollider.bounds.center.y + boxCollider.bounds.extents.y*2,boxCollider.bounds.center.z);
            levelDisplay.transform.position = offsetText;
            Monster monster = transform.GetComponent<MonsterController>().monster;
            if (!string.IsNullOrEmpty(monster.name))
            {
               levelDisplay.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                               monster.customName; 
            }
            else
            {
                levelDisplay.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text =
                    monster.species.speciesName; 
            }
            levelDisplay.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text =
                monster.currentLevel.ToString(); 
            levelDisplay.SetActive(false);
        }
    }



    private Bounds CalculateCombinedBounds(Renderer[] renderers)
    {
        Bounds combinedBounds = new Bounds();

        foreach (Renderer renderer in renderers)
        {
            if (renderer.bounds.size != Vector3.zero)
            {
                if (combinedBounds.size == Vector3.zero)
                {
                    combinedBounds = renderer.bounds;
                }
                else
                {
                    combinedBounds.Encapsulate(renderer.bounds);
                }
            }
        }
        return combinedBounds;
    }
    
}  
        

