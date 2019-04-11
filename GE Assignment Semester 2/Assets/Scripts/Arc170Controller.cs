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
