using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IWandererState
{
    private readonly Wanderer fsm;
    float time;

    public IdleState(Wanderer fsmWanderer)
    {
        fsm = fsmWanderer;
        
    }

    public void ToIdleState()
    {
        throw new System.NotImplementedException();
    }

    public void ToWanderState()
    {
        time = 0;
        fsm.actualState = fsm.wanderState;
    }

    public void UpdateState()
    {
        time += Time.deltaTime;
        if(time > 3)
        {
            ToWanderState();
        }
        

    }
}
