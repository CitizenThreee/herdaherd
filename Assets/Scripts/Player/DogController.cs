using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DogController : MonoBehaviour
{
    [SerializeField] private InputActionAsset dogActions;
    private InputAction mousePos;
    private InputAction run;
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;

    public Vector3 worldPosition;
    Plane plane = new Plane(Vector3.up, 0);

    void Start()
    {
    }

    void Update()
    {
        if(run.ReadValue<float>() > 0f)
        {
            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distance))
            {
                worldPosition = ray.GetPoint(distance);

                Vector3 direction = (worldPosition - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
                
                transform.position = Vector3.MoveTowards(transform.position, worldPosition, speed * Time.deltaTime);
            }
        }
    }

    private void OnEnable()
    {
        mousePos = dogActions.FindActionMap("Main").FindAction("mousePos");
        mousePos.Enable();
        run = dogActions.FindActionMap("Main").FindAction("run");
        run.Enable();
    }

    private void OnDisable()
    {
        mousePos.Disable();
        run.Disable();
    }
}
