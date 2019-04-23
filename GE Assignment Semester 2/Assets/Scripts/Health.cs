using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int startHealth;

    public int health;

    public GameObject[] smokes;

    bool smoke;

    void Awake()
    {
        health = startHealth;
    }

    void Update()
    {
        if (health <= 0) {
            Destroy(gameObject);
        }

        if (health < startHealth / 2f)
        {
            smoke = true;
        }
        else {
            smoke = false;
        }

        if (smoke) {
            foreach (GameObject smoke in smokes) {
                smoke.SetActive(true);
            }
        }
    }

    public void RemoveHealth(int healthToRemove) {
        health -= healthToRemove;
    }
}
