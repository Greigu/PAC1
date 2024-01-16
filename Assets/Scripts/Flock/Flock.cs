using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public Bee beePrefab;
    List<Bee> bees = new List<Bee>();
    
    
    [Range(10, 50)]
    public int numberBees = 10;
    const float density = 0.08f;

    [Range(1f, 50f)]
    public float driveFactor = 1f;
    [Range(1f, 50f)]
    public float maxSpeed = 2f;
    [Range(1f, 10f)]
    public float neighborRadius = 5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMult = 1f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }


    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMult * avoidanceRadiusMult;

        for (int i = 0; i < numberBees; i++)
        {
            Vector3 instPos;
            instPos = (Random.insideUnitSphere * numberBees * density) + transform.position;
            if(instPos.y < 0)
            {
                instPos.y = 0.5f;
            }
            Bee newBee = Instantiate(
                beePrefab, instPos,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
                );
            newBee.name = "Bee " + i;
            bees.Add( newBee );
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Bee agent in bees)
        {
            List<Transform> context = GetNearbyObjects(agent);
            
            Vector3 move = ((AllignmentMove(agent, context) + CohesionMove(agent, context) + AvoidanceMove(agent, context)));
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move * maxSpeed;
            }
            agent.Move(move);
        }
    }
    List<Transform> GetNearbyObjects(Bee agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);
        foreach (Collider c in contextColliders)
        {
            if(c != agent.BeeCollider)
            {
                if (c.gameObject.CompareTag("Bee"))
                context.Add(c.transform);
            }
        }
        return context;
    }

    private Vector3 CohesionMove(Bee agent, List<Transform> context)
    {
        if(context.Count == 0) {
            return Vector3.zero;
        }
        Vector3 cohesionMove = Vector3.zero;
        foreach (Transform item in context)
        {
            cohesionMove += item.position;
        }
        cohesionMove /= context.Count;

        cohesionMove -= agent.transform.position;
        return cohesionMove;
    }

    private Vector3 AllignmentMove(Bee agent, List<Transform> context)
    {
        if (context.Count == 0)
        {
            return agent.transform.forward;
        }
        Vector3 allignmentMove = Vector3.zero;
        foreach (Transform item in context)
        {
            allignmentMove += item.transform.forward;
        }
        allignmentMove /= context.Count;

        return allignmentMove;
    }

    private Vector3 AvoidanceMove(Bee agent, List<Transform> context)
    {
        if (context.Count == 0)
        {
            return Vector3.zero;
        }
        Vector3 avoidanceMove = Vector3.zero;
        int nAvoid = 0;
        foreach (Transform item in context)
        {
            if(Vector3.SqrMagnitude(item.position - agent.transform.position) < squareAvoidanceRadius)
            {
                nAvoid++;
                avoidanceMove += agent.transform.position - item.position;
            }
            
        }
        if(nAvoid > 0)
        {
            avoidanceMove /= nAvoid;

        }

        return avoidanceMove;
    }
}
