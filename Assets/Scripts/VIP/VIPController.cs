using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum VIP
{
    Moving,
    Standing,
    ChangeDirection,
    Death
}

public class VIPController : Actor {

    public float speed;
    public float movingTime;
    public float standingTime;
    public float targetGridX;
    public float targetGridY;
    public float maxXBound;
    public float minXBound;
    public float maxYBound;
    public float minYBound;
    public Vector2 randomDirectionVector;
    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audio;
    private TextMesh healthText;
    [SerializeField]
    private float movingTimer;
    [SerializeField]
    private float standingTimer;
    public VIP state;

    void Start()
    {
        currentHealth = health;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        healthText = GetComponentInChildren<TextMesh>();
        movingTimer = movingTime;
        standingTimer = standingTime;
        StartMoving();
        audio.Play();
        state = VIP.Moving;
        //state = VIP.Standing;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case VIP.Moving:
                if (movingTimer >= 0f)
                {
                    movingTimer -= Time.fixedDeltaTime;
                    CheckBound();
                }
                else
                {
                    StopMoving();
                    movingTimer = movingTime;
                    state = VIP.Standing;
                    audio.Stop();
                }
                break;
            case VIP.Standing:
                if (standingTimer >= 0f)
                {
                    standingTimer -= Time.fixedDeltaTime;
                }
                else
                {
                    standingTimer = standingTime;
                    state = VIP.ChangeDirection;
                }
                break;
            case VIP.ChangeDirection:
                StartMoving();
                audio.Play();
                state = VIP.Moving;
                break;
            case VIP.Death:
                break;
            default:
                break;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        UpdateHealthText();
    }

    void UpdateHealthText()
    {
        if (currentHealth > 0)
            healthText.text = currentHealth.ToString();
        else
            healthText.text = "No good";
    }


    void CheckBound()
    {
        float x = 0f;
        float y = 0f;
        if (transform.position.y >= maxYBound)
        {
            x = Random.Range(-1f, 1f);
            y = Random.Range(-1f, 0f);
            transform.position = new Vector2(transform.position.x, maxYBound - 0.1f);
            ChangeDirection(x, y);
        }
        else
        if (transform.position.y <= minYBound)
        {
            x = Random.Range(-1f, 1f);
            y = Random.Range(0f, 1f);
            transform.position = new Vector2(transform.position.x, minYBound + 0.1f);
            ChangeDirection(x, y);
        }
        else
        if (transform.position.x >= maxXBound)
        {
            x = Random.Range(-1f, 0f);
            y = Random.Range(-1f, 1f);
            transform.position = new Vector2(maxXBound - 0.1f, transform.position.y);
            ChangeDirection(x, y);
        }
        else
        if (transform.position.x <= minXBound)
        {
            x = Random.Range(0f, 1f);
            y = Random.Range(-1f, 1f);
            transform.position = new Vector2(minXBound + 0.1f, transform.position.y);
            ChangeDirection(x, y);
        }

    }
    void ChangeDirection(float x, float y)
    {
        float normalzation = 1f/GetNorm(x,y);
        randomDirectionVector = normalzation * new Vector2(x, y);
        rb.velocity = speed * randomDirectionVector;
        SetAnimatorTrigger(x, y);
    }

    void StopMoving()
    {
        rb.velocity = new Vector2(0f, 0f);
        animator.ResetTrigger("WalkRight");
        animator.ResetTrigger("WalkLeft");
        animator.ResetTrigger("WalkUp");
        animator.ResetTrigger("WalkDown");
        animator.SetTrigger("Stop");

    }

    float GetNorm(float x, float y)
    {
        return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
    }

    void StartMoving()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        ChangeDirection(x, y);

    }
    void SetAnimatorTrigger(float x, float y)
    {
        if(x > 0)
        {
            if (x > Mathf.Abs(y))
            {
                if(!animator.GetCurrentAnimatorStateInfo(0).IsName("WalkRight"))
                    animator.SetTrigger("WalkRight");
            }
            else
            {
                if(y > 0)
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("WalkUp"))
                        animator.SetTrigger("WalkUp");
                }
                else
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("WalkDown"))
                        animator.SetTrigger("WalkDown");
                }
            }
        }
        else
        {
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("WalkLeft"))
                    animator.SetTrigger("WalkLeft");
            }
            else
            {
                if (y > 0)
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("WalkUp"))
                        animator.SetTrigger("WalkUp");
                }
                else
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("WalkDown"))
                        animator.SetTrigger("WalkDown");
                }
            }
        }
    }
        
    public override void Death()
    {
        StopMoving();
        UpdateHealthText();
        audio.Stop();
        state = VIP.Death;
        GameController.isGameOver = true;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Spike")
        {
            if(!collision.gameObject.GetComponent<Spike>().IsGettingDisarmed)
            {
                //collision.gameObject.GetComponent<Spike>().GetSteppedByVip();
                Destroy(collision.gameObject);
                DecrementHealth(1);
                StopMoving();
                movingTimer = movingTime;
                state = VIP.Standing;
                audio.Stop();
            }

        }
    }
}
