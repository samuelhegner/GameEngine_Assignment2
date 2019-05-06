using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject pointToMoveTo;

    public float speed;
    public bool move;

    void Start()
    {
        
    }

    //closes the door
    void Update()
    {
        if(move)
        transform.position = Vector3.Lerp(transform.position, pointToMoveTo.transform.position, Time.deltaTime * speed);
    }
}
