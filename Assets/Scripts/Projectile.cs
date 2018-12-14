using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Hazard {
 
    public float speed = 7.0f;
    private bool canMove = false;

    Rigidbody2D rb2d;
    public Vector3 targetPos;
    Vector3 direction;

    GameObject lineObject;

	// Use this for initialization
	void Start () {
        canMove = false;
        rb2d = GetComponent<Rigidbody2D>();        
        //Object.Destroy(this.gameObject, 10.0f);
	}

    private void Update()
    {
        //Debug.Log(direction);    
        if (canMove)
        {
            rb2d.velocity = direction * speed;
        }
        else
        {
            rb2d.velocity = direction * 0;
        }
    }

    IEnumerator DelayMovement(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        canMove = true;
        SoundController.Play(SFX.GunShot, 0.7f);
    }

    //public void MoveTowards(Vector3 target, GameObject lineRender)
    public void MoveTowards(Vector3 target)
    {
        //Debug.Log("In MoveTowards");
        // Move torwards the target position
        targetPos = target;
        direction = targetPos - this.transform.position;
        direction.Normalize();
        DrawLine(this.transform.position, target);
        StartCoroutine(DelayMovement(1.5f));
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("DragLine", typeof(LineRenderer));
        
        LineRenderer line = lineObj.GetComponent<LineRenderer>();        
        line.startWidth = 0.5f;
        line.endWidth = 0.1f;
        line.startColor = new Color(255, 255, 255, 0.2f);
        line.endColor = new Color(255, 120, 120, 0.5f);
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        Material spriteMat = new Material(Shader.Find("Sprites/Default"));
        line.material = spriteMat;

        lineObject = lineObj;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Actor actor = collision.gameObject.GetComponent<Actor>();
        if (actor != null)
        {
            actor.DecrementHealth(damage);

            DestroyThis();
        }

        if (collision.gameObject.tag == "KillBorder")
        {
            //Debug.Log("Hit killborder");
            DestroyThis();
        }
    }

    public void DestroyThis()
    {
        Object.Destroy(this.gameObject);
        ClearLine();
        Object.Destroy(lineObject);
    }

    void ClearLine()
    {
        LineRenderer lr = lineObject.GetComponent<LineRenderer>();
        lr.SetPosition(0, Vector3.zero);
        lr.SetPosition(1, Vector3.zero);
    }
}
