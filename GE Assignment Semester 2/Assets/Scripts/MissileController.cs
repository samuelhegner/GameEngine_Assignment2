using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MissileIdle : State {

    MissileController controller;


    public override void Enter()
    {
        controller = owner.GetComponent<MissileController>();
    }

    public override void Think()
    {
        if (controller.launch) {
            owner.ChangeState(new MissileClearShip());
        }
    }

    public override void Exit()
    {

    }
}

class MissileClearShip : State {

    MissileController controller;

    Arrive arrive;

    public override void Enter()
    {
        controller = owner.GetComponent<MissileController>();
        arrive = owner.GetComponent<Arrive>();
        
        arrive.targetPosition = owner.transform.TransformPoint(new Vector3(0
            , -40f
            , 75f));

        arrive.enabled = true;
    }

    public override void Think()
    {
        if (Vector3.Distance(owner.transform.position, arrive.targetPosition) < 100f) {
            owner.ChangeState(new MissileChase());
        }
    }

    public override void Exit()
    {
        arrive.enabled = false;
        owner.transform.parent = null;
    }
}

class MissileChase : State
{ 
    MissileController controller;

    SeekFront seekFront;

    public override void Enter()
    {
        controller = owner.GetComponent<MissileController>();
        seekFront = owner.GetComponent<SeekFront>();

        seekFront.targetGameObject = GameObject.Find("Obiwan");
        seekFront.enabled = true;
    }

    public override void Think()
    {
        

        if (Vector3.Distance(owner.transform.position, seekFront.target) < 50f) {
            owner.ChangeState(new MissileExplode());
        }
    }

    public override void Exit()
    {
        seekFront.enabled = false;
    }
}

class MissileExplode : State
{
    MissileController controller;


    public override void Enter()
    {
        controller = owner.GetComponent<MissileController>();

        MonoBehaviour.Instantiate(controller.explosionLarge, owner.transform.position, Random.rotation);

        for (int i = 0; i < controller.droids.Count; i++) {
            controller.droids[i].transform.parent = null;
            controller.droids[i].transform.rotation = Random.rotation;
            controller.droids[i].transform.position = owner.transform.TransformPoint(Random.insideUnitSphere * 4f);
            controller.droids[i].SetActive(true);
        }

        MonoBehaviour.Destroy(owner.gameObject);
    }

    public override void Think()
    {

    }

    public override void Exit()
    {

    }
}

public class MissileController : MonoBehaviour
{
    StateMachine stateMachine;

    public GameObject parent;

    public bool launch;

    public GameObject explosionLarge;
    public GameObject buzzDroid;

    public int numberOfBuzzDroids;

    public List<GameObject> droids = new List<GameObject>();

    void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        stateMachine.ChangeState(new MissileIdle());


        for (int i = 0; i < numberOfBuzzDroids; i++) {
            GameObject buzz = Instantiate(buzzDroid, transform.TransformPoint(Random.insideUnitSphere * 10f), Random.rotation, transform);
            buzz.SetActive(false);
            droids.Add(buzz);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
