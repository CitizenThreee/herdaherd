using UnityEngine;

public class SheepManager : MonoBehaviour
{
    
    [SerializeField] GameObject sheepRef;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 pos = new Vector3(i, transform.position.y, i);
            Instantiate(sheepRef, pos, transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
