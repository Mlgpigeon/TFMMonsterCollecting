using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FollowController : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent nav;
    private BoxCollider boxCollider;
    public float top_speed = 10f;
    private Animator anim;
    private bool isWalking = false;
    private bool isRunning = false;
    private bool isTouchingWater = false;
    private bool onTerrain = false;

    public float minDistance = 5f;
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = GetComponent<MonsterController>().player.transform;
        boxCollider = GetComponent<BoxCollider>();
    }
    
   public void OnCollisionEnter(Collision collision)
   {
       if (collision.gameObject.CompareTag("Terrain"))
       {
           onTerrain = true;
       }
   } 
   private void OnCollisionExit(Collision collision)
   {
       if (collision.gameObject.CompareTag("Terrain"))
       {
           onTerrain = false;
       }
   }
   void Update()
   {
       if(nav.isOnNavMesh)
        {
            if(getWaterDistance() && !onTerrain)
            {
                nav.baseOffset = -boxCollider.size.y/2f;
            }
            else
            {
                nav.baseOffset = 0;
            }

            changeDestination(target);
            if(nav.remainingDistance < minDistance)
            {
               nav.velocity = Vector3.zero;
               nav.isStopped=true;
               isWalking = false;
               isRunning = false;
            }
            else if(nav.remainingDistance > 5f && nav.remainingDistance < 10f)
            {
                if (nav.velocity == Vector3.zero)
                {
                    nav.velocity = new Vector3(top_speed,top_speed,top_speed);
                }
                if(!isWalking){
                    nav.velocity = nav.velocity * 0.3f;
                    isWalking = true;
                    isRunning = false;
                }
                nav.isStopped=false;
                
            }
            else
            {   
                if(!isRunning){
                    isWalking = false;
                    isRunning = true;
                }
                nav.isStopped=false;
                
            }
            
            anim.SetBool("isRunning", isRunning);
            anim.SetBool("isWalking", isWalking);
            
        }
        else
        {
           {this.GetComponent<MonsterController>().recalculatePosition(1f);}
        }
    }

   public void changeDestination(Transform newTarget)
   {
       target = newTarget;
       nav.SetDestination(target.position);
       if (nav.remainingDistance == 0)
       {
           // Sample a point on the NavMesh closest to the target's position
           NavMeshHit navHit;
           if (NavMesh.SamplePosition(target.position, out navHit, 1f, NavMesh.AllAreas))
           {
               // Set the sampled point as the new destination
               nav.SetDestination(navHit.position);
           }
           else
           {
               // If sampling fails, fallback to directly setting the target's position
               nav.SetDestination(target.position);
           }
       }
   }
   
    public bool getWaterDistance()
    {
        float distanceToTerrain = 0f;
        Vector3 pointOnNavMesh = transform.position;
        Vector3 scaledMax = boxCollider.size * transform.localScale.y;
        float maxDistance = scaledMax.y;
        Vector3 nposition = transform.TransformPoint(transform.position);
        NavMeshHit navhit;
        if(nav.SamplePathPosition(NavMesh.GetAreaFromName("Swim"),maxDistance *2,out navhit))
        {
            return true;
        }
        return false;
        
    }
}
