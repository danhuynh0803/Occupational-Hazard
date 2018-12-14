using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public static Transform mainCamera;
    public static GameObject followingAgent;

    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        mainCamera = gameObject.transform;
    }

    void FixedUpdate()
    {
        FollowingAgent();
    }

    void FollowingAgent()
    {
        followingAgent = player.selectedAgent;
        if (followingAgent != null)
        {            
            transform.position = new Vector3(
                player.selectedAgent.transform.position.x,
                player.selectedAgent.transform.position.y,
                -20f);
        }
    }
}
