using System.Collections;
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

    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.tag == "Cannon")
            {
                cannons.Add(transform.GetChild(i).gameObject);
            }
        }

        StartCoroutine(CheckShoot());
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(Random.Range(0, 0.5f));
        while (true)
        {
            if (shoot) {
                foreach (GameObject cannon in cannons) {
                    GameObject newBullet = Instantiate(bullet, cannon.transform.position, cannon.transform.rotation);
                    newBullet.GetComponent<Bullet>().parent = gameObject;
                }
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
                }
                else {
                    shoot = false;
                }
            }
            else {
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
