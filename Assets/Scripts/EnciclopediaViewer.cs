using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnciclopediaViewer : MonoBehaviour
{
    public TextMeshProUGUI id;
    public TextMeshProUGUI monsterName;
    public GameObject model;
    public int enciclopediaLayer;
    public GameObject monsterModel;
    public float degrees = 220f; 
    

    private void Awake()
    {
        enciclopediaLayer = LayerMask.NameToLayer("EnciclopediaPanel");
    }

    public void loadSpecies(Species species)
    {
        
        id.text = "#" + species.speciesId;
        monsterName.text = species.speciesName;

        if (monsterModel != null)
        {
            Destroy(monsterModel); 
        }

        monsterModel = Instantiate(Resources.Load("Monsters/" + species.speciesId + "/" + species.speciesId) as GameObject);
        StartCoroutine(mover(monsterModel,species));

    }

    IEnumerator mover(GameObject monster, Species species)
    {
        MonsterController controller = monsterModel.GetComponent<MonsterController>();
        controller.monster = new Monster(species, false, 1f,1);
        assignLayerRecursively(monsterModel, enciclopediaLayer);
        controller.generateMonster(monsterMode.Enciclopedia);
        Destroy(monsterModel.GetComponent<NavMeshAgent>());
        
        yield return StartCoroutine(controller.bBoxgeneration());
        
        Vector3 viewerCenter = model.GetComponent<BoxCollider>().bounds.center;
        BoxCollider boxCollider = monsterModel.GetComponent<BoxCollider>();
        float offset = boxCollider.bounds.extents.y-(monsterModel.transform.position.y - boxCollider.bounds.min.y);
        float newY = viewerCenter.y - offset;
        monsterModel.transform.position = new Vector3(viewerCenter.x, newY, viewerCenter.z);
        Quaternion targetRotation = Quaternion.Euler(0f, degrees, 0f);
        monster.transform.rotation = targetRotation;
        monster.AddComponent<MoveEnciclopediaMonster>();

    }
    public void assignLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer; // Assign layer to the object
        
        // Assign layer to all child objects
        foreach (Transform child in obj.transform)
        {
            assignLayerRecursively(child.gameObject, layer);
        }
    }

}


