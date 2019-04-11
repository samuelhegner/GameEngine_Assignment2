using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class LeadAllies : State
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

class FormationApproach : State
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

class ChaseDown : State
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

class HelpAlly : State
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

class ShakeEnemy : State
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

public class Arc170Controller : MonoBehaviour
{
    StateMachine stateMachine;

    public bool leader;

    public int number;

    void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        if (leader) {
            //gameObject.AddComponent
        }
    }

    void Start()
    {
        if (leader)
        {
            stateMachine.ChangeState(new LeadAllies());
        }
        else {
            stateMachine.ChangeState(new FormationApproach());
        }
    }

    void Update()
    {   
    }
}
