using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FightSystem : MonoBehaviour
{
    // Start is called before the first frame update
    private NavMeshAgent nav;
    private RoamingAI roamingAi;
    private FollowController followController;
    private GameObject currentOpponent;
    
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        roamingAi = GetComponent<RoamingAI>();
        followController = GetComponent<FollowController>();
    }

    public void goFight(GameObject opponent)
    {
        currentOpponent = opponent;
        FollowController opponentFollowController = opponent.GetComponent<FollowController>();
        RoamingAI opponentRoamingAI = GetComponent<RoamingAI>();
        // Disable other movement scripts
        opponentRoamingAI.enabled = false;

        // Set this GameObject as the target for the opponent
        opponentFollowController.changeDestination(transform);

        // Set the opponent's GameObject as the target for this agent
        followController.changeDestination(opponent.transform);
        
        StartCoroutine(CheckArrival(opponentFollowController, followController));
    }
    private IEnumerator CheckArrival(FollowController opponentController, FollowController thisController)
    {
        float totalRadius = 25f;
        bool opponentArrived = false;
        bool thisArrived = false;

        while (true)
        {
            float opponentDistance = Vector3.Distance(opponentController.transform.position, transform.position);
            float thisDistance = Vector3.Distance(transform.position, currentOpponent.transform.position);

            if (!opponentArrived && opponentDistance < totalRadius)
            {
                opponentArrived = true;
            }

            if (!thisArrived && thisDistance < totalRadius)
            {
                thisArrived = true;
            }

            if (opponentArrived && thisArrived)
            {
                opponentController.nav.enabled = false;
                opponentController.enabled = false;
                thisController.nav.enabled = false;
                thisController.enabled = false;
                StartFight();
                break;
            }

            yield return null;
        }
    }


    private void StartFight()
    {
        // Perform actions to start the fight between the agents
        print("FIGHT");
        // Calculate the direction from this agent to the opponent
        Vector3 direction = currentOpponent.transform.position - transform.position;
        direction.y = 0f; // Ignore vertical component

        // Rotate this agent to face the opponent
        transform.rotation = Quaternion.LookRotation(direction);

        // Rotate the opponent to face this agent
        currentOpponent.transform.rotation = Quaternion.LookRotation(-direction);
        
        DoFight();
    }
    
   private void DoFight()
{
    MonsterController opponentController  = currentOpponent.GetComponent<MonsterController>();
    MonsterController currentController = GetComponent<MonsterController>();
    Monster opponentMonster = opponentController.monster;
    Monster thisMonster = currentController.monster;

    string thisname = checkName(thisMonster);
    string opponnentname =checkName(opponentMonster);
    int firstFainted = 0;
    while (!opponentMonster.fainted() && !thisMonster.fainted())
    {
        // Calculate attack damage based on levels
        int opponentDamage = CalculateDamage(opponentMonster.currentLevel);
        int thisDamage = CalculateDamage(thisMonster.currentLevel);
        // Determine if the attacks hit based on an 80% chance
        bool opponentHit = Random.value < 0.8f;
        bool thisHit = Random.value < 0.8f;
        
        // Apply damage to the monsters
        if (opponentHit)
        {
            Debug.Log(thisname + " attacked " + opponnentname + " for " + thisDamage + " damage.");
            if (firstFainted == 0)
            {
                bool opponentFainted = opponentMonster.receiveDamage(thisDamage);
                if (opponentFainted)
                {
                    firstFainted = 1;
                    // Opponent monster fainted
                    Debug.Log(opponnentname + " fainted!");
                }
            }
            
        }
        else
        {
            Debug.Log(thisname + " missed the attack.");
        }
        if (thisHit)
        {
            if (firstFainted == 0)
            {
                Debug.Log(opponnentname + " attacked " + thisname + " for " +
                          opponentDamage + " damage.");
                print("MYHEALTH"+thisMonster.currentHealth);
                bool thisFainted = thisMonster.receiveDamage(opponentDamage);
                if (thisFainted)
                {
                    firstFainted = 2;
                    // This monster fainted
                    Debug.Log(thisname + " fainted!");
                }

            }
        }
        else
        {
            Debug.Log(opponnentname + " missed the attack.");
        }
        
        currentController.teambarManager.UpdateTeambar();
    }

    print("Combat finished" + thisMonster.fainted() + "opo " + opponentMonster.fainted() + "number" + firstFainted);
    if (firstFainted == 1)
    {
        int level = opponentMonster.currentLevel * 200;
        bool evolved = thisMonster.gainExp(level);
        if (evolved)
        {
            currentController.evolveMonster();
        }
        currentController.teambarManager.UpdateTeambar();
        opponentController.despawn();
        currentController.restoreNavigation();
    }
    else if (firstFainted == 2)
    {
        currentController.despawn();
        opponentController.restoreNavigation();
    }
}

   private string checkName(Monster monster)
   {
       if (!string.IsNullOrEmpty(monster.customName))
       {
           return monster.customName;
       }
       
       return monster.species.speciesName;
   }
    private int CalculateDamage(int level)
    {
        // Calculate the damage based on the monster's level
        // Adjust the formula or use additional factors as needed
        return level;
    }
    
}
