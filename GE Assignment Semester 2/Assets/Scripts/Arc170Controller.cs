using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class LeadAllies : State
{
    Pursue pursue;


    public override void Enter()
    {
        pursue = owner.GetComponent<Pursue>();
        pursue.target = GameObject.Find("Target").GetComponent<Boid>();
        pursue.enabled = true;
    }

    public override void Think()
    {

    }

    public override void Exit()
    {
        pursue.enabled = false;
    }
}

class FormationApproach : State
{

    OffsetPursue offsetPursue;
    Arc170Controller controller;

    public override void Enter()
    {
        offsetPursue = owner.GetComponent<OffsetPursue>();
        controller = owner.GetComponent<Arc170Controller>();

        Arc170Controller[] allies = GameObject.FindObjectsOfType<Arc170Controller>();

        foreach(Arc170Controller ally in allies) {
            if (ally.leader) {
                offsetPursue.leader = ally.GetComponent<Boid>();
            }
        }

        offsetPursue.offset = new Vector3((offsetPursue.sideDistance * controller.formationNumber)
            , -(Mathf.Abs(offsetPursue.bottomDistance * controller.formationNumber))
            , 0f);

        offsetPursue.enabled = true;

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

    public int formationNumber;

    void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        if (leader) {
            List<Arc170Controller> allies = new List<Arc170Controller>();
            Arc170Controller[] _temp = GameObject.FindObjectsOfType<Arc170Controller>();

            for (int i = 0; i < _temp.Length; i++) {
                if (_temp[i] != this) {
                    allies.Add(_temp[i]);
                }
            }

            AssignSquad(allies.ToArray());
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

    void AssignSquad(Arc170Controller[] allies) {

        int num = 1;

        for (int i = 0;  i < allies.Length; i++) {
            if (i % 2 != 0)
            {
                allies[i].formationNumber = num;
                num++;
            }
            else {
                allies[i].formationNumber = -num;
            }
        }
    }
}
