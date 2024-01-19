using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using UnityEngine.AI;

public class RollerAgent : Unity.MLAgents.Agent
{
    Rigidbody rBody;
    
    float episodeStartTime;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public Transform Target;
    public Transform Obstacle;
    public override void OnEpisodeBegin()
    {
        episodeStartTime = Time.time;
        // If the Agent fell, zero its momentum
        if (transform.position.y < 0)
        {
            rBody.angularVelocity = Vector3.zero;
            rBody.velocity = Vector3.zero;
            transform.position = new Vector3(23, 1.0f, 12);
        }

        // Move the target to a new spot
        //float xPos = Random.value * 18 - 9;
        //float zPos = Random.value * 18 - 9;
        //while((xPos >= -2 && xPos <= 2) && (zPos >= -2 && zPos <= 2))
        //{
        //    xPos = Random.value * 18 - 9;
        //    zPos = Random.value * 18 - 9;
        //}
        Vector3 point;
        if (RandomPoint(Obstacle.position, range, out point))
        {
            Debug.DrawRay(point, Vector3.up*10, Color.blue, 10.0f);
        }
        //Target.localPosition = new Vector3(xPos, 0.5f, zPos);
        Target.position = new Vector3(point.x, 1.0f, point.z);

        //Randomize Obstacle
        //Obstacle.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
        //Obstacle.rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
        //Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.5f);
        //foreach (Collider collider in hitColliders)
        //{
        //    if (collider.gameObject.tag == "Target" || collider.gameObject.tag == "Obstacle")
        //    {
        //        //Randomize Obstacle
        //        Obstacle.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
        //        Obstacle.rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
        //    }
        //}
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.position);
        sensor.AddObservation(transform.position);
        
    }

    public float forceMultiplier = 10;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float moveSpeed = 5f;
        // Actions, size = 2
        /*
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);
        */
        float mRotate = actionBuffers.ContinuousActions[0];
        float mForward = actionBuffers.ContinuousActions[1];

        rBody.MovePosition(transform.position + transform.forward * mForward * moveSpeed * Time.deltaTime);
        transform.Rotate(0f, mRotate * moveSpeed, 0f, Space.Self);
        // Fell off platform
        if (transform.localPosition.y < 0)
        {
            EndEpisode();
        }
        if (Time.time - episodeStartTime > 10.0f)
        {
            //End the episode
            SetReward(-2.0f);
            //EndEpisode();
        }

    }
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(this.transform.position, 0.5f);
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "Target")
        {
            SetReward(1.0f);
            EndEpisode();
        } else if(collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "wanderer")
        {
            SetReward(-1.0f);
            //EndEpisode();
        }
    }
    public float range = 10.0f;
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
