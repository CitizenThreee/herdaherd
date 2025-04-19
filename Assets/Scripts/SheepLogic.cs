using UnityEngine;

[RequireComponent(typeof(Collider2D))] // to see the neighbours
public class SheepLogic : MonoBehaviour
{
    private Collider2D agentCollider;
    public Collider2D AgentCollider2D
    {
        get { return agentCollider; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agentCollider = GetComponent<Collider2D>();

    }

    public void Move(Vector3 velocity)
    {
        transform.forward = velocity; // face the direction we're heading
        Vector3 movement = new Vector3(velocity.x, 0f, velocity.y);
        transform.position += movement * Time.deltaTime;
    }
}