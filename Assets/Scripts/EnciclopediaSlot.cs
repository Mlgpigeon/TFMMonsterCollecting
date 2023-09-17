using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnciclopediaSlot : MonoBehaviour
{
    public Species _species;
    public GameObject viewer;
    public GameObject grid;

    
    public void loadImage()
    {
        this.GetComponent<Image>().sprite = _species.sprite;
    }

    
    public void loadDetails()
    {   viewer.SetActive(true);
        viewer.GetComponent<EnciclopediaViewer>().loadSpecies(_species);
        grid.SetActive(false);
    }
}
