using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    public enum enemyState {idle, move, chase, attack}
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

    public Rigidbody2D rb;

    public EnemyPathfinding pathing;

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

    private int subPixelMeasurements = 16;

    public float currentSpeed;

    void Start() {
        animator = transform.GetChild(0).GetComponent<Animator>();
        render = transform.GetChild(0).GetComponent<SpriteRenderer>();
        pathing = GetComponent<EnemyPathfinding>();
        GetPatrolPoints();
        HandleNextAction(); 
    }

    void GetPatrolPoints() {
        patrolPointParent = GameObject.Find("Patrol Points").transform;
        int nodeCount = patrolPointParent.childCount;
        for(int i = 0; i < nodeCount; i++) {
            Debug.Log("Added point " + i);
            patrolPoints.Add(patrolPointParent.GetChild(i).transform);
        }
    }

    void Update() {
        RockEnemyUpdate();
    }

    void RockEnemyUpdate() {
        if(isThinking) return;
        CheckForPlayer();
        switch(state) {
            case enemyState.idle:

                break;
            case enemyState.move:       
                currentSpeed = walkSpeed;         
                if(pathing.reachedDestination) {
                    animator.SetBool("isMoving", false);
                    isThinking = true;
                    TransitionToNextAction();
                }
                break;
            case enemyState.chase:
                if(!isChasingPlayer) {
                    isThinking = true;
                    TransitionToNextAction(); 
                    return;
                }
                currentSpeed = chaseSpeed; 
                break;
        }
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
        pathing.SetDestination(playerTransform);
    }

    void PlayerDetected() {
        if(isChasingPlayer && playerDetected) return;
        GameObject n = Instantiate(notification, transform.position + (Vector3.up), Quaternion.identity);
        isChasingPlayer = true;
        Destroy(n, 0.7f);
    }

    void TransitionToNextAction() {
        timeToNextAction = Random.Range(rangeBetweenActions.x, rangeBetweenActions.y);
        Invoke("HandleNextAction", timeToNextAction);
    }

    void HandleNextAction() {
        isThinking = false;
        switch(state) {
            case enemyState.idle:
                animator.SetBool("isMoving", true);
                while(true) {
                    int newPoint = Random.Range(0, patrolPoints.Count - 1);
                    if(newPoint != currentWaypoint) {
                        pathing.SetDestination(patrolPoints[newPoint].position);
                        currentWaypoint = newPoint;
                        break;
                    }
                }
                state = enemyState.move;
                break;
            case enemyState.move:
                state = enemyState.idle;
                TransitionToNextAction();
                break;
            case enemyState.chase:
                TransitionToNextAction();
                state = enemyState.idle;
                break;
        }
    }

    public void Move(Vector3 velocity) {
       // rb.velocity = velocity * Time.fixedDeltaTime;
        transform.position += velocity * Time.deltaTime;
        FlipSprite(Mathf.Sign(velocity.x));
        previousDirection = Mathf.Sign(velocity.x);
    }   

    void FlipSprite(float direction) {
        bool state = false;
        if(direction == 0) return;
        if(direction > 0) state = false;
        else state = true;
        animator.GetComponent<SpriteRenderer>().flipX = state;
    } 
}

