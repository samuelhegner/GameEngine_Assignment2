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

        }
    }

    public override void Exit()
    {

    }
}

class MissileChase : State
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

class MissileExplode : State
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

public class MissileController : MonoBehaviour
{
    StateMachine stateMachine;

    public bool launch;

    void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        stateMachine.ChangeState(new MissileIdle());
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
