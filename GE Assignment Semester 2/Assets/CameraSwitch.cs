using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    Arrive arrive;

    public GameObject otherCamera;

    void Start()
    {
        arrive = GetComponent<Arrive>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf) {
            if (arrive.targetGameObject == null) {

                otherCamera.GetComponent<Camera>().enabled = true;
                GetComponent<Camera>().enabled = false;

                Invoke("KillCamera", 15f);
            }
        }
    }

    void KillCamera() {
        GameObject.FindObjectOfType<CameraManager>().active = true;
        Destroy(gameObject);
    }
}
