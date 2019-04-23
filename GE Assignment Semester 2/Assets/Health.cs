using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int startHealth;

    public int health;

    void Awake()
    {
        health = startHealth;
    }

    void Update()
    {
        if (health <= 0) {
            Destroy(gameObject);
        } 
    }

    public void RemoveHealth(int healthToRemove) {
        health -= healthToRemove;
    }
}
