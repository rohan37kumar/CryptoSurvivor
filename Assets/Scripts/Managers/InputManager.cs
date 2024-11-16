using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    //public Vector2 MovementInput { get; private set; }
    public Vector2 SwipeInput { get; private set; }

    [SerializeField] private Joystick inputJoystick;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        //HandleKeyboardInput();
        HandleTouchInput();
        //Debug.Log("Touch value: "+SwipeInput);
    }
/*
    private void HandleKeyboardInput()
    {
        MovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
*/
    private void HandleTouchInput()
    {
        // Handle swipe input
        SwipeInput = Vector2.zero;
        if (inputJoystick.Direction != Vector2.zero)
        {
            SwipeInput = inputJoystick.Direction;
        }

    }
}