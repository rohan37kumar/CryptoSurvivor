using UnityEngine;
using Lean.Touch;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public Vector2 MovementInput { get; private set; }
    public Vector2 SwipeInput { get; private set; }

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
    }

    private void HandleKeyboardInput()
    {
        MovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void HandleTouchInput()
    {
        // Handle swipe input
        /*
        SwipeInput = Vector2.zero;
        if (LeanTouch.Fingers.Count > 0)
        {
            LeanFinger finger = LeanTouch.Fingers[0];
            if (finger.DeltaScreenDelta.magnitude > 50f)
            {
                SwipeInput = finger.DeltaScreenDelta.normalized;
            }
        }
        */
    }
}