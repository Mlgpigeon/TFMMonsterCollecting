using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class BoxSlot : MonoBehaviour, IDropHandler
{
    private bool lastPoke;
    private int lastPokeCounter;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        BoxItem boxItem = dropped.GetComponent<BoxItem>();
        Transform previous = boxItem.parentAfterDrag;
        if (transform.childCount == 0)
        {
            
            checkLast(previous);
            if(!lastPoke){
                boxItem.parentAfterDrag = transform;
            }

        }
        else
        {
            transform.GetChild(0).SetParent(previous);
            boxItem.parentAfterDrag = transform;
        }
    }

    public Monster getMonster()
    {
        return transform.GetChild(0).GetComponent<BoxItem>().monster;
    }

    public bool checkLast(Transform droppedPoke)
    {
        GameObject teambox = droppedPoke.parent.gameObject;
        
        if (teambox.name == "TeamBox")
        {
            lastPokeCounter = 1;
            lastPoke = false;
            
            for (int i = 0; i < 6; i++)
            {
                if (teambox.transform.GetChild(i).childCount != 0)
                {
                    lastPokeCounter++;
                }
            }
            if (lastPokeCounter==1)
            {
                lastPoke = true;
            }
        }

        return lastPoke;
    }

    
}
