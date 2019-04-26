using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderCamera : MonoBehaviour
{
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 90f, Time.deltaTime);
    }
}
