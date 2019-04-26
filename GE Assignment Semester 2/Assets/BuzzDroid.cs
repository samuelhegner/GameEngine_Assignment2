using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuzzDroid : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Obiwan" || collision.gameObject.name == "Anakin") {
            GetComponent<Rigidbody>().isKinematic = true;
            transform.parent = collision.transform;
            transform.rotation = Quaternion.LookRotation(-collision.gameObject.transform.right, collision.gameObject.transform.up);
        }
        
        
    }
}
