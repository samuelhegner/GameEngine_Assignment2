using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : SteeringBehaviour {

    public Path path;

    Vector3 nextWaypoint;

    public float slowingDistance = 15.0f;

    public void OnDrawGizmos()
    {
        if (isActiveAndEnabled && Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, nextWaypoint);
        }
    }

    public void Start()
    {
        
    }

    public override Vector3 Calculate()
    {
        nextWaypoint = path.NextWaypoint();
        if (Vector3.Distance(transform.position, nextWaypoint) < 10f)
        {
            path.AdvanceToNext();
        }

        if (!path.looped && path.IsLast())
        {
            return boid.ArriveForce(nextWaypoint, slowingDistance); 
        }
        else
        {
            return boid.SeekForce(nextWaypoint);
        }
    }
}
