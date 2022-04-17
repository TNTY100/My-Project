using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BoxBoiBehaviour : MonoBehaviour
{

    BehaviourTree mouvementTree;
   

    GameObject player;
    NavMeshAgent agent;
    GameObject[] spawners;

    public GameObject backRoom;
    public enum ActionState { IDLE, WORKING };
    ActionState state = ActionState.IDLE;

    public enum LivingState { LIVING, DEAD };
    LivingState livingState = LivingState.LIVING;
    public float respawnTime = 25f;
    private float respawnMoment = 25f;


    Node.Status treeStatus = Node.Status.RUNNING;

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

        mouvementTree = new BehaviourTree();

        Branch inBranch = new Branch("Initial Branch"); 

        Leaf isActive = new Leaf("Is Agent Alive", IsAlive);
        Sequence chassePlayer = new Sequence("Chases player");

        Leaf goToPlayer = new Leaf("Go To player", GoToPlayer);
        Leaf circlePlayer = new Leaf("Circleing player", CirclePlayer);

        

        //tree.AddChild(steal);

        chassePlayer.AddChild(goToPlayer);
        chassePlayer.AddChild(circlePlayer);

        inBranch.AddChild(isActive);
        inBranch.AddChild(chassePlayer);

        mouvementTree.AddChild(inBranch);




        mouvementTree.PrintTree();

    }

    public Node.Status GoToPlayer()
    {
        return GoToLocation(player.transform.position, 4);
    }


    public Node.Status CirclePlayer()
    {
        Debug.Log("Want to Circle player");
        int offset = 5;
        Vector3 pointOffset = Vector3.forward * Random.Range(-offset, offset) + Vector3.left * Random.Range(-offset, offset);
        Vector3 goalPoint = player.transform.position + pointOffset;

        Node.Status status = GoToLocation(goalPoint, 1);

        return status;
    }
    public Node.Status IsAlive()
    {
        
        if(pv > 0)
        {
            livingState = LivingState.LIVING;
            return Node.Status.SUCCESS;
        }

        if (livingState == LivingState.LIVING) 
        {
            agent.ResetPath();
            agent.Warp(backRoom.transform.position);
            livingState = LivingState.DEAD;
            respawnMoment = Time.time + respawnTime;
        }
        return Node.Status.FAILURE;
    }


    public Node.Status GoToLocation(Vector3 destination, int precision)
    {
        float distanceToTarget = Vector3.Distance(destination, this.transform.position);
        //Debug.Log("Distance To Target: " + distanceToTarget.ToString() + " ; Distance path End position: " + Vector3.Distance(agent.pathEndPosition, destination).ToString());

        if (state == ActionState.IDLE || Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {
            Debug.Log("Looking for location");
            agent.SetDestination(destination);
            state = ActionState.WORKING;

            if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
            {
                Debug.Log("No path");
                return Node.Status.FAILURE;
                
            }

        }
        else if (distanceToTarget < precision)
        {
            Debug.Log("Location Success");
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }
        return Node.Status.RUNNING;
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
            livingState = LivingState.LIVING;
        }
    }


    // Update is called once per frame
    void Update()
    {
        AnimatorController();
        if (livingState == LivingState.DEAD)
        {
            RespawnCountdown();
            return;
        }
        if (treeStatus != Node.Status.SUCCESS)
            treeStatus = mouvementTree.Process();
    }
}
