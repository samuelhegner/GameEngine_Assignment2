using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bar from the specialised states, the vulture droid and the arc 170 have the same state machine
//Only the Arc 170 controller is commented fully. The vulture controller is commented only where it differs


// Leader opening state
class LeadAlliesR : State
{
    Pursue pursue;
    Arc170Controller controller;

    //turns on required behaviours and gets the ships controller
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

        //assigns the pursue target to the controllers chase target
        if (controller.enemyToChase != null)
        {
            pursue.target = controller.enemyToChase;
        }

        //checks if there are enemy ships
        if (CurrentShips.enemyNumber > 0)
        {
            //chases the first enemy ship
            controller.enemyToChase = CurrentShips.instance.enemyShips[0].GetComponent<Boid>();
        }

        //checks if the enemy to chase is still allive
        if (controller.enemyToChase != null)
        {
            //checks the distance to the target and if it is in range
            if (Vector3.Distance(owner.transform.position, controller.enemyToChase.transform.position) < 300f)
            {
                //if the enemy ship is in range tell all other allies to allocate their first state
                foreach (GameObject obj in CurrentShips.instance.allyShips)
                {
                    obj.GetComponent<StateMachine>().ChangeStateDelayed(new AllocateStateR(), Random.Range(0, 1f));
                }
            }
        }

    }
    
    //turns off behaviours
    public override void Exit()
    {
        controller.leader = false;
        pursue.enabled = false;

        owner.GetComponent<Constrain>().centerObject = GameObject.Find("Anakin");

        owner.GetComponent<Constrain>().radius = CurrentShips.instance.constrainDistance;
        owner.GetComponent<Constrain>().enabled = true;
    }
}

//follower opening state
class FormationApproachR : State
{

    OffsetPursue offsetPursue;
    Arc170Controller controller;
    
    //turns on required behaviours and gets the ships controller
    public override void Enter()
    {
        offsetPursue = owner.GetComponent<OffsetPursue>();
        controller = owner.GetComponent<Arc170Controller>();
        controller.busy = false;


        //gets all allies in the scene
        Arc170Controller[] allies = GameObject.FindObjectsOfType<Arc170Controller>();

        //assigns all followers their leader
        foreach (Arc170Controller ally in allies)
        {
            if (ally.leader)
            {
                offsetPursue.leader = ally.GetComponent<Boid>();
            }
        }

        //creates the ships offset in relation to the leader based on previously assigned values
        offsetPursue.offset = new Vector3((offsetPursue.sideDistance * controller.formationNumber)
            , -(Mathf.Abs(offsetPursue.bottomDistance * controller.formationNumber))
            , 0f);

        offsetPursue.enabled = true;

        //increases the ships max speed to allow ships to get in formation quickly
        owner.GetComponent<Boid>().maxSpeed = owner.GetComponent<Boid>().maxSpeed + 20f;
    }

    public override void Think()
    {
    }

    //turns off required behaviours
    public override void Exit()
    {
        owner.GetComponent<Boid>().maxSpeed = owner.GetComponent<Boid>().maxSpeed - 20f;
        offsetPursue.enabled = false;

        //enables the constrain behaviour on all ally followers
        owner.GetComponent<Constrain>().centerObject = GameObject.Find("Anakin");
        owner.GetComponent<Constrain>().radius = CurrentShips.instance.constrainDistance;

        owner.GetComponent<Constrain>().enabled = true;
    }
}

//State when chasing down an enemy
class ChaseDownR : State
{
    Pursue pursue;

    Arrive arrive;

    Arc170Controller controller;

    //turns on required behaviours and gets the ships controller
    public override void Enter()
    {
        controller = owner.GetComponent<Arc170Controller>();
        controller.busy = true;


        pursue = owner.GetComponent<Pursue>();
        arrive = owner.GetComponent<Arrive>();

        //if the enemy to chase is unassigned it sets state to WaitWander
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

        //increases the speed to allow for easy chase
        owner.GetComponent<Boid>().maxSpeed = owner.GetComponent<Boid>().maxSpeed + 10f;

    }

    public override void Think()
    {
        //checks if the target enemy is dead
        if (controller.enemyToChase == null)
        {
            //if you are still being chased start fleeing
            if (controller.enemyChasing != null)
            {
                owner.ChangeState(new ShakeEnemyR());
            }
            // the enemy chasing doesnt exist or else allocate a new state
            else if (controller.enemyChasing == null)
            {
                owner.ChangeState(new AllocateStateR());
            }
            else
            {
                owner.ChangeState(new AllocateStateR());
            }
        }

        //if the enemy you are chasing is allive check if the distance to the enemy.
        //if the distance is close, arrive at the target to avoid collision
        //if the distance is far, pursue the target to catch up to it
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
    
    //turns off required behaviours
    public override void Exit()
    {
        pursue.enabled = false;
        arrive.enabled = false;
        owner.GetComponent<Arc170Controller>().enemyToChase = null;

        controller.busy = false;
        owner.GetComponent<Boid>().maxSpeed = owner.GetComponent<Boid>().maxSpeed - 10f;
    }
}

//state to help out an ally
class HelpAllyR : State
{
    Pursue pursue;

    Arrive arrive;

    Boid enemy;

    Arc170Controller controller;

