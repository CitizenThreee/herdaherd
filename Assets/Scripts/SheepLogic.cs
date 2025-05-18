using UnityEngine;

[RequireComponent(typeof(Collider))] // Changed to 3D collider
[RequireComponent(typeof(Rigidbody))]
public class SheepLogic : MonoBehaviour
{
    [SerializeField] private Collider agentCollider;
    [SerializeField] private Rigidbody rb;
    public float maxForce = 10f;

    public Collider AgentCollider
    {
        get { return agentCollider; }
    }

    public void Move(Vector3 desiredVelocity)
    {
        desiredVelocity.y = 0;
        
        // Calculate the force needed to achieve the desired velocity
        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 force = (desiredVelocity - currentVelocity);
        
        // Limit the maximum force
        if (force.magnitude > maxForce)
        {
            force = force.normalized * maxForce;
        }
        
        // Apply the force
        rb.AddForce(force, ForceMode.Acceleration);
        
        // Rotate to face movement direction if we're moving
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(rb.linearVelocity);
        }
    }
}