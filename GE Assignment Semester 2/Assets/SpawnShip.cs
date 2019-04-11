using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShip : MonoBehaviour
{
    public float spawnRatePerSecond;
    public bool usePool;

    public GameObject ship;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    public IEnumerator Spawn() {
        while (true){
            if (usePool)
            {
                Object_Pool.Instance.SpawnFromPool("Vulture", transform.position, Quaternion.identity);
            }
            else {
                Instantiate(ship, transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(1f / spawnRatePerSecond);
        }
    }

}
