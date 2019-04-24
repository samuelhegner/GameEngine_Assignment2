using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class LeadAlliesR : State
{
    Pursue pursue;
    Arc170Controller controller;


    public override void Enter()
    {
        controller = owner.GetComponent<Arc170Controller>();
        pursue = owner.GetComponent<Pursue>();
        pursue.target = GameObject.Find("Target").GetComponent<Boid>();
        pursue.enabled = true;
        controller.busy = false;
    }

    public override void Think()
    {
        if (controller.enemyToChase != null)
        {
            pursue.target = controller.enemyToChase;
        }

        if (CurrentShips.enemyNumber > 0)
        {
            controller.enemyToChase = CurrentShips.instance.enemyShips[0].GetComponent<Boid>();
        }

        if (controller.enemyToChase != null)
        {
            if (Vector3.Distance(owner.transform.position, controller.enemyToChase.transform.position) < 300f)
            {
                foreach (GameObject obj in CurrentShips.instance.allyShips)
                {
                    obj.GetComponent<StateMachine>().ChangeStateDelayed(new AllocateStateR(), Random.Range(0, 1f));
                }
            }
        }

    }

    public override void Exit()
    {
        controller.leader = false;
        pursue.enabled = false;

        owner.GetComponent<Constrain>().centerObject = GameObject.Find("Anakin");

        owner.GetComponent<Constrain>().radius = CurrentShips.instance.constrainDistance;
        owner.GetComponent<Constrain>().enabled = true;
    }
}

class FormationApproachR : State
{

    OffsetPursue offsetPursue;
    Arc170Controller controller;

    public override void Enter()
    {
        offsetPursue = owner.GetComponent<OffsetPursue>();
        controller = owner.GetComponent<Arc170Controller>();
        controller.busy = false;



        Arc170Controller[] allies = GameObject.FindObjectsOfType<Arc170Controller>();

        foreach (Arc170Controller ally in allies)
        {
            if (ally.leader)
            {
                offsetPursue.leader = ally.GetComponent<Boid>();
            }
        }

        offsetPursue.offset = new Vector3((offsetPursue.sideDistance * controller.formationNumber)
            , -(Mathf.Abs(offsetPursue.bottomDistance * controller.formationNumber))
            , 0f);

        offsetPursue.enabled = true;

        owner.GetComponent<Boid>().maxSpeed = owner.GetComponent<Boid>().maxSpeed + 20f;
    }

    public override void Think()
    {
    }

    public override void Exit()
    {
        owner.GetComponent<Boid>().maxSpeed = owner.GetComponent<Boid>().maxSpeed - 20f;
        offsetPursue.enabled = false;

        owner.GetComponent<Constrain>().centerObject = GameObject.Find("Anakin");
        owner.GetComponent<Constrain>().radius = CurrentShips.instance.constrainDistance;

        owner.GetComponent<Constrain>().enabled = true;
    }
}

class ChaseDownR : State
{
    Pursue pursue;

    Arrive arrive;

    Arc170Controller controller;

    public override void Enter()
    {
        controller = owner.GetComponent<Arc170Controller>();
        controller.busy = true;


        pursue = owner.GetComponent<Pursue>();
        arrive = owner.GetComponent<Arrive>();

        if (controller.enemyToChase != null)
        {
            pursue.target = controller.enemyToChase;
            arrive.targetGameObject = controller.enemyToChase.gameObject;

            pursue.enabled = true;
        }
        else
        {
            owner.ChangeState(new WaitWanderR());
        }
        owner.GetComponent<Boid>().maxSpeed = owner.GetComponent<Boid>().maxSpeed + 10f;

    }

    public override void Think()
    {
        if (controller.enemyToChase == null)
        {
            if (controller.enemyChasing != null)
            {
                owner.ChangeState(new ShakeEnemyR());
            }
            else if (controller.enemyChasing == null)
            {
                owner.ChangeState(new AllocateStateR());
            }
            else
            {
                owner.ChangeState(new AllocateStateR());
            }
        }

        if (controller.enemyToChase != null) {
            if (Vector3.Distance(owner.transform.position, controller.enemyToChase.transform.position) > controller.pursueDistance)
            {
                pursue.enabled = true;
                arrive.enabled = false;
            }
            else
            {
                pursue.enabled = false;
                arrive.enabled = true;
            }
        }
    }

