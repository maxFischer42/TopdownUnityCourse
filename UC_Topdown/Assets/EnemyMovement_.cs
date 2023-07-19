using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement_ : MonoBehaviour
{
    public NavMeshAgent agent;

    public GameObject[] patrolPoints;

    public int currentWaypoint;

    public float distanceToSeePlayer = 4f;
    public Transform playerTransform;
    public bool isChasingPlayer = false;
    public LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;

		agent.updateRotation = false;
		agent.updateUpAxis = false;

        patrolPoints = GameObject.FindGameObjectsWithTag("patrol");

        currentWaypoint = Random.Range(0, patrolPoints.Length - 1);

        agent.SetDestination(patrolPoints[currentWaypoint].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        float dist = agent.remainingDistance;

        isChasingPlayer = IsPlayerVisible();

        if(isChasingPlayer == true) {
            agent.SetDestination(playerTransform.position);
        } else {
            if(dist <= agent.stoppingDistance) {

                int newWaypoint = currentWaypoint;
                while(currentWaypoint == newWaypoint) {
                    newWaypoint = Random.Range(0, patrolPoints.Length - 1);
                }
            
                currentWaypoint = newWaypoint;
                agent.SetDestination(patrolPoints[currentWaypoint].transform.position);
            }
        }
    }

    bool IsPlayerVisible() {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distanceToSeePlayer, layerMask);    
        if(hit.collider) {
            if(hit.collider.gameObject.tag == "Player") 
            {
                return true;
            }
        }
        return false;
    }

}
