using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : SteeringBehaviour
{
    public Boid target;

    Vector3 targetPos;

    private void Start()
    {
    }

    public override Vector3 Calculate()
    {
        if (target != null)
        {
            float dist = Vector3.Distance(target.transform.position, transform.position);
            float time = dist / boid.maxSpeed;

            targetPos = target.transform.position + (target.velocity * time);
            return boid.SeekForce(targetPos);
        }
        else {
            return Vector3.zero;
        }
        
       
        
    }
}
