using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DogController : MonoBehaviour
{
    [SerializeField] private InputActionAsset dogActions;
    private InputAction mousePos;
    private InputAction run;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(run.triggered)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            transform.position = new Vector3(worldPosition.x, transform.position.y, worldPosition.y);
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
