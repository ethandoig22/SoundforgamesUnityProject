using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AntManager : MonoBehaviour
{
    
        public Transform HivePos;
        public int FoodCount;
        public GameObject agent, antparent, pathParent;
        public int numOfAgents;
        public ProceduralTreeManager proceduralTreeManager;

                

        void Start()
        {
            int count = 0;
            for (int i = 0; i < numOfAgents; i++)
            {
                GameObject agentInstance = Instantiate(agent);
                count++;
                agentInstance.transform.position = new Vector3(HivePos.position.x + Random.Range(0, 5), HivePos.position.y, HivePos.position.z + Random.Range(0, 5));
                agentInstance.GetComponent<AntBehaviour>().Index = count;
                agentInstance.name = count.ToString();
            agentInstance.transform.parent = transform;
            }

            for (int i = 1; i < numOfAgents / 4; i++)
            {
                GameObject antselected = antparent.transform.GetChild(Random.Range(1, antparent.transform.childCount)).gameObject;

                antselected.GetComponent<AntBehaviour>().wanderRadius += (agent.GetComponent<AntBehaviour>().wanderRadius) * 10f;

            }
        }




}
