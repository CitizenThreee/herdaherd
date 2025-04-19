using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SheepManager : MonoBehaviour
{
    
    [SerializeField] GameObject sheepRef;
    public SheepLogic sheepPrefab;
    List<SheepLogic> sheeps = new List<SheepLogic>();
    public SheepBehaviour behaviour;

    [Range(10,500)]
    public int startCount = 250;
    private const float angentDensity = 0.02f;
    
    [Range(1f, 100f)] 
    public float driveFactor = 10f;
    
    [Range(0f, 100f)]
    public float maxSpeed = 5f;

    [Range(0f, 10f)] 
    public float neighbourRadius = 1.5f;
    
    [Range(0f, 10f)] 
    public float avoidanceMultiplier = 0.5f;

    private float squareMaxSpeed;
    private float squareNeighbourRadius;
    public float squareAvoidanceMultiplier { get; private set; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        squareAvoidanceMultiplier = squareNeighbourRadius * avoidanceMultiplier * avoidanceMultiplier;
        
        for (float i = 0; i < startCount; i++)
        {
            // for (float j = 0; j < 10; j++)
            {
                Vector2 pos = Random.insideUnitCircle * startCount * angentDensity;
                Vector3 pos3D = new Vector3(Mathf.Clamp(pos.x, -5, 5), transform.position.y + 0.06f, Mathf.Clamp(pos.y, -5, 5));
                SheepLogic sheepObj = Instantiate(sheepPrefab,
                    pos3D,
                    Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                    transform
                );
                sheeps.Add(sheepObj);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (SheepLogic sheep in sheeps)
        {
            List<Transform> context = GetNearbyNeighbours(sheep);
            Vector3 move = behaviour.calculateMove(sheep, context, this);
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
        //Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(sheep.transform.position, neighbourRadius);
        foreach (Collider c in colliders)
        {
            if (c != sheep)
            {
                //Debug.Log("neightbours");
                context.Add(c.transform);
            }
        }
        return context;
    }
}
