using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentShips : MonoBehaviour
{

    public List<GameObject> enemyShips = new List<GameObject>();

    public List<GameObject> allyShips = new List<GameObject>();

    public static CurrentShips instance;

    public int numberOfShipsPerSide = 20;

    public float constrainDistance;


    void Awake()
    {
        instance = this;
    }

    public static void AddEnemy(GameObject ship) {
        instance.enemyShips.Add(ship);
    }

    public static void AddAlly(GameObject ship) {
        instance.allyShips.Add(ship);
    }

    public static void RemoveEnemy(GameObject ship)
    {
        instance.enemyShips.Remove(ship);
    }

    public static void RemoveAlly(GameObject ship)
    {
        instance.allyShips.Remove(ship);
    }

    public static int enemyNumber
    {
        get {
            return instance.enemyShips.Count;
        }
    }

    public static int allyNumber
    {
        get
        {
            return instance.allyShips.Count;
        }
    }
}
