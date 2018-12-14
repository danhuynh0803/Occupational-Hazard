using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killborder : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Killborder OnTriggerEnter");
        if (collision.gameObject.tag == "Projectile")
        {
            Projectile proj = collision.gameObject.GetComponent<Projectile>();
            //Debug.Log("Hit killborder");
            if (proj != null)
            {
                proj.DestroyThis();
            }
        }
    }
}
