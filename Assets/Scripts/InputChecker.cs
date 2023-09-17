using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputChecker : MonoBehaviour
{
    //Character Input
    public Vector2 move;
    public Vector2 look;
    public bool roll;
    public bool jump;
    public bool fire;
    public bool sprint;
    public bool aim;
    public bool pause;
    public bool teambar;
    public bool box;
    public bool number1;
    public bool number2;
    public bool number3;
    public bool number4;
    public bool number5;
    public bool number6;
    public bool fighSelect;
    public bool inMenu;
    
    //MouseCursor
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    //Getting Input Values
    public void OnMove(InputValue value)
    {   
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        if(cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }
    public void OnRoll(InputValue value)
    {   
        RollInput(value.isPressed);
    }
    public void OnJump(InputValue value)
    {   
        JumpInput(value.isPressed);
    }
    
    public void OnPause(InputValue value)
    {
        PauseInput();
    }
    public void OnTeambar(InputValue value)
    {
        TeambarInput();
    }
    public void TeambarInput()
    {
        teambar = !teambar;
    }
    public void OnBox(InputValue value)
    {
        BoxInput();
    }
    public void BoxInput()
    {
        box = !box;
    }
    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }
    public void OnAim(InputValue value)
    {   
        aim = !aim;
    }
    public void OnFire(InputValue value)
    {   
        FireInput(value.isPressed);
    }
    public void OnFightSelect(InputValue value)
    {   
        FightSelectInput(value.isPressed);
    }
    public void FightSelectInput(bool newFightSelectState)
    {
        fighSelect = newFightSelectState;
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    //Assigning InputValues
    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    } 

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void RollInput(bool newRollState)
    {
        roll = newRollState;
    }

    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }
    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }
    public void PauseInput()
    {
        pause = !pause;
    }
    public void FireInput(bool newFireState)
    {
        fire = newFireState;
    }
    
    public void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
    
    public void OnPartner0(InputValue value)
    {
        Input0();
    }
    public void Input0()
    {
        number1 = !number1;
    }
    public void OnPartner1(InputValue value)
    {
        Input1();
    }
    public void Input1()
    {
        number2 = !number2;
    }
    public void OnPartner2(InputValue value)
    {
        Input2();
    }
    public void Input2()
    {
        number3 = !number3;
    }
    public void OnPartner3(InputValue value)
    {
        Input3();
    }
    public void Input3()
    {
        number4 = !number4;
    }
    public void OnPartner4(InputValue value)
    {
        Input4();
    }
    public void Input4()
    {
        number5 = !number5;
    }
    public void OnPartner5(InputValue value)
    {
        Input5();
    }
    public void Input5()
    {
        number6 = !number6;
    }
}

