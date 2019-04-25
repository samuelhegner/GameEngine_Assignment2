using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ChaseDownV : State
{
    Pursue pursue;

    Arrive arrive;

    VultureController controller;

    public override void Enter()
    {
        controller = owner.GetComponent<VultureController>();
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
            owner.ChangeState(new WaitWanderV());
        }

        owner.GetComponent<Boid>().maxSpeed = owner.GetComponent<Boid>().maxSpeed + 10f;
    }

    public override void Think()
    {
        if (controller.enemyToChase == null)
        {
            if (controller.enemyChasing != null)
            {
                owner.ChangeState(new ShakeEnemyV());
            }
            else if (controller.enemyChasing == null)
            {
                owner.ChangeState(new AllocateStateV());
            }
            else {
                owner.ChangeState(new AllocateStateV());
            }
        }

        if (controller.enemyToChase != null)
        {
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
        owner.GetComponent<VultureController>().enemyToChase = null;

        controller.busy = false;
        owner.GetComponent<Boid>().maxSpeed = owner.GetComponent<Boid>().maxSpeed - 10f;
    }
}

class ApproachEnemyV : State
{
    Seek seek;
    VultureController controller;

    public override void Enter()
    {
        controller = owner.GetComponent<VultureController>();
        controller.busy = false;
        seek = owner.GetComponent<Seek>();

        int ran = Random.Range(0, CurrentShips.enemyNumber);

        seek.targetGameObject = CurrentShips.instance.allyShips[ran];
        seek.enabled = true;
    }

    public override void Think()
    {
        if (seek.targetGameObject == null)
        {
            owner.ChangeState(new AllocateStateV());
        }
        else if (Vector3.Distance(owner.transform.position, seek.targetGameObject.transform.position) < 500f)
        {
            owner.ChangeState(new AllocateStateV());
        }
    }

    public override void Exit()
    {
        seek.enabled = false;

        owner.GetComponent<Constrain>().centerObject = GameObject.Find("Anakin");
        owner.GetComponent<Constrain>().radius = CurrentShips.instance.constrainDistance;

        owner.GetComponent<Constrain>().enabled = true;
    }
}

class HelpAllyV : State
{
    Pursue pursue;

    Arrive arrive;

    Boid enemy;

    VultureController controller;



    public override void Enter()
    {
        controller = owner.GetComponent<VultureController>();
        controller.busy = true;

        pursue = owner.GetComponent<Pursue>();
        arrive = owner.GetComponent<Arrive>();
        enemy = controller.allyNeedsHelp.GetComponent<VultureController>().enemyChasing;

        if (enemy != null)
        {
            pursue.target = enemy;
            arrive.targetGameObject = enemy.gameObject;
            pursue.enabled = true;
        }
        else
        {
            owner.ChangeState(new WaitWanderV());
        }

        owner.GetComponent<Boid>().maxSpeed = owner.GetComponent<Boid>().maxSpeed + 10f;
    }

    public override void Think()
    {
        if (controller.allyNeedsHelp == null)
        {
            controller.enemyToChase = enemy;
            owner.ChangeState(new ChaseDownV());
        }
        else if (enemy == null)
        {
            controller.allyNeedsHelp = null;
            owner.ChangeState(new AllocateStateV());
        }
        else {
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
        owner.GetComponent<VultureController>().allyNeedsHelp = null;

        controller.busy = false;

        owner.GetComponent<Boid>().maxSpeed = owner.GetComponent<Boid>().maxSpeed - 10f;
    }
}

class ShakeEnemyV : State
{
    NoiseWander[] wanders;
    VultureController controller;

    public override void Enter()
    {
        wanders = owner.GetComponents<NoiseWander>();
        foreach (NoiseWander wander in wanders)
        {
            wander.enabled = true;
        }
        controller = owner.GetComponent<VultureController>();
        controller.busy = true;
        controller.needsHelp = true;
    }

    public override void Think()
    {
        if (controller.enemyChasing == null)
        {
            owner.ChangeState(new AllocateStateV());
        }
    }