    //turns on required behaviours and gets the ships controller
    public override void Enter()
    {
        controller = owner.GetComponent<Arc170Controller>();
        controller.busy = true;

        //assigns the enemy to chase to the enemy chasing the ally that needs help
        pursue = owner.GetComponent<Pursue>();
        arrive = owner.GetComponent<Arrive>();
        enemy = controller.allyNeedsHelp.GetComponent<Arc170Controller>().enemyChasing;
        
        //check if an enemy was assigned and sets up the pursue and arrive behaviours
        //if no enemy is assigned, WaitWander
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
        //check if the ally you are still helping is alive if not, chase down the enemy that killed the ally
        if (controller.allyNeedsHelp == null)
        {
            controller.enemyToChase = enemy;
            owner.ChangeState(new ChaseDownR());
        }
        //check if the enemy you are chasing is still allive, if not, allocate a new state
        else if (enemy == null)
        {
            controller.allyNeedsHelp = null;
            owner.ChangeState(new AllocateStateR());
        }
        //else regularily chase the enemy
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

    //turns off required behaviours
    public override void Exit()
    {
        pursue.enabled = false;
        arrive.enabled = false;
        owner.GetComponent<Arc170Controller>().allyNeedsHelp = null;

        controller.busy = false;

        owner.GetComponent<Boid>().maxSpeed = owner.GetComponent<Boid>().maxSpeed - 10f;
    }
}

//state that flees from an enemy
class ShakeEnemyR : State
{
    NoiseWander[] wanders;
    Arc170Controller controller;

    //turns on required behaviours and gets the ships controller
    public override void Enter()
    {

        //turns on two noise wander, one horizontal and one vertical (creates erratic but realistic looking flying)
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
        //checks if the ship is still being chased, else allocate a new state
        if (controller.enemyChasing == null)
        {
            owner.ChangeState(new AllocateStateR());
        }
    }

    //turns off required behaviours
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

//state to wait 5 seconds if all other ships are busy
class WaitWanderR : State
{

    NoiseWander[] wanders;
    Arc170Controller controller;

    //turns on required behaviours and gets the ships controller
    public override void Enter()
    {
        //turns on two noise wander, one horizontal and one vertical (creates erratic but realistic looking flying)
        wanders = owner.GetComponents<NoiseWander>();
        foreach (NoiseWander wander in wanders)
        {
            wander.enabled = true;
        }
        controller = owner.GetComponent<Arc170Controller>();
        controller.busy = false;

        //allocates new state in 5 seconds
        owner.ChangeStateDelayed(new AllocateStateR(), 5f);
    }

    public override void Think()
    {

    }


    //turns off required behaviours
    public override void Exit()
    {
        wanders = owner.GetComponents<NoiseWander>();
        foreach (NoiseWander wander in wanders)
        {
            wander.enabled = false;
        }
    }
}

//state that assigns a new state
class AllocateStateR : State
{
    Arc170Controller controller;

    //gets all enemy and ally ships
    List<GameObject> enemies;
    List<GameObject> allies;

    public override void Enter()
    {
        owner.GetComponent<Constrain>().enabled = true;
        controller = owner.GetComponent<Arc170Controller>();
        controller.busy = false;

        //turns off all previous enemy and ally information
        controller.enemyToChase = null;
        controller.allyNeedsHelp = null;
        controller.enemyChasing = null;

        //gets the static ship lists of all active ships
        enemies = CurrentShips.instance.enemyShips;
        allies = CurrentShips.instance.allyShips;

        //gameobject to keep track possible new ships to work with
        GameObject newEnemy = null;
        GameObject newAlly = null;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                //check all enemies to see if one of them isn't busy
                if (!enemies[i].GetComponent<VultureController>().busy)
                {
                    newEnemy = enemies[i];
                }
            }

        }

        for (int i = 0; i < allies.Count; i++)
        {
            //check all allies to see if one of them needs help, isnt being helped already
            if (allies[i].GetComponent<Arc170Controller>().needsHelp && allies[i].GetComponent<Arc170Controller>().enemyChasing != null && allies[i].GetComponent<Arc170Controller>().enemyChasing.GetComponent<VultureController>().enemyChasing == null)
            {
                newAlly = allies[i];
            }
        }

        //check if a possible new enemy has been assigned
        //if so chase that enemy and set the new enemies things up for getting chased
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

        //checks if a possible ally to help has been assigned
        //if so chase the enemy chasing the new ally and sets up both the ally and enemy being helped and chased respectively
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

        //if both the newAlly and newEnemy are null, waitwander and check again in 5 seconds
        else if (newAlly == null && newEnemy == null)
        {
            owner.ChangeState(new WaitWanderR());
        }
    }

    public override void Exit()
    {

    }
}

//the controller of the arc 170 ships
public class Arc170Controller : MonoBehaviour
{
    //the three boid that the ships needs to possibly keep track off
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
        //sets up the constraint center
        GetComponent<Constrain>().centerObject = GameObject.Find("Anakin");
        //childs the ship to the manager object
        transform.parent = GameObject.FindGameObjectWithTag("Manager").transform;
        stateMachine = GetComponent<StateMachine>();

        //if the ship is the leader, make it assign the squads positions
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
        //adds the ship to the game manager list
        CurrentShips.AddAlly(gameObject);

        // invokes Set behaviour in 0.1 seconds
        Invoke("SetBehaviour", 0.1f);

        //if the ship is the leader, lead allies
        //if the ship is a follower, waitWander
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
        //allows the current state to be seen in the inspector
        currentState = stateMachine.currentState.ToString();
    }

    //set followers number to 1, -1, 2, -2 .....
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

    //when ship is destroyed, remove the ship from the currentShip list
    void OnDestroy()
    {
        CurrentShips.RemoveAlly(gameObject);
    }

    //this is used to let spawning ships switch into a useful state if the game is in progress and formation flying is useless
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