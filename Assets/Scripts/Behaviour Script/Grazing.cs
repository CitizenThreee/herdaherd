using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement/Grazing")]
public class Grazing : SheepBehaviour
{
    [SerializeField] private float moveTimeMin = 2f;
    [SerializeField] private float moveTimeMax = 4f;
    [SerializeField] private float grazeTimeMin = 3f;
    [SerializeField] private float grazeTimeMax = 6f;
    [SerializeField] private float directionChangeInterval = 3f;
    [SerializeField] private float moveSpeed = 2f;

    private Dictionary<int, GrazingState> sheepStates = new Dictionary<int, GrazingState>();

    private class GrazingState
    {
        public bool isMoving;
        public float stateTimer;
        public Vector3 moveDirection;
        public float directionTimer;

        public GrazingState()
        {
            isMoving = Random.value > 0.5f;
            stateTimer = Random.value * 2f; // Random initial timer
            moveDirection = Random.insideUnitCircle.normalized;
            moveDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
            directionTimer = Random.value * 2f;
        }
    }

    public override Vector3 calculateMove(SheepLogic sheep, List<Transform> context, SheepManager manager, DogController dog)
    {
        int sheepId = sheep.GetInstanceID();
        
        // Initialize state for this sheep if it doesn't exist
        if (!sheepStates.ContainsKey(sheepId))
        {
            sheepStates[sheepId] = new GrazingState();
        }

        GrazingState state = sheepStates[sheepId];
        
        // Update timers
        state.stateTimer -= Time.deltaTime;
        state.directionTimer -= Time.deltaTime;

        // Change direction if timer expired
        if (state.directionTimer <= 0 && state.isMoving)
        {
            state.moveDirection = Random.insideUnitCircle.normalized;
            state.moveDirection = new Vector3(state.moveDirection.x, 0, state.moveDirection.y);
            state.directionTimer = directionChangeInterval;
        }

        // Switch states if timer expired
        if (state.stateTimer <= 0)
        {
            state.isMoving = !state.isMoving;
            state.stateTimer = state.isMoving ? 
                Random.Range(moveTimeMin, moveTimeMax) : 
                Random.Range(grazeTimeMin, grazeTimeMax);
            
            if (state.isMoving)
            {
                state.moveDirection = Random.insideUnitCircle.normalized;
                state.moveDirection = new Vector3(state.moveDirection.x, 0, state.moveDirection.y);
                state.directionTimer = directionChangeInterval;
            }
        }

        // Return movement based on state
        if (state.isMoving)
        {
            return state.moveDirection * moveSpeed;
        }

        return Vector3.zero;
    }
} 