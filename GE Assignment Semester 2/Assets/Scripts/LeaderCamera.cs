using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderCamera : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 90f, Time.deltaTime);
    }
}
