using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BowArrow : MonoBehaviour
{
    public GameObject BowObject;

    public bool aimReady = false;
    public Color aimColor = Color.red;
    public float aimLength = 20f;

    // A reference to the tip of our bow; will use to draw lines from
    public Transform bowTip;

    // Reference to our line renderer script
    public LineRenderer lineRenderer;

    public LayerMask aimLayerMask;

    public int reflections = 5;

    void Update() {
        DrawAim();
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    public float GetAngleOfBow() {
        // Get the screen position of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        // Get the screen position of the mouse
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        // Get the angle between the points
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
        return angle;

        
    }

    public void DrawBow() {

    }

    public Vector3 AngleToVector(float angle) {
        return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    public float drawDistance = -5f;

    public void DrawAim() {
        float angle = GetAngleOfBow();
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(bowTip.position, -1 * AngleToVector(angle), aimLength, aimLayerMask);
        BowObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        lineRenderer.SetPosition(0, bowTip.position + new Vector3(0, 0, drawDistance));
        if(hit.collider) {
            lineRenderer.SetPosition(1, (Vector3)hit.point + new Vector3(0, 0, drawDistance));            
        } else {
            lineRenderer.SetPosition(1, bowTip.position + (-1 * AngleToVector(angle) * aimLength));
        }

        // Get hit angle
        if(hit.collider) {
            RecursiveReflect(1, 3, hit, bowTip.position);
            /*Vector2 angleOfCollision = hit.normal;
            float usedDistance = hit.distance;
            float newDistance = aimLength - usedDistance;
            /*RaycastHit2D secondHit = Physics2D.Raycast(hit.point, angleOfCollision, newDistance, aimLayerMask);
            if(secondHit.collider) {
                lineRenderer.SetPosition(2, (Vector3)secondHit.point + new Vector3(0, 0, drawDistance));
            } else {
                lineRenderer.SetPosition(2, hit.point + (-1 * angleOfCollision * newDistance));
            }
           //print("diff = " + aimLength + " - " + usedDistance + " = " + newDistance);
            Quaternion deflectRotation = Quaternion.FromToRotation(AngleToVector(angle), hit.normal);

            Vector3 deflectDirection = deflectRotation * hit.normal;

            lineRenderer.SetPosition(2, deflectDirection * newDistance);*/

        } else {
            lineRenderer.positionCount = 2;
        }
    }

    public void RecursiveReflect(int currentReflection, int position, RaycastHit2D hit, Vector2 hitOrigin) {
            lineRenderer.positionCount = position;
            lineRenderer.positionCount = position + 1;
            Vector2 angleOfCollision = hit.normal;
            float usedDistance = hit.distance;
            float newDistance = aimLength - usedDistance;

            Quaternion deflectRotation = Quaternion.FromToRotation(hitOrigin, hit.normal);

            Vector3 deflectDirection = deflectRotation * hit.normal;

            RaycastHit2D newhit = Physics2D.Raycast(hit.point, deflectDirection, newDistance, aimLayerMask);
            if(hit.collider) {
                lineRenderer.SetPosition(position, hit.point);
                if(currentReflection < reflections) {
                    RecursiveReflect(currentReflection + 1, position + 1, newhit, hit.point);
                }
            } else {
                lineRenderer.SetPosition(position, deflectDirection * newDistance);
            }
    }
}
