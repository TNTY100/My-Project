using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BoxBoiBehaviour : MonoBehaviour
{

    BehaviourTree mouvementTree;


    public int player_go_to_location_precision = 6;

    // attack
    public int attackRange = 8;
    public LayerMask what_is_player;
    private bool alreadyAttacked = false;
    private float timeBetweenAttacks = 0.5f;
    public GameObject projectile;
    private bool player_in_attack_range;

    // GameObjects
    GameObject player;
    NavMeshAgent agent;
    GameObject[] spawners;


    // Mouvement
    public GameObject backRoom;
    public enum ActionState { IDLE, WORKING };
    ActionState state = ActionState.IDLE;

    public enum ProximityState { FAR, NEAR, DEAD, UNKNOWN };
    ProximityState proximity_state = ProximityState.FAR;

    private bool is_cicling = false;

    // Dead
    public float respawnTime = 25f;
    private float respawnMoment = 25f;

    public Animator animator;

    [Range(0, 100)]
    public int maxPv = 100;
    public int pv = 100;

    // https://learn.unity.com/tutorial/nodes-jxw?uv=2020.2&projectId=60645258edbc2a001f5585aa#60645813edbc2a001f55863a
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        backRoom = GameObject.FindGameObjectWithTag("BackRoom");
        spawners = GameObject.FindGameObjectsWithTag("Spawners");

    }

    public ProximityState GoToPlayer()
    {
        Vector3 playerPosition = player.transform.position;

        if (Vector3.Distance(playerPosition, this.transform.position) <= player_go_to_location_precision + 1)
        {
            Debug.Log("gotoplayerSucces");
            return ProximityState.NEAR;
        }

        return GoToLocation(player.transform.position, player_go_to_location_precision, "Going To Player");
    }
    private Vector3 circle_point;

    public ProximityState CirclePlayer()
    {

        Debug.Log("Want to Circle player");

        if (is_cicling)
        {
            is_cicling = true;
            RaycastHit hit;
            int layerMask = 1 << 8;
            float distance = 1.5f;
            int distance_multiplyer = 4;
            float randomX = Random.Range(-1, 1);
            float randomZ = Random.Range(-1, 1);
            Debug.Log("RandomX:" + randomX.ToString() + "; RandomZ:" + randomZ.ToString());
            Vector3 vector = new Vector3(randomX * distance_multiplyer, 0, randomZ * distance_multiplyer);

            Ray ray = new Ray(player.transform.position, player.transform.position + vector);

            circle_point = ray.origin + (ray.direction * distance);
            Debug.Log("World point " + player.transform.position);
            Debug.Log("World point " + circle_point);
            Debug.DrawLine(player.transform.position, circle_point, Color.red);
            
        }

        ProximityState status = GoToLocation(circle_point, 1, "Circling");

        if (status == ProximityState.NEAR)
        {
            is_cicling = false;
        }

        return status;
    }


    public ProximityState GoToLocation(Vector3 destination, int precision, string destinationName = "")
    {
        float distanceToTarget = Vector3.Distance(destination, this.transform.position);
        //Debug.Log("Distance To Target: " + distanceToTarget.ToString() + " ; Distance path End position: " + Vector3.Distance(agent.pathEndPosition, destination).ToString());

        if (state == ActionState.IDLE || Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {
            Debug.Log("Looking for " + destinationName);
            agent.SetDestination(destination);
            state = ActionState.WORKING;

            if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
            {
                Debug.Log("No path" + destinationName);
                state = ActionState.IDLE;
                return ProximityState.UNKNOWN;
                
            }

        }
        else if (distanceToTarget < precision)
        {
            Debug.Log("Location Success" + destinationName);
            state = ActionState.IDLE;
            proximity_state = ProximityState.NEAR;
        }
        return ProximityState.FAR;
    }

    public void AnimatorController() 
    {
        if (state == ActionState.WORKING)
            animator.SetBool("is walking", true);

        else
            animator.SetBool("is walking", false);
    }

    public void RespawnCountdown()
    {
        Debug.Log("Check Respawned");
        if (Time.time >= respawnMoment)
        {
            Debug.Log("Respawned");
            pv = maxPv;
            agent.Warp(spawners[Random.Range(0, spawners.Length)].transform.position);
            proximity_state = ProximityState.FAR;
        }
    }


    // Update is called once per frame
    void Update()
    {
        AnimatorController();
        if (proximity_state == ProximityState.DEAD)
        {
            RespawnCountdown();
            return;
        }

        // attack
        player_in_attack_range = Physics.CheckSphere(transform.position, attackRange, what_is_player);

        if (player_in_attack_range) AttackPlayer();

        ProximityState try_going_to_player = GoToPlayer();

        if (try_going_to_player == ProximityState.NEAR)
        {
            CirclePlayer();
        }

    }

    private void AttackPlayer()
    {

        // problem with looking at the player
        // https://youtu.be/UjkSFoLxesw
        transform.rotation.SetLookRotation(player.transform.position);

        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);


            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
