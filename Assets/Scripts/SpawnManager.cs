using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject mobCap;
    public int maxMobs = 30;
    
    public void addToMobCap(GameObject spawned)
    {
        spawned.transform.SetParent(mobCap.transform);
    }

    public bool mobLimit()
    {
        return ( mobCap.transform.childCount >= maxMobs);
    }
    
}