    public override void Exit()
    {
        pursue.enabled = false;
        arrive.enabled = false;
        owner.GetComponent<Arc170Controller>().enemyToChase = null;

        controller.busy = false;
        owner.GetComponent<Boid>().maxSpeed = owner.GetComponent<Boid>().maxSpeed - 10f;
    }
}

class HelpAllyR : State
{
    Pursue pursue;

    Arrive arrive;

    Boid enemy;

    Arc170Controller controller;

    public override void Enter()
    {
        controller = owner.GetComponent<Arc170Controller>();
        controller.busy = true;

        pursue = owner.GetComponent<Pursue>();
        arrive = owner.GetComponent<Arrive>();
        enemy = controller.allyNeedsHelp.GetComponent<Arc170Controller>().enemyChasing;

        if (enemy != null)
        {
            pursue.target = enemy;
            arrive.targetGameObject = enemy.gameObject;
            pursue.enabled = true;
        }
        else
        {
            owner.ChangeState(new WaitWanderR());
        }
        owner.GetComponent<Boid>().maxSpeed = owner.GetComponent<Boid>().maxSpeed + 10f;
    }

    public override void Think()
    {
        if (controller.allyNeedsHelp == null)
        {
            controller.enemyToChase = enemy;
            owner.ChangeState(new ChaseDownR());
        }
        else if (enemy == null)
        {
            controller.allyNeedsHelp = null;
            owner.ChangeState(new AllocateStateR());
        }
        else
        {
            if (Vector3.Distance(owner.transform.position, enemy.transform.position) > controller.pursueDistance)
            {
                pursue.enabled = true;
                arrive.enabled = false;
            }
            else
            {
                pursue.enabled = false;
                arrive.enabled = true;
            }
        }
    }

    public override void Exit()
    {
        pursue.enabled = false;
        arrive.enabled = false;
        owner.GetComponent<Arc170Controller>().allyNeedsHelp = null;

        controller.busy = false;

        owner.GetComponent<Boid>().maxSpeed = owner.GetComponent<Boid>().maxSpeed - 10f;
    }
}

class ShakeEnemyR : State
{
    NoiseWander[] wanders;
    Arc170Controller controller;

    public override void Enter()
    {
        wanders = owner.GetComponents<NoiseWander>();
        foreach (NoiseWander wander in wanders)
        {
            wander.enabled = true;
        }
        controller = owner.GetComponent<Arc170Controller>();
        controller.busy = true;
        controller.needsHelp = true;
    }

    public override void Think()
    {
        if (controller.enemyChasing == null)
        {
            owner.ChangeState(new AllocateStateR());
        }
    }

    public override void Exit()
    {
        foreach (NoiseWander wander in wanders)
        {
            wander.enabled = false;
        }

        controller.busy = false;
        controller.needsHelp = false;
    }
}

class WaitWanderR : State
{

    NoiseWander[] wanders;
    Arc170Controller controller;

    public override void Enter()
    {
        wanders = owner.GetComponents<NoiseWander>();
        foreach (NoiseWander wander in wanders)
        {
            wander.enabled = true;
        }
        controller = owner.GetComponent<Arc170Controller>();
        controller.busy = false;
        owner.ChangeStateDelayed(new AllocateStateR(), 5f);
    }

    public override void Think()
    {

    }

    public override void Exit()
    {
        wanders = owner.GetComponents<NoiseWander>();
        foreach (NoiseWander wander in wanders)
        {
            wander.enabled = false;
        }
    }
}

class AllocateStateR : State
{
    Arc170Controller controller;

    List<GameObject> enemies;
    List<GameObject> allies;

