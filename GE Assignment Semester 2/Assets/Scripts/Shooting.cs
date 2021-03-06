﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public LayerMask mask;

    public bool shoot;

    public float shotsPerSecond;
    public float updateShootPerSecond;
    public float radiusOfSphereCast;
    public float maxSphereCastDistance;

    List<GameObject> cannons = new List<GameObject>();
    Quaternion[] ogRotation = new Quaternion[2];

    public GameObject bullet;

    public GameObject bin;

    public Arc170Controller controller;

    AudioSource audioSource;

    GameObject objLastScanned;

    void Start()
    {
        objLastScanned = null;
        bin = GameObject.FindGameObjectWithTag("Bin");
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.tag == "Cannon")
            {
                cannons.Add(transform.GetChild(i).gameObject);
            }
        }

        ogRotation[0] = cannons[0].transform.rotation;
        ogRotation[1] = cannons[1].transform.rotation;

        StartCoroutine(CheckShoot());
        StartCoroutine(Shoot());

        if (GetComponent<Arc170Controller>()) {
            controller = GetComponent<Arc170Controller>();
        }
    }

    void Update()
    {
        if (controller != null && shoot && controller.enemyToChase != null && objLastScanned != null && objLastScanned == controller.enemyToChase.gameObject)
        {
            foreach (GameObject cannon in cannons)
            {
                cannon.transform.LookAt(controller.enemyToChase.transform.position);
            }
        }
        else if (controller != null) {
            for(int i = 0; i < cannons.Count; i++)
            {
                cannons[i].transform.rotation = ogRotation[i];
            }
        }
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(Random.Range(0, 0.5f));
        while (true)
        {
            if (shoot) {
                foreach (GameObject cannon in cannons) {
                    GameObject newBullet;
                    if (cannons.Count == 4)
                    {
                         newBullet = Object_Pool.Instance.SpawnFromPool("BulletRed", cannon.transform.position, cannon.transform.rotation);
                    }
                    else { 
                         newBullet = Object_Pool.Instance.SpawnFromPool("BulletBlue", cannon.transform.position, cannon.transform.rotation);
                    }

                    newBullet.GetComponent<Bullet>().parent = gameObject;
                    newBullet.GetComponent<TrailRenderer>().enabled = true;
                    newBullet.GetComponent<TrailRenderer>().Clear();
                    //newBullet.transform.parent = bin.transform; 
                }
                audioSource.Stop();
                audioSource.Play();
            }
            yield return new WaitForSeconds(1f / shotsPerSecond);
        }
    }

    IEnumerator CheckShoot()
    {
        yield return new WaitForSeconds(Random.Range(0, 0.5f));
        while (true)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit info;
            bool tempCheck = Physics.SphereCast(ray, radiusOfSphereCast, out info, maxSphereCastDistance);

            if (tempCheck)
            {
                if (info.transform.tag != transform.tag && info.transform.tag != "Bullet")
                {
                    shoot = true;
                    objLastScanned = info.transform.gameObject;
                }
                else {
                    objLastScanned = null;
                    shoot = false;
                }
            }
            else {
                objLastScanned = null;
                shoot = false;
            }
            
            yield return new WaitForSeconds(1f / updateShootPerSecond);
        }
    }

    public void OnDrawGizmos()
    {
        if (shoot)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }

        Gizmos.DrawLine(transform.position, (transform.position + transform.forward * maxSphereCastDistance));
    }
}
