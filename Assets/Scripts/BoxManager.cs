using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    
    private GameObject boxGroup;

    void Start()
    {
        boxGroup = this.GetComponent<VariableHolder>().boxGroup;
    }

    public void swapBoxVisibility(bool state)
    {
        boxGroup.SetActive(state);
    }
    
}
