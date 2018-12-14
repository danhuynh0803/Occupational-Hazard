using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : Actor {

    [Header("Speed Settings")]
    public float speed; // walk speed
    public float runSpeed = 10.0f;
    public Vector3 reposition;
    public Vector3 beginPosition;
    public Vector3 targetPosition;
    Rigidbody2D rb2d;
    private PlayerController playerController;
    private Animator animator;
    public float walkTimer = 1.5f;
    public float timer;
    public bool newlySpawned;
    private bool isRunning = false;

    // Use this for initialization
    void Start() {
        currentHealth = health;
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();
        timer = walkTimer;
        beginPosition = transform.position;
        animator.SetTrigger("WalkDown");
        targetPosition = transform.position + reposition;
    }

    // Update is called once per frame
    void Update() {
        if (timer >= 0f)
            timer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        if (currentHealth <= 0)
        {
            Death();
        }

        if (newlySpawned)
        {
            //Debug.Log("NewSpawnedAgent");
            if (timer > 0.0f)
            {
                transform.position = Vector3.Lerp(beginPosition, targetPosition, 1f - timer/ walkTimer);
            }
            else
            {
                StopMovement();
                newlySpawned = false;
            }
        }
    }

    // Controls movement based on the inputs from the player controller. 
    // To avoid moving all three agents, Movement should only be called from the PlayerController
    public void Reposition(Vector3 newPosition)
    {
        transform.Translate(newPosition * Time.deltaTime);
    }

    public void Movement(float moveX, float moveY)
    {
        Vector2 dir = new Vector2(moveX, moveY);
        dir.Normalize();

        //Debug.Log("isRunning = " + isRunning);
        if (isRunning)
        {
            rb2d.velocity = runSpeed * dir;
        }
        else
        {
            rb2d.velocity = speed * dir;
        }
        SetAnimation(moveX, moveY);
    }
    void SetAnimation(float moveX, float moveY)
    {
        if (moveX > 0 && moveY > 0)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("WalkUp"))
                animator.SetTrigger("WalkUp");
        }
        else
        if (moveX > 0 && moveY < 0)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("WalkRight"))
                animator.SetTrigger("WalkRight");
        }
        else
        if (moveX < 0 && moveY > 0)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("WalkLeft"))
                animator.SetTrigger("WalkLeft");
        }
        else
        if (moveX < 0 && moveY < 0)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("WalkDown"))
                animator.SetTrigger("WalkDown");
        }
        else
        if ((int)moveX == 0 && moveY < 0)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("WalkDown"))
                animator.SetTrigger("WalkDown");
        }
        else
        if ((int)moveX == 0 && moveY > 0)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("WalkUp"))
                animator.SetTrigger("WalkUp");
        }
        else
        if ((int)moveY == 0 && moveX > 0)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("WalkRight"))
                animator.SetTrigger("WalkRight");
        }
        else
        if ((int)moveY == 0 && moveX < 0)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("WalkLeft"))
                animator.SetTrigger("WalkLeft");
        }
        else
        if ((int)moveY == 0 && (int)moveX == 0)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("IdleRight") ||
                !animator.GetCurrentAnimatorStateInfo(0).IsName("IdleLeft") ||
                !animator.GetCurrentAnimatorStateInfo(0).IsName("IdleUp") ||
                !animator.GetCurrentAnimatorStateInfo(0).IsName("IdleDown"))
            {
                animator.ResetTrigger("WalkRight");
                animator.ResetTrigger("WalkLeft");
                animator.ResetTrigger("WalkUp");
                animator.ResetTrigger("WalkDown");
                animator.SetTrigger("Stop");
            }
        }
    }

    public void StopMovement()
    {
        rb2d.velocity = new Vector2(0f, 0f);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("IdleRight") ||
            !animator.GetCurrentAnimatorStateInfo(0).IsName("IdleLeft") ||
            !animator.GetCurrentAnimatorStateInfo(0).IsName("IdleUp") ||
            !animator.GetCurrentAnimatorStateInfo(0).IsName("IdleDown"))
        {
            animator.ResetTrigger("WalkRight");
            animator.ResetTrigger("WalkLeft");
            animator.ResetTrigger("WalkUp");
            animator.ResetTrigger("WalkDown");
            animator.SetTrigger("Stop");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Spike")
        {
            Death();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bomb")
        {
            //Debug.Log("On top of bomb");
            // Pick up the bomb only if the agent is currently selected
            if (this.gameObject == playerController.selectedAgent)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //collision.gameObject.transform.SetParent(this.transform);
                    //collision.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    collision.gameObject.GetComponent<Bomb>().FollowAgent(gameObject);
                }
            }
        }
    }

    public override void Death()
    {
        // Clean up the PlayerController's selectedAgent var
        // if the destroyed agent was the one selected
        if (playerController.selectedAgent == this.gameObject)
        {                 
            playerController.selectedAgent = null;        
        }

        GameController.deathCount++;

        Object.Destroy(this.gameObject);
    }
}
