using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Can control 3 Agents at a time
    public GameObject[] agents = new GameObject[3];
    public GameObject selectedAgent;

    public Transform agentSpawnpoint;
    public GameObject agentObject;
    public float spawnCooldown = 1.0f;
    private float spawnTimer = 0.0f;
    private bool isRunning = false;
    // Use this for initialization
    void Start () {
        // Start with 3 agents initialized
        spawnTimer = 0.0f;
        selectedAgent = agents[0];
    }

    // Update is called once per frame
    void Update ()
    {
        SelectAgent();
        SpawnAgent();
    }

    private void FixedUpdate()
    {
        MoveAgent();
    }

    private void SpawnAgent()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            for (int i = 0; i < agents.Length; ++i)
            {
                if (agents[i] == null)
                {
                    //Debug.Log("Spawning new agent");

                    GameObject newAgent =
                        Instantiate(agentObject, agentSpawnpoint.position, Quaternion.identity) 
                            as GameObject;
                    agents[i] = newAgent;                 

                    spawnTimer = spawnCooldown;
                }
            }
        }
    }

    void SelectAgent()
    { 
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CameraMovement.followingAgent = agents[0];
            selectedAgent = agents[0];
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CameraMovement.followingAgent = agents[1];
            selectedAgent = agents[1];
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CameraMovement.followingAgent = agents[2];
            selectedAgent = agents[2];
        }

        //Debug.Log(selectedAgent);
    }

    void MoveAgent()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        foreach (GameObject go in agents)
        {
            // Only move the selected agent
            // All other agents will no longer move
            if(go != null)
            {
                if (selectedAgent == go)
                {
                    //Debug.Log("X: " + moveX + "\tY: " + moveY);
                    selectedAgent.GetComponent<Agent>()
                        .Movement(moveX, moveY);
                }
                else
                {
                    go.GetComponent<Agent>()
                        .StopMovement();
                }
            }
        }
    }
}