    public override void Enter()
    {
        owner.GetComponent<Constrain>().enabled = true;
        controller = owner.GetComponent<Arc170Controller>();
        controller.busy = false;

        controller.enemyToChase = null;
        controller.allyNeedsHelp = null;
        controller.enemyChasing = null;

        enemies = CurrentShips.instance.enemyShips;
        allies = CurrentShips.instance.allyShips;


        GameObject newEnemy = null;
        GameObject newAlly = null;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                if (!enemies[i].GetComponent<VultureController>().busy)
                {
                    newEnemy = enemies[i];
                }
            }

        }

        for (int i = 0; i < allies.Count; i++)
        {
            if (allies[i].GetComponent<Arc170Controller>().needsHelp && allies[i].GetComponent<Arc170Controller>().enemyChasing != null && allies[i].GetComponent<Arc170Controller>().enemyChasing.GetComponent<VultureController>().enemyChasing == null)
            {
                newAlly = allies[i];
            }
        }

        if (newEnemy != null)
        {
            controller.enemyToChase = newEnemy.GetComponent<Boid>();

            newEnemy.GetComponent<VultureController>().enemyChasing = owner.GetComponent<Boid>();
            newEnemy.GetComponent<VultureController>().enemyToChase = null;
            newEnemy.GetComponent<VultureController>().allyNeedsHelp = null;
            newEnemy.GetComponent<StateMachine>().ChangeState(new ShakeEnemyV());
            newEnemy.GetComponent<StateMachine>().CancelDelayedStateChange();


            owner.ChangeState(new ChaseDownR());
        }
        else if (newAlly != null)
        {
            controller.allyNeedsHelp = newAlly.GetComponent<Boid>();

            controller.allyNeedsHelp.GetComponent<Arc170Controller>().allyNeedsHelp = null;
            controller.allyNeedsHelp.GetComponent<Arc170Controller>().enemyToChase = null;

            controller.enemyToChase = controller.allyNeedsHelp.GetComponent<Arc170Controller>().enemyChasing;
            controller.enemyToChase.GetComponent<VultureController>().enemyChasing = owner.GetComponent<Boid>();
            controller.enemyToChase.GetComponent<VultureController>().allyNeedsHelp = null;
            owner.ChangeState(new HelpAllyR());
        }
        else if (newAlly == null && newEnemy == null)
        {
            owner.ChangeState(new WaitWanderR());
        }
    }

    public override void Exit()
    {

    }
}

public class Arc170Controller : MonoBehaviour
{
    public Boid allyNeedsHelp;
    public Boid enemyToChase;
    public Boid enemyChasing;

    StateMachine stateMachine;

    public bool leader;

    public bool needsHelp;

    public bool busy;


    public float pursueDistance;

    public string currentState;



    public int formationNumber;

    void Awake()
    {
        GetComponent<Constrain>().centerObject = GameObject.Find("Anakin");
        transform.parent = GameObject.FindGameObjectWithTag("Manager").transform;
        stateMachine = GetComponent<StateMachine>();
        if (leader)
        {
            List<Arc170Controller> allies = new List<Arc170Controller>();
            Arc170Controller[] _temp = GameObject.FindObjectsOfType<Arc170Controller>();

            for (int i = 0; i < _temp.Length; i++)
            {
                if (_temp[i] != this)
                {
                    allies.Add(_temp[i]);
                }
            }

            AssignSquad(allies.ToArray());
        }

        needsHelp = false;
        busy = false;
    }

    void Start()
    {
        CurrentShips.AddAlly(gameObject);
        Invoke("SetBehaviour", 0.1f);

        if (leader)
        {
            stateMachine.ChangeState(new LeadAlliesR());
        }
        else
        {
            stateMachine.ChangeState(new WaitWanderR());
        }
    }

    void Update()
    {
        currentState = stateMachine.currentState.ToString();
    }

    void AssignSquad(Arc170Controller[] allies)
    {

        int num = 1;

        for (int i = 0; i < allies.Length; i++)
        {
            if (i % 2 != 0)
            {
                allies[i].formationNumber = num;
                num++;
            }
            else
            {
                allies[i].formationNumber = -num;
            }
        }
    }

    void OnDestroy()
    {
        CurrentShips.RemoveAlly(gameObject);
    }

    void SetBehaviour()
    {
        stateMachine.CancelDelayedStateChange();

        bool leaderLess = true;

        foreach (GameObject ship in CurrentShips.instance.allyShips)
        {
            if (ship.GetComponent<Arc170Controller>().leader)
            {
                leaderLess = false;
            }
        }

        if (leaderLess && !leader)
        {
            stateMachine.ChangeState(new AllocateStateR());
        }
        else if (!leaderLess && !leader)
        {
            stateMachine.ChangeState(new FormationApproachR());
        }
    }
}