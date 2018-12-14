using System;
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
    public bool isFollowing = false;
    private bool isRunning = false;
    private bool isDisarmingSpike = false;
    private GameObject disarmingSpike;
    private UI ui;
    private bool isHoldingObject;
    private VIPController vip;
    private Transform vipTransform;
    private Transform followingTransform;
    private float stopDistance;

    public bool IsBusy()
    {
        return isDisarmingSpike; // || || ||;
    }

    // Use this for initialization
    void Start() {
        vip = FindObjectOfType<VIPController>();
        vipTransform = vip.transform;
        
        isHoldingObject = false;
        isFollowing = false;
        ui = FindObjectOfType<UI>();
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

        if (this.gameObject == playerController.selectedAgent)
        {
            if (isHoldingObject)
            {
                ui.UpdateControlText("Press Space to drop");
            }                        
            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }
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

        if (isFollowing)
        {            
            float distance = Vector3.Distance(followingTransform.position, this.transform.position);
            Vector3 direction = followingTransform.position - this.transform.position;
            direction.Normalize();
            SetAnimation(direction.x, direction.y);
            if (distance > 3.0f)
            {
                rb2d.velocity = direction * speed;                
            }
            // Stop when close to the VIP
            else if (distance <= stopDistance)
            {
                StopMovement();
                rb2d.velocity = direction * 0;
            }
            // Slow down agent when nearing VIP
            else
            {
                //StopMovement();
                rb2d.velocity = direction * (speed - 3.0f);
                //SetAnimation(0.0f, 0.0f);
            }
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }

    // Controls movement based on the inputs from the player controller. 
    // To avoid moving all three agents, Movement should only be called from the PlayerController
    public void Reposition(Vector3 newPosition)
    {
        transform.Translate(newPosition * Time.deltaTime);
    }

    public void FollowVIP(Transform transform, float stopDistance)
    {
        followingTransform = transform;
        this.stopDistance = stopDistance;
        isFollowing = true;
        /*
        float distance = Vector3.Distance(this.transform.position, vipTransform.position);
        float t = distance / speed;

        transform.position = 
            Vector3.Lerp(this.transform.position, 
                         vipTransform.position, 
                         t);
        */
    }

    public void Movement(float moveX, float moveY)
    {        
        isFollowing = false;
        Vector2 dir = new Vector2(moveX, moveY);
        dir.Normalize();
        newlySpawned = false;

        //Debug.Log("isRunning = " + isRunning);
        int disarmingToInt = (int)Convert.ToInt32(!isDisarmingSpike);
        if (isRunning)
        {
            //putting disarming spike condition here
            rb2d.velocity = disarmingToInt * runSpeed * dir;
        }
        else
        {
            rb2d.velocity = disarmingToInt * speed * dir;
        }
       
        if(!isDisarmingSpike)
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

    void DisarmingSpike(GameObject spike)
    {
        disarmingSpike = spike;
        isDisarmingSpike = true;
        isFollowing = false;
        StopMovement();
    }

    public void DisarmedSpike()
    {
        isDisarmingSpike = false;
        disarmingSpike = null;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bomb")
        {
            //Debug.Log("On top of bomb");
            // Pick up the bomb only if the agent is currently selected
            if (this.gameObject == playerController.selectedAgent)
            {         
                if (!isHoldingObject)
                {
                    ui.UpdateControlText("Press Space to Pickup");
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        collision.gameObject.GetComponent<Bomb>().FollowAgent(gameObject);
                        isHoldingObject = true;
                    }
                }
                else
                {
                    ui.UpdateControlText("Press Space to Drop");
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        collision.gameObject.GetComponent<Bomb>().DefollowAgent();
                        isHoldingObject = false;
                    }
                }
            }
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bomb")
        {
            //Debug.Log("On top of bomb");
            // Pick up the bomb only if the agent is currently selected
            if (this.gameObject == playerController.selectedAgent)
            {
                ui.ClearControlText();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spike")
        {
            if (!isDisarmingSpike && !newlySpawned)
            {
                DisarmingSpike(collision.gameObject);
                collision.gameObject.GetComponent<Spike>().GettingDisarmed(gameObject);
            }
        }

        if (collision.gameObject.tag == "Bomb")
        {
            //Debug.Log("On top of bomb");
            // Pick up the bomb only if the agent is currently selected
            if (this.gameObject == playerController.selectedAgent)
            {
                if (!isHoldingObject)
                {
                    ui.UpdateControlText("Press Space to Pickup");
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        collision.gameObject.GetComponent<Bomb>().FollowAgent(gameObject);
                        isHoldingObject = true;
                    }
                }
            }
        }
    }

    public override void Death()
    {
        int SFXIndex = UnityEngine.Random.Range(9, 12);
        SoundController.Play(SFXIndex);
        // Clean up the PlayerController's selectedAgent var
        // if the destroyed agent was the one selected
        if (playerController.selectedAgent == this.gameObject)
        {                 
            playerController.selectedAgent = null;        
        }
        if( disarmingSpike != null)
        {
            disarmingSpike.GetComponent<Spike>().RemoveDisarmingAgent(gameObject);
        }
        GameController.deathCount++;

        UnityEngine.Object.Destroy(this.gameObject);
    }
}
