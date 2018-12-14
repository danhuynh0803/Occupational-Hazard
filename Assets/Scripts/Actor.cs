using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour {

    public int health;
    [SerializeField]
    protected int currentHealth;

    public abstract void Death();

    public void DecrementHealth(int damage)
    {
        currentHealth -= damage;
    }
}
