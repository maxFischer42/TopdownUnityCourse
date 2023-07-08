using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyMovement : MonoBehaviour
{
    // A reference to our enemy range script
    public BasicEnemyRange RangeObject;

    // An array of Transforms to patrol between
    public Transform[] patrolPoints;

    // Our current patrol point;
    private int currentPatrolPoint;

    // Tells whether or not the enemy will navigate between the patrol points
    // in a predictable order, or do them randomly.
    public bool randomizePatrolPoints;

    public float patrolSpeed = 2f;
    public float chaseSpeed = 2f;

    private Animator animator;
    private SpriteRenderer render;

    // An effect to spawn when the player is detected by the enemy
    public GameObject notification;
    
    // A buffer distance so the enemy doesn't get too close to their waypoint
    private float distanceToWaypoint = 0.25f;

    // A check to see if the player was in range a second before
    private bool wasChasing = false;

    private int subPixelMeasurements = 16;

    public float previousXDirection = 0;

    void Start() {
        animator = transform.GetChild(0).GetComponent<Animator>();
        render = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isMoving", true);
        bool isInRange = RangeObject.targetIsInRange;
        if(isInRange) {
           // If the target object (the player) is in range of the enemy, do this

            // If the player was detected on this frame, display the ! above the enemy
           if(wasChasing == false) {
                GameObject n = Instantiate(notification, transform.position + (Vector3.up), Quaternion.identity);
                n.transform.parent = transform;
                Destroy(n, 0.7f);
                wasChasing = true;
           }

            MoveTowardsPoint(RangeObject.target, chaseSpeed);

        } else {
            // Otherwise, do this.
            
            // Get the distance between the object and it's target
            float distanceBetweenPoints = (transform.position - patrolPoints[currentPatrolPoint].transform.position).magnitude;
            // If they're close enough, move on to the next waypoint
            if(Mathf.Abs(distanceBetweenPoints) <= distanceToWaypoint) {
                currentPatrolPoint = GetNextWaypoint();
            }
            MoveTowardsPoint(patrolPoints[currentPatrolPoint], patrolSpeed);
            wasChasing = false;
        }
    }

    public void MoveTowardsPoint(Transform target, float speed) {

            // The direction between the enemy and the target
           Vector3 direction = target.transform.position - transform.position;
           
           // Normalize the direction vector, making it a Unit vector so we can move with it.
           direction.Normalize();

            // Multiply the direction and speed, as well as our time scalar to get our velocity
           Vector3 velocity = direction * speed;
           transform.position += (velocity / subPixelMeasurements) * Time.deltaTime;

            if(Mathf.Sign(velocity.x) != previousXDirection && velocity.x != 0) {
                    render.flipX = !render.flipX;
                    previousXDirection = Mathf.Sign(velocity.x);
            }
            
    }

    // Gets the next waypoint for our object to move towards
    int GetNextWaypoint() {
        int numOfWaypoints = patrolPoints.Length;
        int nextWaypoint;
        if(randomizePatrolPoints){
            // Handle the next random waypoint
            int rand;
            while(true) {
                // Get a random number in our list. Add a 1 to the end because Random.Range
                // is exclusive with the length with integers
                rand = Random.Range(0, numOfWaypoints + 1);
                // If the random number we got is different than the previous point, break the loop
                if(rand != currentPatrolPoint) break;
            }
            nextWaypoint = rand;
        } else {
            // Handle the next linear waypoint
            if(currentPatrolPoint == patrolPoints.Length) {
                nextWaypoint = 0;
            } else {
                nextWaypoint = currentPatrolPoint + 1;
            }
        }
        return nextWaypoint;
    }
}
