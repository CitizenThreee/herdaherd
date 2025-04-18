using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;

    private void Awake()
    {
        player = Object.FindFirstObjectByType<DogController>().gameObject;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(player != null)
        {
            Vector3 playerPos = player.transform.position;
            transform.position = new Vector3(playerPos.x, transform.position.y, playerPos.z);
        }
    }
}