    public override void Exit()
    {
        wanders = owner.GetComponents<NoiseWander>();
        foreach (NoiseWander wander in wanders)
        {
            wander.enabled = false;
        }
        controller.busy = false;
        controller.needsHelp = false;
    }
}

class AllocateStateV : State
{

    VultureController controller;

    List<GameObject> enemies;
    List<GameObject> allies;



    public override void Enter()
    {
        controller = owner.GetComponent<VultureController>();
        controller.busy = false;

        controller.enemyToChase = null;
        controller.allyNeedsHelp = null;
        controller.enemyChasing = null;

        enemies = CurrentShips.instance.allyShips;
        allies = CurrentShips.instance.enemyShips;


        GameObject newEnemy = null;
        GameObject newAlly = null;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                if (!enemies[i].GetComponent<Arc170Controller>().busy)
                {
                    newEnemy = enemies[i];
                }
            }
        }

        for (int i = 0; i < allies.Count; i++)
        {
            if (allies[i].GetComponent<VultureController>().needsHelp && allies[i].GetComponent<VultureController>().enemyChasing != null && allies[i].GetComponent<VultureController>().enemyChasing.GetComponent<Arc170Controller>().enemyChasing == null)
            {
                newAlly = allies[i];
            }
        }

        if (newEnemy != null)
        {
            controller.enemyToChase = newEnemy.GetComponent<Boid>();

            newEnemy.GetComponent<Arc170Controller>().enemyChasing = owner.GetComponent<Boid>();
            newEnemy.GetComponent<Arc170Controller>().enemyToChase = null;
            newEnemy.GetComponent<Arc170Controller>().allyNeedsHelp = null;
            newEnemy.GetComponent<StateMachine>().ChangeState(new ShakeEnemyR());
            newEnemy.GetComponent<StateMachine>().CancelDelayedStateChange();


            owner.ChangeState(new ChaseDownV());
        }
        else if (newAlly != null)
        {
            controller.allyNeedsHelp = newAlly.GetComponent<Boid>();

            controller.allyNeedsHelp.GetComponent<VultureController>().allyNeedsHelp = null;
            controller.allyNeedsHelp.GetComponent<VultureController>().enemyToChase = null;

            controller.enemyToChase = controller.allyNeedsHelp.GetComponent<VultureController>().enemyChasing;
            controller.enemyToChase.GetComponent<Arc170Controller>().enemyChasing = owner.GetComponent<Boid>();
            controller.enemyToChase.GetComponent<Arc170Controller>().allyNeedsHelp = null;
            owner.ChangeState(new HelpAllyV());
        }
        else if (newAlly == null && newEnemy == null)
        {
            owner.ChangeState(new WaitWanderV());
        }

    }

    public override void Exit()
    {

    }
}

class WaitWanderV : State
{

    NoiseWander[] wanders;
    VultureController controller;

    public override void Enter()
    {
        wanders = owner.GetComponents<NoiseWander>();
        foreach (NoiseWander wander in wanders)
        {
            wander.enabled = true;
        }
        controller = owner.GetComponent<VultureController>();
        controller.busy = false;
        owner.ChangeStateDelayed(new AllocateStateV(), 2f);
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

public class VultureController : MonoBehaviour
{

    public Boid allyNeedsHelp;
    public Boid enemyToChase;
    public Boid enemyChasing;

    StateMachine stateMachine;


    public bool needsHelp;

    public bool busy;

    public float pursueDistance;

    public string currentState;



    void Awake()
    {
        GetComponent<Constrain>().centerObject = GameObject.Find("Anakin");
        CurrentShips.AddEnemy(gameObject);
        transform.parent = GameObject.FindGameObjectWithTag("Manager").transform;

        stateMachine = GetComponent<StateMachine>();
        needsHelp = false;
        busy = false;
    }

    void Start()
    {
        stateMachine.ChangeState(new ApproachEnemyV());
    }

    void Update()
    {
        currentState = stateMachine.currentState.ToString();
    }

    void OnDestroy()
    {
        CurrentShips.RemoveEnemy(gameObject);
    }
}