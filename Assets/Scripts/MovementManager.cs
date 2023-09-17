using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    private GameObject gameManager;
    private MenuManager menuManager;
    private ThirdPersonPlayer groundMove;
    private AimController groundAim;
    private void Start()
    {
        gameManager = this.GetComponent<CharacterVariables>().gameManager;
        this.menuManager = gameManager.GetComponent<MenuManager>();
        this.groundMove = this.gameObject.GetComponent<ThirdPersonPlayer>();
        this.groundAim = this.gameObject.GetComponent<AimController>();
    }
    
    void Update()
    {
        if (menuManager.inPause)
        {
            groundMovement(false);
        }
        else
        {
            groundMovement(true);
        }
    }

    private void groundMovement(bool state)
    {
        groundMove.enabled = state;
        groundAim.enabled = state;
    }
}
