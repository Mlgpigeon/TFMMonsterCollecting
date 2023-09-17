using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCapture : MonoBehaviour
{
    BoxSlotManager boxSlotManager;
    private TeambarManager teambarManager;
    void Start()
    {
        boxSlotManager = FindObjectOfType<BoxSlotManager>();
        teambarManager = FindObjectOfType<TeambarManager>();
    }
    public bool initiateCapture(GameObject monsterCaptured)
    {
        bool success = false;

        Monster captured =monsterCaptured.GetComponent<MonsterController>().monster;
        success = boxSlotManager.AddMonster(captured);
        if (success)
        {
            teambarManager.UpdateTeambar();
        }
        return success;
    }
}
