using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerController : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform[] waypoints;
    public int waypointIndex;
    public bool forward;
    public bool isGhost;
    public Vector3 pos;
    Vector3 target;
    public List<Agent> agents;
    private NavMeshPath smoothedPath;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Random.Range(3f, 8f);
        smoothedPath = new NavMeshPath();
        NextWaypoint();
    }

    void Update()
    {
        if (Vector3.Distance(agent.transform.position, target) < 2)
        {

            NextIndex();
            NextWaypoint();
        }

    }

    void NextWaypoint()
    {

        target = waypoints[waypointIndex].position;
        UpdatePath();
        agent.SetPath(smoothedPath);
        agent.SetDestination(target);



    }
    void NextIndex()
    {
        if (forward)
        {
            waypointIndex++;
            if (waypointIndex == waypoints.Length)
            {
                waypointIndex = 0;
            }
        }
        else
        {
            waypointIndex--;
            if (waypointIndex == -1)
            {
                waypointIndex = waypoints.Length - 1;
            }
        }


    }

    public List<Vector3> SmoothPath(List<Vector3> originalPath)
    {
        List<Vector3> smoothedPath = new List<Vector3>();

        for (int i = 0; i < originalPath.Count; i++)
        {
            if (i == 0 || i == originalPath.Count - 1)
            {
                smoothedPath.Add(originalPath[i]);
            }
            else
            {
                Vector3 previous = originalPath[i - 1];
                Vector3 current = originalPath[i];
                Vector3 next = originalPath[i + 1];

                Vector3 midPoint1 = (previous + current) / 2;
                Vector3 midPoint2 = (current + next) / 2;

                smoothedPath.Add(current);
                smoothedPath.Add((current + midPoint1) / 2);
                smoothedPath.Add((current + midPoint2) / 2);
            }
        }

        return smoothedPath;
    }

    private void UpdatePath()
    {
        if (agent.hasPath)
        {
            List<Vector3> originalPathCorners = new List<Vector3>(agent.path.corners);
            List<Vector3> smoothedPathCorners = SmoothPath(originalPathCorners);
            agent.CalculatePath(smoothedPathCorners[smoothedPathCorners.Count - 1], smoothedPath);

        }
    }
}
