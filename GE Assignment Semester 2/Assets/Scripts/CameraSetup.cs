using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    public GameObject attached;
    public bool ally;

    public bool randomFoV;


    Camera cam;

    void Start()
    {
        SetUp();
    }

    void Update()
    {
        if (attached != null)
        {
            transform.position = attached.transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, attached.transform.rotation,Time.deltaTime * 2f);
        }
        else {
            transform.position = transform.position;
            GameObject closestShip = null;
            float closestDist = float.MaxValue;
            if (ally)
            {
                for (int i = 0; i < CurrentShips.instance.enemyShips.Count; i++)
                {
                    if (Vector3.Distance(transform.position, CurrentShips.instance.enemyShips[i].transform.position) < closestDist)
                    {
                        closestDist = Vector3.Distance(transform.position, CurrentShips.instance.enemyShips[i].transform.position);
                        closestShip = CurrentShips.instance.enemyShips[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < CurrentShips.instance.allyShips.Count; i++)
                {
                    if (Vector3.Distance(transform.position, CurrentShips.instance.allyShips[i].transform.position) < closestDist)
                    {
                        closestDist = Vector3.Distance(transform.position, CurrentShips.instance.allyShips[i].transform.position);
                        closestShip = CurrentShips.instance.allyShips[i];
                    }
                }
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(closestShip.transform.position - transform.position, Vector3.up), Time.deltaTime * 2f);
        }
    }

    void SetUp() {
        cam = GetComponent<Camera>();
        if (randomFoV) {
            cam.fieldOfView = Random.Range(55, 75);
        }
    }
}
