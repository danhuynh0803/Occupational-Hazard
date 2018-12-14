using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Can control 3 Agents at a time
    public GameObject[] agents = new GameObject[3];
    public GameObject VIP;
    public GameObject selectedAgent;
    public float agentSoundLevel = 0.5f;
    public RectTransform[] arrows;
    public float arrowOffsetX;
    public float arrowOffsetY;
    public Vector2[] viewPort;
    public Transform agentSpawnpoint;
    public GameObject agentObject;
    public float spawnCooldown = 1.0f;
    private float spawnTimer = 0.0f;
    private bool isRunning = false;
    private VIPController vipController;
    // Use this for initialization
    void Start () {
        // Start with 3 agents initialized
        vipController = FindObjectOfType<VIPController>();
        viewPort = new Vector2[4];
        spawnTimer = 0.0f;
        selectedAgent = agents[0];
    }

    // Update is called once per frame
    void Update ()
    {
        SelectAgent();
        SpawnAgent();

        if (Input.GetKeyDown(KeyCode.F) && selectedAgent != null)
        {
            GroupInactiveAgents(selectedAgent.transform, 0.0f);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            GroupInactiveAgents(vipController.transform, 2.0f);
        }
    }

    private void FixedUpdate()
    {        
        MoveAgent();
        MoveArrow();
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
                    if (selectedAgent == null)
                    {
                        selectedAgent = newAgent;
                    }
                    newAgent.GetComponentInChildren<TextMesh>().text = (i + 1).ToString();
                    spawnTimer = spawnCooldown;
                }
            }
        }
    }

    void SelectAgent()
    { 
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //CameraMovement.followingAgent = agents[0];
            selectedAgent = agents[0];
            SoundController.Play(Random.Range(1, 7), agentSoundLevel);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //CameraMovement.followingAgent = agents[1];
            selectedAgent = agents[1];
            SoundController.Play(Random.Range(1, 7), agentSoundLevel);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //CameraMovement.followingAgent = agents[2];
            selectedAgent = agents[2];
            SoundController.Play(Random.Range(1, 7), agentSoundLevel);
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
                    selectedAgent.GetComponent<Agent>()
                        .Movement(moveX, moveY);
                }
                else
                {
                    if (!go.GetComponent<Agent>().isFollowing)
                    {
                        go.GetComponent<Agent>()
                            .StopMovement();
                    }
                }
            }
        }
    }
    //4th arrow in the array is for VIP
    void MoveArrow()
    {
        //for agents
        for (int i = 0; i < agents.Length; i++)
        {
            if (agents[i] != null)
            {
                arrows[i].gameObject.SetActive(true);
                if (agents[i] == selectedAgent)
                {
                    arrows[i].gameObject.SetActive(false);
                }
                else
                {
                    Vector2 agentsPos = agents[i].transform.position;  // get the game object position
                    Vector2 viewportPoint = Camera.main.WorldToViewportPoint(agentsPos);  //convert game object position to VievportPoint
                    viewPort[i] = new Vector2(viewportPoint.x, viewportPoint.y);
                    if (viewportPoint.x < 1.0f &&
                        viewportPoint.x > 0 &&
                        viewportPoint.y < 1.0f &&
                        viewportPoint.y > 0f)
                    {
                        arrows[i].gameObject.SetActive(false);
                    }
                    else
                    if (viewportPoint.x > 1.0f && viewportPoint.y > 0f && viewportPoint.y < 1.0f)
                    {
                        arrows[i].anchorMin = new Vector2(1f, viewportPoint.y) - new Vector2(arrowOffsetX, 0);
                        arrows[i].anchorMax = new Vector2(1f, viewportPoint.y) - new Vector2(arrowOffsetX, 0);
                        arrows[i].eulerAngles = new Vector3(0f, 0f, 0f);
                    }
                    else
                    if (viewportPoint.x < 0f && viewportPoint.y > 0f && viewportPoint.y < 1.0f)
                    {
                        arrows[i].anchorMin = new Vector2(0f, viewportPoint.y) + new Vector2(arrowOffsetX, 0);
                        arrows[i].anchorMax = new Vector2(0f, viewportPoint.y) + new Vector2(arrowOffsetX, 0);
                        arrows[i].eulerAngles = new Vector3(0f, 0f, 180f);

                    }
                    else
                    if (viewportPoint.x > 0f && viewportPoint.x < 1.0f && viewportPoint.y > 1.0f)
                    {
                        arrows[i].anchorMin = new Vector2(viewportPoint.x, 1f) - new Vector2(0, arrowOffsetY);
                        arrows[i].anchorMax = new Vector2(viewportPoint.x, 1f) - new Vector2(0, arrowOffsetY);
                        arrows[i].eulerAngles = new Vector3(0f, 0f, 90f);

                    }
                    else
                    if (viewportPoint.x > 0f && viewportPoint.x < 1.0f && viewportPoint.y < 0f)
                    {
                        arrows[i].anchorMin = new Vector2(viewportPoint.x, 0f) + new Vector2(0, arrowOffsetY);
                        arrows[i].anchorMax = new Vector2(viewportPoint.x, 0f) + new Vector2(0, arrowOffsetY);
                        arrows[i].eulerAngles = new Vector3(0f, 0f, 270f);
                    }
                    else
                    if (viewportPoint.x <= 0f && viewportPoint.y <= 0f)
                    {
                        arrows[i].anchorMin = new Vector2(0f, 0f) + new Vector2(arrowOffsetX, arrowOffsetY);
                        arrows[i].anchorMax = new Vector2(0f, 0f) + new Vector2(arrowOffsetX, arrowOffsetY);
                        arrows[i].eulerAngles = new Vector3(0f, 0f, 225f);

                    }
                    else
                    if (viewportPoint.x <= 0f && viewportPoint.y >= 1.0f)
                    {
                        arrows[i].anchorMin = new Vector2(0f, 1f) + new Vector2(arrowOffsetX, -1 * arrowOffsetY);
                        arrows[i].anchorMax = new Vector2(0f, 1f) + new Vector2(arrowOffsetX, -1 * arrowOffsetY);
                        arrows[i].eulerAngles = new Vector3(0f, 0f, 135f);

                    }
                    else
                    if (viewportPoint.x >= 1.0f && viewportPoint.y <= 0f)
                    {
                        arrows[i].anchorMin = new Vector2(1f, 0f) + new Vector2(-1 * arrowOffsetX, arrowOffsetY);
                        arrows[i].anchorMax = new Vector2(1f, 0f) + new Vector2(-1 * arrowOffsetX, arrowOffsetY);
                        arrows[i].eulerAngles = new Vector3(0f, 0f, 315f);
                    }
                    else
                    if (viewportPoint.x >= 1.0f && viewportPoint.y >= 1.0f)
                    {
                        arrows[i].anchorMin = new Vector2(1f, 1f) + new Vector2(-1 * arrowOffsetX, -1 * arrowOffsetY);
                        arrows[i].anchorMax = new Vector2(1f, 1f) + new Vector2(-1 * arrowOffsetX, -1 * arrowOffsetY);
                        arrows[i].eulerAngles = new Vector3(0f, 0f, 45f);
                    }
                }
            }
            else
            {
                arrows[i].gameObject.SetActive(false);
            }
        }
        //for VIP
        if (VIP != null)
        {
            arrows[3].gameObject.SetActive(true);
            Vector2 VIPPos = VIP.transform.position;  // get the game object position
            Vector2 viewportPoint = Camera.main.WorldToViewportPoint(VIPPos);  //convert game object position to VievportPoint
            viewPort[3] = new Vector2(viewportPoint.x, viewportPoint.y);
            if (viewportPoint.x < 1.0f &&
                viewportPoint.x > 0 &&
                viewportPoint.y < 1.0f &&
                viewportPoint.y > 0f)
            {
                arrows[3].gameObject.SetActive(false);
            }
            else
            if (viewportPoint.x > 1.0f && viewportPoint.y > 0f && viewportPoint.y < 1.0f)
            {
                arrows[3].anchorMin = new Vector2(1f, viewportPoint.y) - new Vector2(arrowOffsetX, 0);
                arrows[3].anchorMax = new Vector2(1f, viewportPoint.y) - new Vector2(arrowOffsetX, 0);
                arrows[3].eulerAngles = new Vector3(0f, 0f, 0f);
            }
            else
            if (viewportPoint.x < 0f && viewportPoint.y > 0f && viewportPoint.y < 1.0f)
            {
                arrows[3].anchorMin = new Vector2(0f, viewportPoint.y) + new Vector2(arrowOffsetX, 0);
                arrows[3].anchorMax = new Vector2(0f, viewportPoint.y) + new Vector2(arrowOffsetX, 0);
                arrows[3].eulerAngles = new Vector3(0f, 0f, 180f);

            }
            else
            if (viewportPoint.x > 0f && viewportPoint.x < 1.0f && viewportPoint.y > 1.0f)
            {
                arrows[3].anchorMin = new Vector2(viewportPoint.x, 1f) - new Vector2(0, arrowOffsetY);
                arrows[3].anchorMax = new Vector2(viewportPoint.x, 1f) - new Vector2(0, arrowOffsetY);
                arrows[3].eulerAngles = new Vector3(0f, 0f, 90f);

            }
            else
            if (viewportPoint.x > 0f && viewportPoint.x < 1.0f && viewportPoint.y < 0f)
            {
                arrows[3].anchorMin = new Vector2(viewportPoint.x, 0f) + new Vector2(0, arrowOffsetY);
                arrows[3].anchorMax = new Vector2(viewportPoint.x, 0f) + new Vector2(0, arrowOffsetY);
                arrows[3].eulerAngles = new Vector3(0f, 0f, 270f);
            }
            else
            if (viewportPoint.x <= 0f && viewportPoint.y <= 0f)
            {
                arrows[3].anchorMin = new Vector2(0f, 0f) + new Vector2(arrowOffsetX, arrowOffsetY);
                arrows[3].anchorMax = new Vector2(0f, 0f) + new Vector2(arrowOffsetX, arrowOffsetY);
                arrows[3].eulerAngles = new Vector3(0f, 0f, 225f);

            }
            else
            if (viewportPoint.x <= 0f && viewportPoint.y >= 1.0f)
            {
                arrows[3].anchorMin = new Vector2(0f, 1f) + new Vector2(arrowOffsetX, -1 * arrowOffsetY);
                arrows[3].anchorMax = new Vector2(0f, 1f) + new Vector2(arrowOffsetX, -1 * arrowOffsetY);
                arrows[3].eulerAngles = new Vector3(0f, 0f, 135f);

            }
            else
            if (viewportPoint.x >= 1.0f && viewportPoint.y <= 0f)
            {
                arrows[3].anchorMin = new Vector2(1f, 0f) + new Vector2(-1 * arrowOffsetX, arrowOffsetY);
                arrows[3].anchorMax = new Vector2(1f, 0f) + new Vector2(-1 * arrowOffsetX, arrowOffsetY);
                arrows[3].eulerAngles = new Vector3(0f, 0f, 315f);
            }
            else
            if(viewportPoint.x >= 1.0f && viewportPoint.y >= 1.0f)
            {
                arrows[3].anchorMin = new Vector2(1f, 1f) + new Vector2(-1 * arrowOffsetX, -1 * arrowOffsetY);
                arrows[3].anchorMax = new Vector2(1f, 1f) + new Vector2(-1 * arrowOffsetX, -1 * arrowOffsetY);
                arrows[3].eulerAngles = new Vector3(0f, 0f, 45f);
            }
        }
        else
        {
            arrows[4].gameObject.SetActive(false);
        }
    
    }
    void GroupInactiveAgents(Transform followTransform, float followDist)
    {
        foreach (GameObject go in agents)
        {
            // Only move the selected agent
            // All other agents will no longer move
            if (go != null && go != selectedAgent)
            {
                if (!go.GetComponent<Agent>().IsBusy())
                {
                    go.GetComponent<Agent>().FollowVIP(followTransform, followDist);
                }
                else
                {
                    go.GetComponent<Agent>().isFollowing = false;
                }
            }
        }
    }
}
