using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ChaseDownV : State
{
    public override void Enter()
    {

    }

    public override void Think()
    {

    }

    public override void Exit()
    {

    }
}

class ApproachEnemyV : State
{
    Seek seek;
    Arc170Controller controller;

    public override void Enter()
    {
        seek = owner.GetComponent<Seek>();

        foreach (GameObject enemy in CurrentShips.instance.allyShips)
        {
            if (enemy.GetComponent<Arc170Controller>().leader)
            {
                seek.targetGameObject = enemy;
            }
        }

        controller = owner.GetComponent<Arc170Controller>();

        seek.enabled = true;

        
    }

    public override void Think()
    {

    }

    public override void Exit()
    {
        seek.enabled = false;
    }
}

class HelpAllyV : State
{
    public override void Enter()
    {

    }

    public override void Think()
    {

    }

    public override void Exit()
    {

    }
}

class ShakeEnemyV : State
{
    public override void Enter()
    {

    }

    public override void Think()
    {

    }

    public override void Exit()
    {

    }
}

public class VultureController : MonoBehaviour
{

    public Boid allyNeedsHelp;
    public Boid enemyToChase;

    StateMachine stateMachine;


    void Awake()
    {
        CurrentShips.AddEnemy(gameObject);
        transform.parent = GameObject.FindGameObjectWithTag("Manager").transform;

        stateMachine = GetComponent<StateMachine>();
    }

    void Start()
    {
        stateMachine.ChangeState(new ApproachEnemyV());
    }

    void Update()
    {
        
    }

    void OnDestroy()
    {
        CurrentShips.RemoveEnemy(gameObject);
    }
}
