using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBehaviourStraf : MonoBehaviour
{

    private float waitTime;
    public float startWaitTime = 1f;

    NavMeshAgent nav;

    // Strafe 
    public float distToPlayer = 5.0f;

    public float randomStrafeStartTime;
    public float waitStrafeTime;
    public float t_minStrafe;
    public float t_maxStrafe;

    public Transform strafeRight;
    public Transform strafeLeft;
    private int randomStrafeDir; 

    // Chase 
    public float facePlayerFactor = 20f;

    // Player
    private Vector3 playerPos;
    private GameObject player;


    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        nav.enabled = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        float distance = Vector3.Distance(playerPos, transform.position);

        ChasePlayer();

        FacePlayer();
    }

    void ChasePlayer()
    {
        float distance = Vector3.Distance(playerPos, transform.position);

        if (distance > distToPlayer)
        {
            nav.SetDestination(playerPos);
        }

        else if (nav.isActiveAndEnabled && distance <= distToPlayer)
        {
            randomStrafeDir = Random.Range(0, 2);
            randomStrafeStartTime = Random.Range(t_minStrafe, t_maxStrafe);

            if (waitStrafeTime <= 0)
            {
                if (randomStrafeDir == 1)
                {
                    nav.SetDestination(strafeLeft.position);
                }
                else if (randomStrafeDir == 1)
                {
                    nav.SetDestination(strafeRight.position);
                }
                waitStrafeTime = randomStrafeStartTime;

            }
            else
            {
                waitStrafeTime -= Time.deltaTime;
            }
        }
    }

    void FacePlayer()
    {
        Vector3 direction = (playerPos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * facePlayerFactor);
    }
    /*
    private void LateUpdate()
    {
        if (aiMemorizeesPlayer == true && playerIsInLOS == false)
        {
            distToPlayerToPlayer = 0.5f;
        }
        else
        {
            distToPlayer = 10f;
        }
    }*/
}
