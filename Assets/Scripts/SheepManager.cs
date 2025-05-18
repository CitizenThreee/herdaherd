using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SheepManager : MonoBehaviour
{
    public SheepLogic sheepPrefab;
    List<SheepLogic> sheeps = new List<SheepLogic>();
    public SheepBehaviour behaviour;

    public DogController dog;

    [Range(10,500)]
    public int startCount = 250;
    private const float agentDensity = 0.02f;
    
    [Range(1f, 50f)] 
    public float driveFactor = 5f;
    
    [Range(0f, 20f)]
    public float maxSpeed = 5f;

    [Range(0f, 10f)] 
    public float neighbourRadius = 1.5f;
    
    [Range(0f, 10f)] 
    public float avoidanceMultiplier = 0.5f;

    [Range(0f, 350f)]
    public float blindspotAngle = 90f;

    private float squareMaxSpeed;
    private float squareNeighbourRadius;
    public float squareAvoidanceMultiplier { get; private set; }

    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        squareAvoidanceMultiplier = squareNeighbourRadius * avoidanceMultiplier * avoidanceMultiplier;
        
        for (float i = 0; i < startCount; i++)
        {
            Vector2 pos = Random.insideUnitCircle * startCount * agentDensity;
            Vector3 pos3D = new Vector3(
                Mathf.Clamp(pos.x, -5, 5), 
                transform.position.y + 0.06f, 
                Mathf.Clamp(pos.y, -5, 5)
            );
            
            SheepLogic sheepObj = Instantiate(
                sheepPrefab,
                pos3D,
                Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)),
                transform
            );
            
            sheeps.Add(sheepObj);
        }
    }

    void FixedUpdate() // Changed from Update to FixedUpdate for physics
    {
        foreach (SheepLogic sheep in sheeps)
        {
            List<Transform> context = GetNearbyNeighbours(sheep);
            Vector3 move = behaviour.calculateMove(sheep, context, this, dog);
            
            // Scale the movement by drive factor
            move *= driveFactor;
            
            // Limit to max speed
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            
            sheep.Move(move);
        }
    }

    private List<Transform> GetNearbyNeighbours(SheepLogic sheep)
    {
        List<Transform> context = new List<Transform>();
        
        Collider[] colliders = Physics.OverlapSphere(sheep.transform.position, neighbourRadius);
        foreach (Collider c in colliders)
        {
            if (c != sheep.AgentCollider)
            {
                Vector3 toTarget = c.transform.position - sheep.transform.position;
                float angle = Vector3.Angle(sheep.transform.forward, toTarget);

                if (angle > 180 - blindspotAngle / 2) { continue; }

                context.Add(c.transform);
            }
        }
        return context;
    }
}
