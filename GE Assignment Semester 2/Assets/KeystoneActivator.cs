using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeystoneActivator : MonoBehaviour
{
    CameraManager cameraManager;

    Path path;

    public int currentWaypoint;

    void Start()
    {
        cameraManager = GameObject.FindObjectOfType<CameraManager>();
        path = GameObject.FindObjectOfType<Path>();
    }

    void Update()
    {
        currentWaypoint = path.next;
    }
}
