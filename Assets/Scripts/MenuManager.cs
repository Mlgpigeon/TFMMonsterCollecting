using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private GameObject pauseMenu;
    private InputChecker _input;
    
    private TeambarManager _teambar;
    private bool lastTeambarState;
    
    private BoxManager _boxes;
    private bool lastBoxState;
    private bool[] partnerState = new bool[6];

    public bool inPause = false;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu = this.GetComponent<VariableHolder>().pauseMenu;
        _input = this.gameObject.GetComponent<InputChecker>();
        _teambar = this.gameObject.GetComponent<TeambarManager>();
        _boxes = this.gameObject.GetComponent<BoxManager>();
        for (int i = 0; i < 6; i++)
        {
            partnerState[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Not in box menu nor in pause and pause is pressed
        if (!lastBoxState && !inPause && _input.pause)
        {
            _input.inMenu = true;
            pauseMenu.SetActive(true);
            inPause = true;
            lockCursor(false);
            updateTeambar(false);
            savePartnerState();
        }
        //In pause and the menu is exited
        if (inPause && !_input.pause)
        {
            _input.inMenu = false;
            pauseMenu.SetActive(false);
            _input.box = false;
            _input.teambar = false;
            inPause = false;
            loadPartnerState();
            lockCursor(true);
        }
        
        if (!inPause)
        {
            //Box is activated if it wasnt and viceversa
            if (lastBoxState != _input.box)
            {
                updateBox(_input.box);
                if (_input.box)
                {
                    _input.inMenu = true;
                    _teambar.despawnAll();
                    lastTeambarState = false;
                }
                else
                {
                    _input.inMenu = false;
                    _teambar.resetAllInput();
                }
                
            }
            
            if(!lastBoxState && lastTeambarState != _input.teambar)
            {
                updateTeambar(_input.teambar);
                
            }
                        
            if (lastTeambarState)
            {
                checkPartner();
            }
        }
    }
    public void updateTeambar(bool state)
    {
        _teambar.swapBarVisibility(state);
        lastTeambarState = _input.teambar;
        if (!lastBoxState)
        {
            savePartnerState();
        }
    }
    public void updateBox(bool state)
    {
        if (state)
        {
            updateTeambar(false);
            lockCursor(false);
        }
        else
        {
            _input.pause = false;
            _input.teambar = false;
            lockCursor(true);
        }
        
        _boxes.swapBoxVisibility(state);
        lastBoxState = _input.box;
    }

    public void lockCursor(bool state)
    {
        _input.SetCursorState(state);
    }

    public void savePartnerState()
    {
        partnerState[0] = _input.number1;
        partnerState[1] = _input.number2;
        partnerState[2] = _input.number3;
        partnerState[3] = _input.number4;
        partnerState[4] = _input.number5;
        partnerState[5] = _input.number6;
    }

    public void loadPartnerState()
    {
        _input.number1 = partnerState[0];
        _input.number2 = partnerState[1];
        _input.number3 = partnerState[2];
        _input.number4 = partnerState[3];
        _input.number5 = partnerState[4];
        _input.number6 = partnerState[5];
    }

    public void checkPartner()
    {
        bool changed = false;
        
        if (partnerState[0] != _input.number1)
        {
            print("_input.number1"+_input.number1);
            _teambar.selectPartner(0,_input.number1);
            changed = true;
        }
        if (partnerState[1] != _input.number2)
        {
            _teambar.selectPartner(1,_input.number2);
            changed = true;
        }
        if (partnerState[2] != _input.number3)
        {
            _teambar.selectPartner(2,_input.number3);
            changed = true;
        }
        if (partnerState[3] != _input.number4)
        {
            _teambar.selectPartner(3,_input.number4);
            changed = true;
        }
        if (partnerState[4] != _input.number5)
        {
            _teambar.selectPartner(4,_input.number5);
            changed = true;
        }
        if (partnerState[5] != _input.number6)
        {
            _teambar.selectPartner(5,_input.number6);
            changed = true;
        }

        if (changed)
        {
            savePartnerState();
        }
    }
}
