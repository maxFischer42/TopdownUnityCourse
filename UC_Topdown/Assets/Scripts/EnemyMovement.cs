using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public enum enemyState {idle, move, chase}
    private enemyState state = enemyState.idle;

    private Animator animator;
    private SpriteRenderer render;
    public Vector2 rangeBetweenActions = new Vector2(5f, 8f);
    private float timeToNextAction = 0f;

    public float walkSpeed = 1f;
    public float chaseSpeed = 1.5f;

    public Transform patrolPointParent;
    private List<Transform> patrolPoints = new List<Transform>();
    private int currentWaypoint = 0;

    public NavMeshAgent agent;

    // An effect to spawn when the player is detected by the enemy
    public GameObject notification;

    // If the enemy is currently thinking, don't do anything
    private bool isThinking = false;

    // Used to check if we should flip the sprite
    private float previousDirection = 1;

    public float distanceToSeePlayer = 10f;

    public bool isChasingPlayer = false;

    public LayerMask blockingLayers;

    bool playerDetected = false;

    public float currentSpeed;
    public float distanceToStop = 1.5f;

    public bool isIdling = false;
    public NavMeshPathStatus status;

    void Start() {
        animator = transform.GetChild(0).GetComponent<Animator>();
        render = transform.GetChild(0).GetComponent<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
        GetPatrolPoints();
        state = enemyState.move;
    }

    void GetPatrolPoints() {
        int nodeCount = patrolPointParent.childCount;
        for(int i = 0; i < nodeCount; i++) {
            Debug.Log("Added point " + i);
            patrolPoints.Add(patrolPointParent.GetChild(i).transform);
        }
    }

    void Update() {
        float dist = agent.remainingDistance;
        FlipSprite(Mathf.Sign(agent.velocity.x));        
        
        switch(state) {
            case enemyState.idle:
                if(isIdling) return;
                timeToNextAction = Random.Range(rangeBetweenActions.x, rangeBetweenActions.y);
                isIdling = true;
                Invoke("IdleWait", timeToNextAction);
                break;
            case enemyState.move:       
                agent.speed = walkSpeed;
                if(dist < agent.stoppingDistance) {
                    animator.SetBool("isMoving", false);
                    state = enemyState.idle;  
                }
                break;
            case enemyState.chase:
                if(!isChasingPlayer) {
                    state = enemyState.idle;
                } else {
                    isIdling = false;
                    agent.speed = chaseSpeed; 
                    if(dist < agent.stoppingDistance) {
                        ChasePlayer();
                    }
                }
                break;
        }
    }

    void FixedUpdate() {
        CheckForPlayer();
    }

    void CheckForPlayer() {
        Transform playerTransform = GameObject.Find("Player").transform;
        if(playerTransform.GetComponent<PlayerCombat>().isSlime) {
            isChasingPlayer = false;
            return;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerTransform.position - transform.position, distanceToSeePlayer, blockingLayers);
        Debug.DrawLine(transform.position, hit.point);
        if(hit.collider) {
            if(hit.collider.tag == "Player") {
                playerDetected = true;
            } else {
                playerDetected = false;
                isChasingPlayer = false;
                return;
            }
        } else {
            return;
            
        }
        if(playerDetected) {
            PlayerDetected();
            state = enemyState.chase;            
        } else {
            playerDetected = false;
            isChasingPlayer = false;
            return;
        }
        ChasePlayer();
    }

    void ChasePlayer() {
        Transform playerTransform = GameObject.Find("Player").transform;
        agent.SetDestination(playerTransform.position);
    }

    void PlayerDetected() {
        if(isChasingPlayer && playerDetected) return;
        GameObject n = Instantiate(notification, transform.position + (Vector3.up), Quaternion.identity);
        isChasingPlayer = true;
        Destroy(n, 0.7f);
    }

    void IdleWait() {
        if(state != enemyState.chase) {
            state = enemyState.move;
            isIdling = false;
            animator.SetBool("isMoving", true);
            while(true) {
                int newPoint = Random.Range(0, patrolPoints.Count - 1);
                if(newPoint != currentWaypoint) {
                    agent.SetDestination(patrolPoints[newPoint].position);
                    currentWaypoint = newPoint;
                    break;
                }
            }
        }
    }

    void FlipSprite(float direction) {
        if(previousDirection != direction) {
            previousDirection = direction;
            animator.GetComponent<SpriteRenderer>().flipX = !animator.GetComponent<SpriteRenderer>().flipX;
        }
    } 
}

