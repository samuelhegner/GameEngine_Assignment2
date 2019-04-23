using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    public GameObject parent;

    public int bulletDamage;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroySelf", 15f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != parent) {
            if (other.GetComponent<Health>()) {
                other.GetComponent<Health>().RemoveHealth(bulletDamage);
            }

            DestroySelf();
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
