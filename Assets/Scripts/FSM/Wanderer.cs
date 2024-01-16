using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wanderer : MonoBehaviour
{

    [HideInInspector] public IWandererState actualState;
    [HideInInspector] public WanderState wanderState;
    [HideInInspector] public IdleState idleState;
    public NavMeshAgent agent;


    private void Awake()
    {
        wanderState = new WanderState(this);
        idleState = new IdleState(this);

        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        actualState = wanderState;
    }

    // Update is called once per frame
    void Update()
    {
        actualState.UpdateState();
    }

}
