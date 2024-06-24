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

    private InputAction moveAction;
    //private InputAction lookAction;
    private InputAction fireAction;
    private InputAction pauseAction;

    public Vector2 MoveInput { get; private set; }
    //public Vector2 LookInput { get; private set; }
    public bool fireTriggered { get; private set; }
    public bool pauseTriggered { get; private set; }
    public bool pauseDown { get; private set; }

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
    }
    private void OnDisable()
    {
        moveAction.Disable();
        //lookAction.Disable();
        fireAction.Disable();
        pauseAction.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {

    }
}
