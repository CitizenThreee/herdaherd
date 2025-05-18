using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement/AntiCollision")]
public class AntiCollision : SheepBehaviour
{
    public float rayLength = 1.5f;
    public LayerMask obstacleLayer;

    public override Vector3 calculateMove(SheepLogic sheep, List<Transform> context, SheepManager manger, DogController dog)
    {
        Vector3 avoidanceMove = Vector3.zero;
        bool needsAvoidance = false;
        
        // Check for obstacles using raycasts
        Vector3[] rayDirections = new Vector3[]
        {
            sheep.transform.forward,
            Quaternion.Euler(0, 45, 0) * sheep.transform.forward,
            Quaternion.Euler(0, -45, 0) * sheep.transform.forward,
            Quaternion.Euler(0, 90, 0) * sheep.transform.forward,
            Quaternion.Euler(0, -90, 0) * sheep.transform.forward
        };

        foreach (Vector3 direction in rayDirections)
        {
            Ray ray = new Ray(sheep.transform.position, direction);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, rayLength, obstacleLayer))
            {
                needsAvoidance = true;
                float distance = hit.distance;
                Vector3 avoidanceVector = hit.normal * (rayLength - distance) / (distance * 2);
                avoidanceMove += avoidanceVector;
            }
        }

        if (needsAvoidance)
        {
            return avoidanceMove.normalized * manger.maxSpeed;
        }

        return Vector3.zero;
    }
}
