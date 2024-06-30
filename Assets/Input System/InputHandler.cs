using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public InputActionAsset playerControls;

    private string actionMapName = "Player";
    private string move = "Move";
    //private string look = "Look";
    private string fire = "Fire";
    private string pause = "Pause";
    private string jump = "Jump"; // Add this line

    private InputAction moveAction;
    //private InputAction lookAction;
    private InputAction fireAction;
    private InputAction pauseAction;
    private InputAction jumpAction; // Add this line

    public Vector2 MoveInput { get; private set; }
    //public Vector2 LookInput { get; private set; }
    public bool fireTriggered { get; private set; }
    public bool pauseTriggered { get; private set; }
    public bool pauseDown { get; private set; }
    public bool jumpTriggered { get; private set; } // Add this line

    public static InputHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
        //lookAction = playerControls.FindActionMap(actionMapName).FindAction(look);
        fireAction = playerControls.FindActionMap(actionMapName).FindAction(fire);
        pauseAction = playerControls.FindActionMap(actionMapName).FindAction(pause);
        jumpAction = playerControls.FindActionMap(actionMapName).FindAction(jump); // Add this line
        RegisterInputActions();
    }

    private void Update()
    {
        pauseTriggered = CheckTriggered(pauseAction);
    }

    void RegisterInputActions()
    {
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInput = Vector2.zero;

        //lookAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        //lookAction.canceled += context => MoveInput = Vector2.zero;

        fireAction.performed += context => fireTriggered = true;
        fireAction.canceled += context => fireTriggered = false;

        //pauseAction.performed += context => pauseTriggered = true;
        //pauseAction.canceled += context => pauseTriggered = false;

        jumpAction.performed += context => jumpTriggered = true; // Add this line
        jumpAction.canceled += context => jumpTriggered = false; // Add this line
    }

    bool CheckTriggered(InputAction action)
    {
        return action.triggered;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        //lookAction.Enable();
        fireAction.Enable();
        pauseAction.Enable();
        jumpAction.Enable(); // Add this line
    }

    private void OnDisable()
    {
        moveAction.Disable();
        //lookAction.Disable();
        fireAction.Disable();
        pauseAction.Disable();
        jumpAction.Disable(); // Add this line
    }

    // Start is called before the first frame update
    void Start()
    {

    }
}
