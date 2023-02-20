using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Owens_TestEnemy : MonoBehaviour
{
    private Owens_StateMachine brain;
    // We don't have these yet but when we do, obv copy this file and enable it.
    //private Animator anim;
    private GameObject player;

    // Distances for chasing and murdering, not implemented yet.
    public int distanceToChase;
    public int distanceToMurder;

    // Bools for States later.
    private bool isWithinMurderRange;
    private bool isPlayerNear;

    // Navmesh stuff for movement.
    private NavMeshAgent nav;
    [SerializeField]private float idleTimer;

    // Debug state identifier for when we are testing.
    [SerializeField] private UnityEngine.UI.Text stateNote;


    // Finds all the stuff we don't want to input ourselves.
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        //anim = transform.GetChild(0).GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        brain = GetComponent<Owens_StateMachine>();

        isPlayerNear = false;
        distanceToChase = 5;
        isWithinMurderRange = false;
        distanceToMurder = 1;

        // Initialize State Machine

        brain.PushState(Idle, IdleEnter, IdleExit);
    }

    // Update is called once per frame, but also we need a player for this stuff.
    void Update()
    {
        isPlayerNear = Vector3.Distance(transform.position, player.transform.position) < distanceToChase;
        isWithinMurderRange = Vector3.Distance(transform.position, player.transform.position) < distanceToMurder;
    }

    void IdleEnter() //Clears any movement data and readies for a new state.
    {
        stateNote.text = "Idle";
        nav.ResetPath();
    }

    void Idle() // If the player is close enough, start chasing. Otherwise, wait a bit and then move to a random spot.
    {
        if (isPlayerNear)
        {
            brain.PushState(Chase, ChaseEnter, ChaseExit);
        }

        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0)
        {
            brain.PushState(Wander, WanderEnter, WanderExit);
            idleTimer = Random.Range(1, 3);
        }
    }

    void IdleExit() // Empty function for State Machine. 
    {    
        
    }

    void ChaseEnter() // Prepares our enemy to actually chase the player.
    {
        stateNote.text = "Chase";
        //anim.SetBool("Chase",true);
    }

    void Chase() // Actually chases the player unless they get too far away.
    {
        nav.SetDestination(player.transform.position); //when you are in chase move towards the player's position
        //if the player moves too far from you go back into idle 
        if (Vector3.Distance(transform.position, player.transform.position) > distanceToChase + 0.5)
        {
            brain.PushState(Idle, IdleEnter, IdleExit);
        }
    }

    void ChaseExit() // Welp, looks like they either got away or I added a state to attack... Take your pick really.
    {
        //anim.SetBool("Chase", false);
    }

    void WanderEnter() // Generates a random position to move to, and readies everything to do so.
    {
        stateNote.text = "Wander";
        //anim.SetBool("Chase", true);
        float distanceFromCurrentPosition = 10f;
        Vector3 wanderDirection = (Random.insideUnitSphere * distanceFromCurrentPosition) + transform.position; //current position of enemy offset by unitspehre calc 
        //we need to make sure it will use the navmesh
        //similiar to physics raycast
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(wanderDirection, out navMeshHit, 3f, NavMesh.AllAreas);
        Vector3 destination = navMeshHit.position;  //Target point.
        nav.SetDestination(destination);
    }

    void Wander() // Handles the moving between Idle and Chase states, since those are really all we will be going to in this demo.
    {
        if (nav.remainingDistance <= 0.25f) //we're close to the destination
        {
            nav.ResetPath();
            brain.PushState(Idle, IdleEnter, IdleExit);
        }

        if (isPlayerNear)
        {
            brain.PushState(Chase, ChaseEnter, ChaseExit);
        }
    }

    void WanderExit() // Leaves the Wander state, and turns off animation flag that totally exists right now... Totally.
    {
        //anim.SetBool("Chase",false);
    }
}
