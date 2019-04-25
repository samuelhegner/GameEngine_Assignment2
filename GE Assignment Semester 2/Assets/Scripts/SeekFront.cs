using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekFront : SteeringBehaviour
{
    public GameObject targetGameObject = null;
    public Vector3 target = Vector3.zero;

    public float distanceInFront;

    public override Vector3 Calculate()
    {
        return boid.ArriveForce(target);
    }

    public void Update()
    {
        if (targetGameObject != null)
        {
            target = targetGameObject.transform.TransformPoint(0, 0, distanceInFront);
        }
    } 
}
