using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    public GameObject parent;

    public GameObject explosionSmall;

    public int bulletDamage;




    // Start is called before the first frame update
    void Awake()
    {
        CancelInvoke();
        //Destroy itself after 15 seconds
        Invoke("DestroySelf", 15f);
    }

    // Update is called once per frame
    void Update()
    {
        //moves the bullet forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        //if the bullet his something other than the object that spawned it, explode and destroy the bullet
        if (other.gameObject != parent) {
            if (other.GetComponent<Health>()) {
                other.GetComponent<Health>().RemoveHealth(bulletDamage);
                Instantiate(explosionSmall, transform.position, Random.rotation);
            }

            DestroySelf();
        }
    }

    void DestroySelf()
    {
        GetComponent<TrailRenderer>().enabled = false;
        gameObject.SetActive(false);
    }
}
