using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeystoneActivator : MonoBehaviour
{

    //Key moments in the game get triggered in this script based on which waypoint the jedi ship is at

    CameraManager cameraManager;

    public Camera FollowCam1;
    public Camera FollowCam2;
    public Camera DoorCam;

    bool trigger1 = false;
    bool trigger2 = false;

    Path path;

    public GameObject vulture;

    public int currentWaypoint;

    void Start()
    {
        cameraManager = GameObject.FindObjectOfType<CameraManager>();
        path = GameObject.FindObjectOfType<Path>();
    }

    void Update()
    {
        currentWaypoint = path.next;
        if (currentWaypoint > 11)
        {
            if (!trigger2) {
                cameraManager.active = false;
                
                trigger2 = true;
                DoorCam.enabled = true;

                GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

                foreach (GameObject door in doors) {
                    door.GetComponent<Door>().move = true;
                }

                Invoke("LastSwitch", 20f);
            }
        }
        else if (currentWaypoint > 9)
        {
            if (!trigger1) {
                cameraManager.active = false;
                Destroy(cameraManager.newCamera);
                Destroy(cameraManager.previousCamera);
                vulture.SetActive(true);
                FollowCam2.gameObject.SetActive(true);
                vulture.transform.Find("Missile").GetComponent<MissileController>().launch = true;
                trigger1 = true;
            }
            
        }
        else if (currentWaypoint > 6)
        {
            cameraManager.active = true;
            FollowCam1.enabled = false;
        }
    }

    void LastSwitch() {
        cameraManager.active = true;
    }
}
