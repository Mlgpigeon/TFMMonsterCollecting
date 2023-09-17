using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    private BoundingBox boundingBox;
    private MonsterAnimator monsterAnimator;
    private FollowController followController;
    private NavMeshAgent navMeshAgent;
    public GameObject player;
    public Monster monster;
    public monsterMode monsterMode;
    public int teambarIndex;
    public TeambarManager teambarManager;
    private bool coroutineFinished = false;

    private void Start()
    {
        teambarManager = GameObject.Find("GameManager").GetComponent<TeambarManager>();
        player = GameObject.Find("Player");
    }

    public void generateMonster(monsterMode mode)
    {
        
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        boundingBox = gameObject.GetComponent<BoundingBox>();
        followController = gameObject.GetComponent<FollowController>();
        monsterAnimator = GetComponent<MonsterAnimator>();
        monsterAnimator.generateAnimator(monster.species.speciesId);
        transform.localScale = new Vector3(monster.size, monster.size, monster.size);
        monsterMode = mode;
        
        switch (mode)
        {
            case monsterMode.Wild:
                
                tag = "Monster";
                navMeshAgent.enabled = true;
                this.GetComponent<RoamingAI>().enabled = true;
                followController.enabled = true;
                break;
            case monsterMode.Partner:
                tag = "Partner";
                if (player == null)
                {
                    player = GameObject.Find("Player");
                }
                navMeshAgent.enabled = true;
                followController.enabled = true;
                followController.target = player.transform;
                navMeshAgent.speed = 15;
                break;
            case monsterMode.Enciclopedia:
                Destroy(GetComponent<ExitCollider>());
                break;
        }
    }
    
    public IEnumerator bBoxgeneration()
    {
        yield return new WaitForEndOfFrame();// Wait until the end of the frame to ensure that scaling is applied
        boundingBox.generateBoundingBox();
    }

    public void despawn()
    {
        switch (monsterMode)
        {
            case monsterMode.Wild:
                Destroy(this.gameObject);
                break;
            case monsterMode.Partner:
                teambarManager.despawnPartner(teambarIndex);
                break;
        }
    }

    public void restoreNavigation()
    {
        switch (monsterMode)
        {
            case monsterMode.Wild:
                navMeshAgent.enabled = true;
                this.GetComponent<RoamingAI>().enabled = true;
                break;
            case monsterMode.Partner:
                navMeshAgent.enabled = true;
                followController.enabled = true;
                followController.target = player.transform;
                navMeshAgent.speed = 15;
                break;
        }
    }

    public void evolveMonster()
    {
        Monster evolvedMonster = new Monster(monster.species, monster.shiny, monster.size, monster.currentLevel);

        // Transfer the current health and experience to the evolved monster
        evolvedMonster.currentHealth = monster.currentHealth;
        evolvedMonster.currentExp = monster.currentExp;

        // Spawn the evolved monster GameObject
        GameObject evolvedMonsterGO = evolvedMonster.spawn();

        // Set the references and properties for the evolved monster
        MonsterController evolvedMonsterController = evolvedMonsterGO.GetComponent<MonsterController>();
        evolvedMonsterController.monster = evolvedMonster;
        evolvedMonsterController.monsterMode = monsterMode;
        evolvedMonsterGO.transform.position = transform.position;
        Physics.SyncTransforms();
        evolvedMonsterController.generateMonster(monsterMode);
        evolvedMonsterController.recalculatePosition(1f);
        evolvedMonsterController.regenerateBbox();
        evolvedMonsterGO.transform.parent = transform.parent;

        Destroy(gameObject);

    }

    public void regenerateBbox()
    {
        StartCoroutine(bBoxgeneration());
    }
    public void recalculatePosition(float radius)
    {
        // Sample a point on the NavMesh closest to the target's position
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(transform.position, out navHit, radius, NavMesh.AllAreas))
        {
                // Set the sampled point as the new destination
                transform.position = navHit.position;
                if (!navMeshAgent.isOnNavMesh)
                {
                    recalculatePosition(radius+1f);
                }
        }
        else
        {
                // If sampling fails, fallback to directly setting the target's position
                recalculatePosition(radius+1f);
        }
    }
}
 public enum monsterMode {Wild,Partner,Enciclopedia};