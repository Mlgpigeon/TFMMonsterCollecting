using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TeambarManager : MonoBehaviour
{
    private GameObject teambar;
    private GameObject teambox;
    private GameObject partnerGroup;
    private List<BoxSlot> BoxSlots;
    private List<GameObject> barSlots= new List<GameObject>();
    private List<Image> Sprites = new List<Image>();
    private List<GameObject> Healthbars = new List<GameObject>();
    private List<GameObject> Expbars= new List<GameObject>();
    
    private InputChecker _input;
    
    private List<bool> isOut = new List<bool>();
    private Transform _target;
   
    
    void Start()
    {
        teambar = this.GetComponent<VariableHolder>().teamBar;
        teambox = this.GetComponent<VariableHolder>().teamBox;
        partnerGroup = this.GetComponent<VariableHolder>().partnerGroup;
        _target = this.GetComponent<VariableHolder>().player.transform;
        _input = this.GetComponent<InputChecker>();
        BoxSlots = teambox.GetComponentsInChildren<BoxSlot>().ToList();
        for (int i = 0; i < teambar.transform.childCount; i++)
        {
            Transform UISlot = teambar.transform.GetChild(i);
            Sprites.Add(UISlot.GetChild(0).GetComponent<Image>());
            Healthbars.Add(UISlot.GetChild(1).gameObject);
            Expbars.Add(UISlot.GetChild(2).gameObject);
        }

        for (int i = 0; i < partnerGroup.transform.childCount; i++)
        {
            GameObject Partner = teambar.transform.GetChild(i).gameObject;
            barSlots.Add(Partner);
            isOut.Add(false);
        }
    }
    public void swapBarVisibility(bool state)
    {
        teambar.SetActive(state);
        UpdateTeambar();
    }

    public void UpdateTeambar()
    {
        for (int i = 0; i < 6; i++)
        {
            if (BoxSlots[i].transform.childCount > 0)
            {
                activateVisualElements(i,true);
            }
            else
            {
                activateVisualElements(i,false);
            }
        }
    }

    public void activateVisualElements(int partner,bool active)
    {
        Sprites[partner].gameObject.SetActive(active);
        Healthbars[partner].gameObject.SetActive(active);
        Expbars[partner].gameObject.SetActive(active);
        if (active)
        {
            Monster monster = BoxSlots[partner].getMonster();
            
            if (!monster.shiny)
            {
                Sprites[partner].sprite = monster.species.sprite;
            }
            else
            {
                Sprites[partner].sprite = monster.species.shinySprite;
            }
            Sprites[partner].color = checkColor(partner, monster);
            
            Healthbars[partner].transform.GetChild(0).gameObject.GetComponent<Healthbar>().UpdateHealth(monster);
            Expbars[partner].transform.GetChild(0).gameObject.GetComponent<ExperienceBar>().UpdateExp(monster);
            Expbars[partner].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = monster.currentLevel.ToString();
        }
    }

    public Color checkColor(int partner, Monster monster)
    {
        Color defeated = new Color(0.5f, 0.0f, 0.0f, 1f);
        Color inactive = new Color(0.3f, 0.3f, 0.3f, 1f);
        Color active = new Color(1f, 1f, 1f, 1f);
        if (monster.fainted())
        {
            return defeated;
        }
        if (isOut[partner])
        {
            return inactive;
        }
        return active;
    }

    public void selectPartner(int partner, bool state)
    {
        if (BoxSlots[partner].transform.childCount > 0)
        {
            Monster monster = BoxSlots[partner].getMonster();
            
            if (state)
            {
                if (!isOut[partner] && !monster.fainted())
                {
                        spawnPartner(partner);  
                }
            }
            else
            {
                if (isOut[partner])
                {
                        despawnPartner(partner);
                }
                            
            }
            UpdateTeambar();
        }
    }
    
    public void despawnPartner(int partner)
    {
        Destroy(barSlots[partner].transform.GetChild(0).GetChild(0).gameObject);
        isOut[partner] = false;
        resetInput(partner);
        UpdateTeambar();
    }

    public void despawnAll()
    {
        for (int i = 0; i <6; i++)
        {
            if (isOut[i])
            {
                despawnPartner(i);
            }
        }
    }

    public void healPartner(int partner)
    {
        BoxSlots[partner].getMonster().restoreHealth();
        UpdateTeambar();
    }

    public void healAll()
    {
        for (int i = 0; i < 6; i++)
        {
            if (BoxSlots[i].transform.childCount > 0)
            {
               healPartner(i); 
            }
            
        }
    }

    public void resetInput(int index)
    {
        switch (index)
        {
            case 0:
                _input.number1 = false;
                break;
            case 1:
                _input.number2 = false;
                break;
            case 2:
                _input.number3 = false;
                break;
            case 3:
                _input.number4 = false;
                break;
            case 4:
                _input.number5 = false;
                break;
            case 5:
                _input.number6 = false;
                break;
        }
    }

    public void resetAllInput()
    {
        for (int i = 0; i < barSlots.Count; i++)
        {
            resetInput(i);
        }
    }
    
    public void spawnPartner(int partner)
    {
        Monster monster = BoxSlots[partner].transform.GetChild(0).gameObject.GetComponent<BoxItem>().monster;

        print("Spawning " + monster.species.speciesId);
        GameObject newPartner = monster.spawn();
        newPartner.transform.position = randomPointBehindPlayer();
        Physics.SyncTransforms();
        MonsterController controller = newPartner.GetComponent<MonsterController>();
        controller.generateMonster(monsterMode.Partner);
        controller.teambarIndex = partner;
        StartCoroutine(newPartner.GetComponent<MonsterController>().bBoxgeneration());
        newPartner.transform.SetParent(barSlots[partner].transform.GetChild(0));
        isOut[partner] = true;
    }
    
    private Vector3 randomPointBehindPlayer()
    {
        // Set the distance behind the player you want the random point to be generated
        float distanceBehind = 15f;

        Vector3 position = _target.position;
        Vector3 forward = _target.forward;

        // Calculate the random point behind the player
        Vector3 randomPoint = position - forward * distanceBehind;

        NavMeshHit hit;
        // Find the closest point on the NavMesh to the random point
        if (NavMesh.SamplePosition(randomPoint, out hit, distanceBehind, NavMesh.AllAreas))
        {
            return hit.position;
        }

        // If no valid point is found, return the player's position
        return position;
    }
}

