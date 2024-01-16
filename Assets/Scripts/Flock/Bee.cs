using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Bee : MonoBehaviour
{
    CapsuleCollider beeCollider;
    public CapsuleCollider BeeCollider { get { return beeCollider; } }
    // Start is called before the first frame update
    void Start()
    {
        beeCollider = GetComponent<CapsuleCollider>();
    }

    public void Move(Vector3 velocity)
    {
        
        transform.forward = velocity;
        transform.Translate(velocity * Time.deltaTime);
    }
}
