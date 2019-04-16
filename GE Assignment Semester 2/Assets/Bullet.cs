using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    public GameObject parent;


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
        if(other.gameObject != parent)
        DestroySelf();
    }

    void TurnOnCol()
    {
        GetComponent<BoxCollider>().enabled = true;
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
