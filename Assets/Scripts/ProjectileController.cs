using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController: MonoBehaviour {

    public float waveCooldown;
    public float shotCooldown;
    public float shotDelay = 2.0f;
    public GameObject projectile;
    public int numShots;

    public Transform minBound;
    public Transform maxBound;

    [SerializeField]
    private float timer;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    LineRenderer lr;

	// Use this for initialization
	void Start () {       
        // Dont fire until 10 to 15 seconds in
        timer = Random.Range(8.0f, 15.0f); 
        //timer = 0.5f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer -= Time.deltaTime;
	    if (timer <= 0.0f)
        {
            StartCoroutine(FireWave());
            //StartCoroutine(FireRandomStraightLine());
            timer = waveCooldown;
        }
	}

    IEnumerator FireWave()
    {
        for (int i = 0; i < numShots; ++i)
        {
            StartCoroutine(FireRandomStraightLine());
            yield return new WaitForSeconds(shotCooldown);
        }

        timer = Random.Range(waveCooldown, waveCooldown + 10.0f);
    }

    IEnumerator FireRandomStraightLine()
    {
        Vector3 startPos, endPos;

        // Coin flip between shooting from bottom or top
        if (Random.Range(0.0f, 1.0f) > 0.5f)
        {
            // Shoot from bottom to top        
            startPos = new Vector3(
                Random.Range(minBound.position.x, maxBound.position.x),
                             minBound.position.y,
                             minBound.position.z);

            endPos = new Vector3(
              Random.Range(minBound.position.x, maxBound.position.x),
                           maxBound.position.y,
                           minBound.position.z);
        }
        else
        {
            // Shoot from top to bottom
            startPos = new Vector3(
                Random.Range(minBound.position.x, maxBound.position.x),
                             maxBound.position.y,
                             minBound.position.z);

            endPos = new Vector3(
              Random.Range(minBound.position.x, maxBound.position.x),
                           minBound.position.y,
                           minBound.position.z);
        }

        GameObject projGO =
            Instantiate(projectile, startPos, Quaternion.identity) as GameObject;
        projGO.GetComponent<Projectile>().MoveTowards(endPos);
        yield return new WaitForSeconds(0.5f);        
    }
    
    void FollowAndFire()
    {
        Vector3 startPos;

        // Coin flip between shooting from bottom or top
        if (Random.Range(0.0f, 1.0f) > 0.5f)
        {
            // Shoot from bottom to top        
            startPos = new Vector3(
                Random.Range(minBound.position.x, maxBound.position.x),
                             minBound.position.y,
                             minBound.position.z);
        }
        else
        {
            // Shoot from top to bottom
            startPos = new Vector3(
                Random.Range(minBound.position.x, maxBound.position.x),
                             maxBound.position.y,
                             minBound.position.z);
        }
    }


}
