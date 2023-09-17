using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnciclopediaGrid : MonoBehaviour
{
    public GameObject parent;
    public GameObject viewer;
    private SaveManager loader;

    private void Awake()
    {
        loader = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        if (parent.transform.childCount > 0)
        {
            for (int j =0; j< parent.transform.childCount; j++)
            {
               Destroy(parent.transform.GetChild(j).gameObject); 
            }
        }
        int i = 0;
        foreach (var species in loader.species)
        {
            GameObject newGrid = Instantiate(Resources.Load("Prefabs/EnciclopediaSlot") as GameObject);
            
            var slot = newGrid.GetComponent<EnciclopediaSlot>();
            slot._species = species;
            slot.loadImage();
            slot.grid = this.gameObject;
            slot.viewer = this.viewer;
            newGrid.transform.SetParent(parent.transform);
            i++;
        }
    }
}
