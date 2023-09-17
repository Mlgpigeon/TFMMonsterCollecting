using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSlotManager : MonoBehaviour
{
    private GameObject teambox;
    private GameObject boxGroup;
    public List<BoxSlot> teamBoxSlots;
    public List<BoxSlot> boxSlots;
    private GameObject boxItemPrefab;
    private HashSet<Transform> visitedTransforms = new HashSet<Transform>();

    private void Start()
    {
        teambox = this.GetComponent<VariableHolder>().teamBox;
        boxGroup = this.GetComponent<VariableHolder>().boxGroup;
        boxItemPrefab = this.GetComponent<VariableHolder>().itemPrefab;
        CollectBoxSlotsRecursively(teambox.transform, teamBoxSlots);
        CollectBoxSlotsRecursively(boxGroup.transform, boxSlots);
    }

    public bool AddMonster(Monster monster)
    {
        for (int i = 0; i < teamBoxSlots.Count; i++)
        {
            BoxSlot slot = teamBoxSlots[i];
            BoxItem monsterInSlot = slot.GetComponentInChildren<BoxItem>();
            if (monsterInSlot == null)
            {
                SpawnMonster(monster,slot);
                return true;
            }
        }
        
        for (int i = 0; i < boxSlots.Count; i++)
        {
            BoxSlot slot = boxSlots[i];
            BoxItem monsterInSlot = slot.GetComponentInChildren<BoxItem>();
            if (monsterInSlot == null)
            {
                SpawnMonster(monster,slot);
                return true;
            }
        }

        return false;
    }

    void SpawnMonster(Monster monster, BoxSlot slot)
    {
        GameObject newMonsterGO = Instantiate(boxItemPrefab, slot.transform);
        BoxItem boxItem = newMonsterGO.GetComponent<BoxItem>();
        boxItem.InitialiseItem(monster);
    }
    
    private void CollectBoxSlotsRecursively(Transform parent, List<BoxSlot> slots)
    {
        // Check if the transform has already been visited
        if (visitedTransforms.Contains(parent))
        {
            return;
        }

        visitedTransforms.Add(parent);

        BoxSlot[] childBoxSlots = parent.GetComponents<BoxSlot>();

        // Loop through the childBoxSlots array and add non-null components to the boxSlots list
        foreach (BoxSlot boxSlot in childBoxSlots)
        {
            if (boxSlot != null)
            {
                slots.Add(boxSlot);
            }
        }

        // Recursive call for each child
        for (int i = 0; i < parent.childCount; i++)
        {
            CollectBoxSlotsRecursively(parent.GetChild(i),slots);
        }
    }
}
