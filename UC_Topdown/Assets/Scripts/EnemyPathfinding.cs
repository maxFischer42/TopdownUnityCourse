using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
[RequireComponent(typeof(EnemyMovement))]
public class EnemyPathfinding : MonoBehaviour
{
    public Transform targetPosition;
    private Seeker seeker;
    public bool reachedDestination;
    public Path path;
    public float speed = 2;
    public float nextWaypointDistance = 3;
    private int currentWaypoint = 0;
    private EnemyMovement movement;
    public Vector2 currentVelocity;
    public void Start() {
        movement = GetComponent<EnemyMovement>();

        seeker = GetComponent<Seeker>();

        //seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);

    }

    public void SetDestination(Transform destination) {
        seeker.StartPath(transform.position, destination.position, OnPathComplete);
    }
    public void SetDestination(Vector2 destination) {
        seeker.StartPath(transform.position, destination, OnPathComplete);
    }

    public void OnPathComplete (Path p) {
        if(!p.error) {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }

    public void Update() {
        if(path == null) {
            // We have no path to follow yet, so don't do anything
            return;
        }

        // Check in a loop if we are close enough to the current waypoint to switch to the next one.
        // We do this in a loop because many waypoints might be close to each other and we may reach
        // several of them in the same frame.
        reachedDestination = false;

        // The distance to the next waypoint in the path
        float distanceToWaypoint;

                // Get the current speed of our object
        speed = GetComponent<EnemyMovement>().currentSpeed;

        while (true) {
            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance) {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count) {
                    currentWaypoint++;
                } else {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedDestination = true;
                    break;
                }
            } else {
                break;
            }
        }

        // Slow down smoothly upon approaching the end of the path
        // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
        var speedFactor = reachedDestination ? Mathf.Sqrt(distanceToWaypoint/nextWaypointDistance) : 1f;

        // Direction to the next waypoint
        // Normalize it so that it has a length of 1 world unit
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;

        // Multiply the direction by our desired speed to get a velocity
        Vector3 velocity = dir * speed * speedFactor;


        currentVelocity = dir;
    }


}
