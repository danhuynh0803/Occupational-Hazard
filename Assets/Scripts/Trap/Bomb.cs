using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Bomb : Hazard {

    //public int bombDamage = 10;
    public float bombTimer;
    public float shakeTimer;
    public float shakeAmount;
    public float blastRadius = 3.0f;
    private Animator animator;
    private GameObject agentCarrying;
    [Header("Bomb radius indicator")]
    [Range(0, 50)]
    public int segments = 50;
    private LineRenderer line;
    private float originalBombTimer;

    private UI ui;

    void Start()
    {
        ui = FindObjectOfType<UI>();
        originalBombTimer = bombTimer;
        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = segments + 1;
        line.useWorldSpace = false;
        CreatePoints();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (agentCarrying != null)
        {            
            this.transform.position = 
                new Vector2(agentCarrying.transform.position.x - 0.5f,
                            agentCarrying.transform.position.y - 0.2f);

        }
        
        UpdateLineColor();
    }

    private void UpdateLineColor()
    {
        float t = bombTimer / originalBombTimer;
        line.startColor = Color.Lerp(Color.red, Color.white, t);
        line.endColor = Color.Lerp(Color.red, Color.white, t);
    }

    void CreatePoints()
    {
        float x;
        float y;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * blastRadius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * blastRadius;
            z = 0.0f;

            line.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / segments);
        }
    }

    void FixedUpdate()
    {
        if(bombTimer <= 0)
        {
            Explode();
            bombTimer = 100f;
        }
        else
        {
            bombTimer -= Time.fixedDeltaTime;
        }
    }

    // Move the position based on the agent carrying the bomb
    public void FollowAgent(GameObject agent)
    {
        agentCarrying = agent;
    }

    public void DefollowAgent()
    {
        agentCarrying = null;
    }

    void Explode()
    {
        animator.SetTrigger("Explode");
        ui.ClearControlText();
        transform.Translate(new Vector3(0f, 0f, -1f));
        Destroy(gameObject, 3f);
        CameraShake.Shake(shakeAmount, shakeTimer);
        SoundController.Play(SFX.Explode, 1f);
        line.positionCount = 0;
        // Deal damage to all actors within the radius
        Collider2D[] radiusObjects =
            Physics2D.OverlapCircleAll(
                new Vector2(transform.position.x, transform.position.y), 
                blastRadius);

        foreach (Collider2D col in radiusObjects)
        {
            //Debug.Log("Found objects in radius");
            Actor actor = col.gameObject.GetComponent<Actor>();
            if (actor != null)
            {
                actor.DecrementHealth(damage);
            }
        }
      
    }
}
