using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;
using TMPro;
using System;
using System.Collections.Generic;

public class ControlUpdater : MonoBehaviour, IPointerClickHandler
{
    public InputActionAsset playerControls;
    public string actionMapName = "Player";
    public string actionName;

    private InputAction action;
    private TextMeshProUGUI buttonText;
    public string compositeName; // Name of the composite binding
    public string bindingName; // Name of the specific binding to change

    public Vector2 MoveInput { get; private set; }
    public bool fireTriggered { get; private set; }
    public bool jumpTriggered { get; private set; } // Add this line

    private void Start()
    {
        action = playerControls.FindActionMap(actionMapName).FindAction(actionName);
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private bool CheckDuplicateBinding(InputAction action, int bindingIndex)
    {
        InputBinding newBinding = action.bindings[bindingIndex];
        int count = 0;

        foreach (InputBinding binding in action.actionMap.bindings)
        {
            if (binding.effectivePath == newBinding.effectivePath)
            {
                count++;
            }

        }
        if(count>1)return true;
        return false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (actionName == "Move")
        {
            UpdateControlMove(action);
        }
        else
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
            if (action.bindings[i].isPartOfComposite && action.bindings[i].name == bindingName && action.bindings[i].action == action.name)
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

        InputBinding oldBinding = action.bindings[bindingIndex];
        // Store the original binding path
        string originalBindingPath = oldBinding.effectivePath;

        // Disable the action while rebinding
        action.Disable();

        var rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(operation =>
            {
                if (CheckDuplicateBinding(action, bindingIndex))
                {
                    // Rollback to the original binding path if duplicate is found
                    action.ApplyBindingOverride(bindingIndex, new InputBinding { overridePath = originalBindingPath });

                    Debug.Log("Duplicate binding found. Rolling back to original binding.");
                }
                else
                {
                    // Extract the key from the effective path
                    string fullPath = action.bindings[bindingIndex].effectivePath;
                    string key = fullPath.Substring(fullPath.LastIndexOf('/') + 1).ToUpper();

                    buttonText.text = key;
                }
                // After rebinding, re-enable the action 
                action.Enable();
                operation.Dispose();
            });

        rebindOperation.Start();
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

        InputBinding oldBinding = action.bindings[bindingIndex];
        // Store the original binding path
        string originalBindingPath = oldBinding.effectivePath;
        // Disable the action while rebinding
        action.Disable();

        var rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(operation =>
            {
                if (CheckDuplicateBinding(action, bindingIndex))
                {
                    // Rollback to the original binding path if duplicate is found
                    action.ApplyBindingOverride(bindingIndex, new InputBinding { overridePath = originalBindingPath });

                    Debug.Log("Duplicate binding found. Rolling back to original binding.");
                }
                else
                {
                    // Extract the key from the effective path
                    string fullPath = action.bindings[bindingIndex].effectivePath;
                    string key = fullPath.Substring(fullPath.LastIndexOf('/') + 1).ToUpper();

                    buttonText.text = key;
                }
                // After rebinding, re-enable the action 
                action.Enable();
                operation.Dispose();
            });

        rebindOperation.Start();
    }
}
