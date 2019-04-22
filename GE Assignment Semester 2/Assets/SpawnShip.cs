using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShip : MonoBehaviour
{
    public float spawnRatePerSecond;

    public string poolString;

    public bool ally;

    public GameObject ship;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    public IEnumerator Spawn() {
        while (true){
            yield return new WaitForSeconds(1f / spawnRatePerSecond);

            if(ship != null) {
                if (ally)
                {
                    if (CurrentShips.allyNumber < CurrentShips.instance.numberOfShipsPerSide)
                    {
                        Instantiate(ship, transform.position, Quaternion.identity);
                    }
                }
                else {
                    if (CurrentShips.enemyNumber < CurrentShips.instance.numberOfShipsPerSide)
                    {
                        Instantiate(ship, transform.position, Quaternion.identity);
                    }
                }
            }
        }
    }

}
