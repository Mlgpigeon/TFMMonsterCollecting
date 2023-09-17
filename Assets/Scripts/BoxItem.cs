using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoxItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")] public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    public Transform previous;
    
    public Monster monster;
    
    private void Start()
    {
        InitialiseItem(monster);
    }

    public void InitialiseItem(Monster newMonster)
    {
        monster = newMonster;
        this.GetComponent<UnityEngine.UI.Image>().color = new Color(1f,1f,1f,1f);
        if(!newMonster.shiny){
            this.GetComponent<Image>().sprite = newMonster.species.sprite;
        }
        else
        {
            this.GetComponent<Image>().sprite = newMonster.species.shinySprite;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        this.GetComponent<Image>().raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root.GetChild(0));
        previous = parentAfterDrag;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.GetComponent<Image>().raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        checkTeamBoxOrder(previous);
        checkTeamBoxOrder(parentAfterDrag);
    }
    public void checkTeamBoxOrder(Transform droppedPoke)
    {
        GameObject teambox = droppedPoke.parent.gameObject;
        if (teambox.name == "TeamBox")
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int i = 0; i < 6; i++)
            {
                if (teambox.transform.GetChild(i).childCount != 0)
                {
                    queue.Enqueue(teambox.transform.GetChild(i).GetChild(0).gameObject);
                }
            }
            
            int count = queue.Count;
            for (int i = 0; i < count; i++)
            {
                GameObject aux = queue.Dequeue();
                aux.transform.SetParent(teambox.transform.GetChild(i));
            }
        }
    }
}
