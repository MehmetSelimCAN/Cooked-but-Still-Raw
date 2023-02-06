using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputController : MonoBehaviour {

    //Input System Properties
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction dashAction;
    private InputAction pickDropAction;
    private InputAction interactAction;

    public event EventHandler OnPickDropAction;
    public event EventHandler OnInteractAction;
    public event EventHandler OnDashAction;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        dashAction = playerInput.currentActionMap["Dash"];
        moveAction = playerInput.currentActionMap["Movement"];
        pickDropAction = playerInput.currentActionMap["PickDrop"];
        interactAction = playerInput.currentActionMap["Interact"];

        SubscribeControllerEvents();
    }

    private void SubscribeControllerEvents() {
        pickDropAction.performed += PickDropAction_performed;
        interactAction.performed += InteractAction_performed;
        dashAction.performed += DashAction_performed;
    }

    private void UnsubscribeControllerEvents() {
        pickDropAction.performed -= PickDropAction_performed;
        interactAction.performed -= InteractAction_performed;
        dashAction.performed -= DashAction_performed;
    }

    private void DashAction_performed(InputAction.CallbackContext context) {
        OnDashAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAction_performed(InputAction.CallbackContext context) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void PickDropAction_performed(InputAction.CallbackContext context) {
        OnPickDropAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementDirectionNormalized() {
        Vector2 inputVector = moveAction.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }
}
