using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetPursue : SteeringBehaviour
{
    public Boid leader;

    Vector3 targetPos;
    Vector3 worldTarget;
    public Vector3 offset;

    public float sideDistance;
    public float bottomDistance;

    public float slowingDistance;

    // Start is called before the first frame update
    void Start()
    {

    }

    public override Vector3 Calculate()
    {
        worldTarget = leader.transform.TransformPoint(offset);

        float dist = Vector3.Distance(worldTarget, transform.position);
        float time = dist / boid.maxSpeed;
        targetPos = worldTarget + (leader.velocity * time);
        return boid.ArriveForce(targetPos, slowingDistance);
    }
 
}
