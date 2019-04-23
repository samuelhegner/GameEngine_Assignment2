using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constrain : SteeringBehaviour
{
    public GameObject centerObject;
    public float radius;


    void Start()
    {
        
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawSphere(centerObject.transform.position + centerObject.transform.forward * 500f, radius);
    }

    public override Vector3 Calculate()
    {
        Vector3 force = Vector3.zero;
        Vector3 toTarget = boid.transform.position - (centerObject.transform.position + centerObject.transform.forward * 500f);

        if (toTarget.magnitude > radius)
        {
            force = Vector3.Normalize(toTarget) * (radius - toTarget.magnitude);
        }

        return force;
    }
}
