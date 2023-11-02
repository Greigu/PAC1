using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    NavMeshAgent agent;
    GameObject dest;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        dest = GameObject.FindGameObjectWithTag("Dest");
    }

    // Update is called once per frame
    void Update()
    {
        //agent.Move(transform.forward * Time.deltaTime*agent.speed);
        agent.SetDestination(dest.transform.position);
    }
}
