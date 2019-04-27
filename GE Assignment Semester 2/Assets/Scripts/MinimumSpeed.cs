using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimumSpeed : SteeringBehaviour
{
    public float minMomentum;

    public override Vector3 Calculate()
    {
        return boid.transform.forward * minMomentum;
    }

    private void Start()
    {
        
    }
}
