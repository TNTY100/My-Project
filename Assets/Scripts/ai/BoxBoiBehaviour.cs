using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// Testing the gitHub thing
/// </summary>

public class BoxBoiBehaviour : MonoBehaviour
{

    BehaviourTree mouvementTree;

    public int player_go_to_location_precision = 4;

    GameObject player;
    NavMeshAgent agent;
    GameObject[] spawners;

    public GameObject backRoom;
    public enum ActionState { IDLE, WORKING };
    ActionState state = ActionState.IDLE;

    public enum ProximityState { FAR, NEAR, DEAD, UNKNOWN};
    ProximityState proximity_state = ProximityState.NEAR;


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

        if (state == ActionState.IDLE)
        {
            RaycastHit hit;
            int layerMask = 1 << 8;
            float distance = 1.5f;

            float randomX = Random.Range(-1, 1);
            float randomZ = Random.Range(-1, 1);
            Debug.Log("RandomX:" + randomX.ToString() + "; RandomZ:" + randomZ.ToString());
            Vector3 vector = new Vector3(randomX, 0, randomZ);

            Ray ray = new Ray(player.transform.position, player.transform.position + vector);

            circle_point = ray.origin + (ray.direction * distance);
            Debug.Log("World point " + player.transform.position);
            Debug.Log("World point " + circle_point);
            // Debug.DrawLine(Camera.main.transform.position, point, Color.red);
            /*
            Vector3 wantedPosition = Physics.Raycast(player.transform.position, transform.TransformDirection(Vector3.forward), out hit, 100, layerMask);

            Debug.Log("Want to Circle player");
            int offset = 5;
            Vector3 pointOffset = Vector3.forward * Random.Range(-offset, offset) + Vector3.left * Random.Range(-offset, offset);
            Vector3 goalPoint = player.transform.position + pointOffset;
            */
            
        }

        ProximityState status = GoToLocation(circle_point, 2, "Circling");
        /*
        if (status == Node.Status.SUCCESS)
        {
            Debug.Log("Player circled Successfully");
        }
        else if (status == Node.Status.FAILURE)
        {
            Debug.Log("Player circled Faill");
        }
        else
        {
            Debug.Log("Player is Circleing");
        }
        */
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

        ProximityState try_going_to_player = GoToPlayer();

        if (try_going_to_player == ProximityState.NEAR)
        {
            CirclePlayer();
        }

    }
}
