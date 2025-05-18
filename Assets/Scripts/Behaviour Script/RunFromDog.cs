using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement/RunFromDog")]
public class RunFromDog : SheepBehaviour
{
    [SerializeField] private float dogDetectionRange = 10f;
    [SerializeField] private float panicMultiplier = 2f;
    [SerializeField] private float minDistance = 2f;

    public override Vector3 calculateMove(SheepLogic sheep, List<Transform> context, SheepManager manager, DogController dog)
    {
        // Calculate distance to dog
        Vector3 distanceToDog = sheep.transform.position - dog.transform.position;
        float distance = distanceToDog.magnitude;

        // If dog is outside detection range, no movement needed
        if (distance > dogDetectionRange) return Vector3.zero;

        // Calculate avoidance force based on distance
        float panicIntensity = 1f - Mathf.Clamp01((distance - minDistance) / (dogDetectionRange - minDistance));
        // Square the panic intensity to create stronger avoidance at closer distances
        panicIntensity = panicIntensity * panicIntensity;

        // Calculate avoidance direction and scale by panic intensity
        Vector3 avoidanceDirection = distanceToDog.normalized;
        return avoidanceDirection * (panicIntensity * panicMultiplier * manager.maxSpeed);
    }
} 