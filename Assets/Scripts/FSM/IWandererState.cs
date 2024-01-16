using System.Collections;
using UnityEngine;

public interface IWandererState
{
    // Start is called before the first frame update
    void UpdateState();
    void ToWanderState();
    void ToIdleState();
}
