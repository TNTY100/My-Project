using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    GameObject[] agents;

    // Start is called before the first frame update
    void Start()
    {
        //finds all of the AIs 
        agents = GameObject.FindGameObjectsWithTag("ai");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject a in agents)
        {
            a.GetComponent<AIControl>().agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
        }

    }
}
