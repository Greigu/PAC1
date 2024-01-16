using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
    public Transform[] waypoints; 
    public GameObject player;

    public GameObject wanderer;
    public Transform wandererWaypoint;
    private int r;
    // Start is called before the first frame update
    void Start()
    {
        SpawnRunners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnRunners()
    {

        for (int i = 0; i < 15; i++)
        {
            r = Random.Range(0, waypoints.Length);
            GameObject a = Object.Instantiate(player, waypoints[r].position, waypoints[r].rotation);
            a.GetComponent<PlayerController>().waypointIndex = r;
            int forward = Random.Range(0, 2);
            a.GetComponent<PlayerController>().forward = forward == 1 ? true : false;            
        }
    }

}
