﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public GameObject cameraPrefab;
    public GameObject leaderCameraPrefab;
    public GameObject sideCameraPrefab;


    public GameObject previousCamera;
    public GameObject newCamera;

    public float longestTime;
    public float shortestTime;

    public bool active = true;

    public int switchesMade = 2;

    void Start()
    {
        StartCoroutine(ChangeCamera());
    }

    void Update()
    {
        
    }

    IEnumerator ChangeCamera() {
        while (true) {
            if (active) {
                float ranTime = 0;
                if (switchesMade == 0)
                {
                    if (newCamera != null)
                    {
                        previousCamera = newCamera;
                    }

                    CreateLeaderCamera();
                    switchesMade++;

                    if (previousCamera != null)
                    {
                        Invoke("DestroyCamera", 1f);
                    }

                    ranTime = longestTime;
                }
                else if (switchesMade == 1)
                {
                    if (newCamera != null)
                    {
                        previousCamera = newCamera;
                    }

                    CreateSideCamera();
                    switchesMade++;

                    if (previousCamera != null)
                    {
                        Invoke("DestroyCamera", 1f);
                    }

                    ranTime = longestTime * 2f;
                }
                else if (switchesMade > 1 && switchesMade % 2 != 0)
                {
                    switchesMade++;
                    GameObject newPosition = SearchForNewCameraPosition(CurrentShips.instance.allyShips.ToArray(), true);

                    if (newCamera != null) {
                        previousCamera = newCamera;
                    }
                    
                    newCamera = Instantiate(cameraPrefab, transform.position, transform.rotation);
                    newCamera.GetComponent<Camera>().enabled = false;
                    newCamera.GetComponent<CameraSetup>().attached = newPosition;
                    newCamera.transform.rotation = newCamera.GetComponent<CameraSetup>().attached.transform.rotation;
                    newCamera.GetComponent<CameraSetup>().ally = true;



                    if (previousCamera != null) {
                        Invoke("DestroyCamera", 1f);
                    }

                    ranTime = Random.Range(shortestTime, longestTime);
                }
                else if (switchesMade > 1 && switchesMade % 2 == 0) {
                    switchesMade++;
                    GameObject newPosition = SearchForNewCameraPosition(CurrentShips.instance.enemyShips.ToArray(), false);

                    if (newCamera != null)
                    {
                        previousCamera = newCamera;
                    }

                    newCamera = Instantiate(cameraPrefab, transform.position, transform.rotation);
                    newCamera.GetComponent<CameraSetup>().attached = newPosition;
                    newCamera.transform.rotation = newCamera.GetComponent<CameraSetup>().attached.transform.rotation;
                    newCamera.GetComponent<CameraSetup>().ally = false;

                    if (previousCamera != null)
                    {
                        Destroy(previousCamera);
                    }
                    ranTime = Random.Range(shortestTime, longestTime);
                }

                
                yield return new WaitForSeconds(ranTime);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void CreateSideCamera() {
        GameObject objToSpawn = GameObject.Find("SideCamera");

        newCamera = Instantiate(sideCameraPrefab, transform.position, transform.rotation);
        newCamera.GetComponent<Camera>().enabled = true;
        newCamera.GetComponent<CameraSetup>().attached = objToSpawn;
        newCamera.transform.rotation = newCamera.GetComponent<CameraSetup>().attached.transform.rotation;
        newCamera.GetComponent<CameraSetup>().ally = true;
    }

    public GameObject SearchForNewCameraPosition(GameObject [] arrayToCheck, bool ally) {
        GameObject possibleObj = null;

        float distFromObjToChase = float.MaxValue;
        bool front = false;

        for (int i = 0; i < arrayToCheck.Length; i++) {
            if (ally == true)
            {
                if (arrayToCheck[i] != null) {
                    Arc170Controller controller = arrayToCheck[i].GetComponent<Arc170Controller>();
                    if (controller.enemyChasing != null
                        && Vector3.Distance(controller.enemyChasing.transform.position, controller.transform.position) < distFromObjToChase
                        || controller.enemyToChase != null
                        && Vector3.Distance(controller.enemyToChase.transform.position, controller.transform.position) < distFromObjToChase)
                    {
                        
                        possibleObj = controller.gameObject;
                        if (controller.enemyToChase != null)
                        {
                            distFromObjToChase = Vector3.Distance(controller.enemyToChase.transform.position, controller.transform.position);
                            front = false;
                        }
                        else
                        {
                            distFromObjToChase = Vector3.Distance(controller.enemyChasing.transform.position, controller.transform.position);
                            front = true;
                        }
                    }
                }
                
            }
            else {
                if (arrayToCheck[i] != null)
                {
                    VultureController controller = arrayToCheck[i].GetComponent<VultureController>();
                    if (controller.enemyToChase != null
                        && Vector3.Distance(controller.enemyToChase.transform.position, controller.transform.position) < distFromObjToChase
                        || controller.enemyChasing != null
                        && Vector3.Distance(controller.enemyChasing.transform.position, controller.transform.position) < distFromObjToChase)
                    {
                        possibleObj = controller.gameObject;
                        if (controller.enemyToChase != null)
                        {
                            distFromObjToChase = Vector3.Distance(controller.enemyToChase.transform.position, controller.transform.position);
                            front = false;
                        }
                        else
                        {
                            distFromObjToChase = Vector3.Distance(controller.enemyChasing.transform.position, controller.transform.position);
                            front = true;
                        }
                    }
                }
            }
        }



        if (possibleObj != null && front)
        {
            return possibleObj.transform.Find("CameraFront").gameObject;
        }
        else if (possibleObj != null && !front)
        {
            return possibleObj.transform.Find("CameraBack").gameObject;
        }
        else {
            int ran = Random.Range(0, 2);
            if (ran == 0)
            {
                arrayToCheck = CurrentShips.instance.allyShips.ToArray();
                return arrayToCheck[Random.Range(0, arrayToCheck.Length)].transform.Find("CameraBack").gameObject;
            }
            else {
                arrayToCheck = CurrentShips.instance.enemyShips.ToArray();
                return arrayToCheck[Random.Range(0, arrayToCheck.Length - 1)].transform.Find("CameraBack").gameObject;
            }
        }
    }

    private void DestroyCamera()
    {
        newCamera.GetComponent<Camera>().enabled = true;
        Destroy(previousCamera);
    }

    void CreateLeaderCamera() {
        GameObject objToSpawn = null;

        for (int i = 0; i < CurrentShips.instance.allyShips.Count; i++) {
            if (CurrentShips.instance.allyShips[i].GetComponent<Arc170Controller>().leader) {
                objToSpawn = CurrentShips.instance.allyShips[i].transform.Find("CameraLeader").gameObject;
            }
        }

        

        newCamera = Instantiate(leaderCameraPrefab, transform.position, transform.rotation);
        newCamera.GetComponent<Camera>().enabled = true;
        newCamera.GetComponent<CameraSetup>().attached = objToSpawn;
        newCamera.transform.rotation = newCamera.GetComponent<CameraSetup>().attached.transform.rotation;
        newCamera.GetComponent<CameraSetup>().ally = true;
    }
}
