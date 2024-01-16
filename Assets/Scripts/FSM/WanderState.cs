using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderState : IWandererState
{
    private readonly Wanderer fsm;
    GameObject collider;
    GameObject[] wanderers;

    private bool isMoving = false;
    //private bool isResting = false;
    Vector3 randDest;
    NavMeshHit hit;
    float time;
    float startTime;
    public WanderState(Wanderer fsmWanderer)
    {
        fsm = fsmWanderer;
        collider = GameObject.FindGameObjectWithTag("BenchCollider");
        wanderers = GameObject.FindGameObjectsWithTag("wanderer");
    }

    public void ToIdleState()
    {
        fsm.actualState = fsm.idleState;
    }

    public void ToWanderState()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState()
    {
        time += Time.deltaTime;
        
            if ((fsm.agent.GetComponent<NavMeshAgent>().velocity.magnitude < 0.8f || (time % 5 >= 0 && time % 5 < 0.2f) && !isMoving))
            {
                randDest = RandomNavmeshLocation();
            fsm.agent.GetComponent<NavMeshAgent>().SetDestination(randDest);
                isMoving = true;
            }
            if (time % 5 >= 0 && time % 5 < 0.2f)
            {
                isMoving = false;
            }

            Vector3 spherePosition = fsm.agent.transform.position;

            // Check for colliders within the specified radius.
            Collider[] colliders = Physics.OverlapSphere(spherePosition, 0.1f);

            foreach (Collider collider in colliders)
            {
                // Check if the collider has the specified tag.
                if (collider.CompareTag("BenchCollider") && !isMoving)
                {
                    ToIdleState();
                }

            }
  
    }

    

    public Vector3 RandomNavmeshLocation()
    {
        //Vector3 randomDirection = Random.insideUnitSphere * radius;
        //NavMeshHit hit;
        //Vector3 finalPosition = Vector3.zero;
        //if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        //{
        //    finalPosition = hit.position;
        //}
        //return finalPosition;
        
        float x = Random.Range(10, 40);
        float z = Random.Range(10, 40);
        Vector3 test = new Vector3(x, 0, z);
        //if(NavMesh.SamplePosition(test, out hit, 0.1f, 1 << NavMesh.AllAreas))
        //{
        //    Debug.Log("Out");
        //    x = Random.Range(10, 40);
        //    z = Random.Range(10, 40);
        //    test = new Vector3(x, 0, z);
        //    Debug.Log(test);
        //}
        return test;
    }

}
