using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;
using TMPro;
using System;

public class ControlUpdater : MonoBehaviour, IPointerClickHandler
{
    public InputActionAsset playerControls;
    public string actionMapName = "Player";
    public string actionName;

    private InputAction action;
    private TextMeshProUGUI buttonText;
    public string compositeName ; // Name of the composite binding
    public string bindingName ; // Name of the specific binding to change

    public Vector2 MoveInput { get; private set; }
    public bool fireTriggered { get; private set; }
    public bool jumpTriggered { get; private set; } // Add this line
    private void Start()
    {
        action = playerControls.FindActionMap(actionMapName).FindAction(actionName);
        RegisterInputActions();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (actionName == "Move")
        {
            UpdateControlMove(action);
        }else 
        {
            UpdateControl(action);
        }
    }
    void UpdateControlMove(InputAction action)
    {
        // Find the binding index for the specific part of the composite
        int bindingIndex = -1;
        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (action.bindings[i].isPartOfComposite && action.bindings[i].name==bindingName && action.bindings[i].action == action.name)
            {
                bindingIndex = i;
                break;
            }
        }
        Debug.Log(bindingIndex);
        if (bindingIndex == -1)
        {
            Debug.LogError("Binding not found");
            return;
        }

        // Disable the action while rebinding
        action.Disable();

        var rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(operation =>
            {
                // After rebinding, re-enable the action and update the button text
                action.Enable();
                // Extract the key from the effective path
                string fullPath = action.bindings[bindingIndex].effectivePath;
                string key = fullPath.Substring(fullPath.LastIndexOf('/') + 1).ToUpper();

                buttonText.text = key;
                operation.Dispose();
            });

        rebindOperation.Start();
        RegisterInputActions();
    }
    void UpdateControl(InputAction action)
    {
        // Find the binding index for the specific part of the composite
        int bindingIndex = -1;
        for (int i = 0; i < action.bindings.Count; i++)
        {
            Debug.Log(action.bindings[i].action);
            if (action.bindings[i].action == action.name)
            {
                bindingIndex = i;
                break;
            }
        }
        if (bindingIndex == -1)
        {
            Debug.LogError("Binding not found");
            return;
        }

        // Disable the action while rebinding
        action.Disable();

        var rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(operation =>
            {
                // After rebinding, re-enable the action and update the button text
                action.Enable();
                // Extract the key from the effective path
                string fullPath = action.bindings[bindingIndex].effectivePath;
                string key = fullPath.Substring(fullPath.LastIndexOf('/') + 1).ToUpper();

                buttonText.text = key;
                operation.Dispose();
            });

        rebindOperation.Start();
        RegisterInputActions();
    }
    void RegisterInputActions()
    {
        switch (actionName)
        {
            case "Move":
                action.performed += context => MoveInput = context.ReadValue<Vector2>();
                action.canceled += context => MoveInput = Vector2.zero;
                break;
            case "Jump":
                action.performed += context => jumpTriggered = true;
                action.canceled += context => jumpTriggered = false;
                break;
            case "Fire":
                 action.performed += context => fireTriggered = true;
                 action.canceled += context => fireTriggered = false;
                break;
                // Add other cases as needed
        }

        // Example for other actions
        // lookAction.performed += context => LookInput = context.ReadValue<Vector2>();
        // lookAction.canceled += context => LookInput = Vector2.zero;

        // fireAction.performed += context => fireTriggered = true;
        // fireAction.canceled += context => fireTriggered = false;

        // pauseAction.performed += context => pauseTriggered = true;
        // pauseAction.canceled += context => pauseTriggered = false;

    }
}
