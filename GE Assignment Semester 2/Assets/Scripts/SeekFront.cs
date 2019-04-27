using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekFront : SteeringBehaviour
{
    public GameObject targetGameObject = null;
    public Vector3 target = Vector3.zero;

    public float distanceInFront;

    public float distanceToTarget;

    public override Vector3 Calculate()
    {
        distanceToTarget = Vector3.Distance(boid.transform.position, targetGameObject.transform.TransformPoint(0, 0, distanceInFront));
        return boid.ArriveForce(targetGameObject.transform.TransformPoint(0, 0, distanceInFront));
    }

    public void Update()
    {
        //if (targetGameObject != null)
        //{
            //target = targetGameObject.transform.TransformPoint(0, 0, distanceInFront);
        //}
    } 
}
