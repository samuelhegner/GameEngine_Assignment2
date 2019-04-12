using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShip : MonoBehaviour
{
    public float spawnRatePerSecond;

    public string poolString;

    public bool usePool;

    public GameObject ship;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    public IEnumerator Spawn() {
        while (true){
            yield return new WaitForSeconds(1f / spawnRatePerSecond);

            if (usePool)
            {
                Object_Pool.Instance.SpawnFromPool(poolString, transform.position, Quaternion.identity);
            }
            else if(ship != null) {
                Instantiate(ship, transform.position, Quaternion.identity);
            }
        }
    }

}
