using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//controller for the missile

//state before the missile is launched
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

//state that gets some clearance from the ship
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

//State that seeks the from on the jedi ship
class MissileChase : State
{ 
    MissileController controller;

    SeekFront seekFront;

    public override void Enter()
    {
        controller = owner.GetComponent<MissileController>();
        seekFront = owner.GetComponent<SeekFront>();

        seekFront.targetGameObject = GameObject.Find("Anakin");
        seekFront.enabled = true;
    }

    public override void Think()
    {
        seekFront = owner.GetComponent<SeekFront>();
        if (seekFront.distanceToTarget < 50f) {
            owner.ChangeState(new MissileExplode());
        }
    }

    public override void Exit()
    {
        seekFront.enabled = false;
    }
}

//state that explodes the missile and creates the buzz droids
class MissileExplode : State
{
    MissileController controller;


    public override void Enter()
    {
        controller = owner.GetComponent<MissileController>();

        MonoBehaviour.Instantiate(controller.explosionLarge, owner.transform.position, Random.rotation);

        for (int i = 0; i < controller.droids.Count; i++) {
            controller.droids[i].transform.parent = GameObject.Find("Bin").transform;
            controller.droids[i].transform.rotation = Random.rotation;
            controller.droids[i].transform.position = owner.transform.TransformPoint(Random.insideUnitSphere * 4f);
            controller.droids[i].SetActive(true);
        }

        controller.DestroyMissile();
    }

    public override void Think()
    {

    }

    public override void Exit()
    {

    }
}

//controller for the missile
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

    public void DestroyMissile()
    {
        gameObject.SetActive(false);
        droids.Clear();
        Destroy(gameObject, 2f);
    }
}
