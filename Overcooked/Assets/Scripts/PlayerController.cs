using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    //Movement Properties
    private float movementSpeed = 15f;
    private Vector3 movementDirection;
    private Vector3 inputDirection;
    private Rigidbody playerRigidbody;

    //Dash Properties
    private float dashForce = 900f;
    private float dashDuration = 0.17f;
    private WaitForSeconds dashCooldown = new WaitForSeconds(0.25f);
    private bool _isDashingPossible = true;

    //Input System Properties
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction dashAction;
    private InputAction pickDropAction;
    private InputAction interactAction;

    private InteractController interactableController;

    [SerializeField] private Transform itemSlot;
    private Item itemInHand;

    private Animator playerAnimator;


    private void Awake() {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();

        playerInput = GetComponent<PlayerInput>();
        dashAction = playerInput.currentActionMap["Dash"];
        moveAction = playerInput.currentActionMap["Movement"];
        pickDropAction = playerInput.currentActionMap["PickDrop"];
        interactAction = playerInput.currentActionMap["Interact"];

        interactableController = GetComponent<InteractController>();

        SubscribeControllerEvents();
    }

    private void SubscribeControllerEvents() {
        pickDropAction.performed += PickDrop;
        dashAction.performed += Dash;
        interactAction.performed += Interact;
        interactAction.started += InteractStart;
        interactAction.canceled += InteractCancel;
    }

    private void UnsubscribeControllerEvents() {
        pickDropAction.performed -= PickDrop;
        dashAction.performed -= Dash;
    }

    private void Update() {
        CalculateMovementDirection();
    }

    private void FixedUpdate() {
        MoveThePlayer();
        TurnThePlayer();
        AnimateThePlayer();
    }

    private void CalculateMovementDirection() {
        inputDirection = moveAction.ReadValue<Vector2>();
        movementDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
    }

    public void MoveThePlayer() {
        playerRigidbody.velocity = movementDirection * movementSpeed;
    }

    private void TurnThePlayer() {
        if (!(playerRigidbody.velocity.magnitude > 0.1f) || inputDirection == Vector3.zero) return;

        Quaternion newRotation = Quaternion.LookRotation(new Vector3(inputDirection.x, 0, inputDirection.y));
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 15f);
    }

    private void AnimateThePlayer() {
        playerAnimator.SetBool("Running", movementDirection != Vector3.zero ? true : false);
    }

    private void PickDrop(InputAction.CallbackContext context) {
        if (itemInHand) DropItem();
        else PickItem();
    }
    private void PickItem() {
        Item pickedItem = interactableController.ClosestInteractableFurniture?.GetItemOnTop();
        if (pickedItem == null) return;

        HandlePickedItem(pickedItem);
        playerAnimator.SetBool("PickedUp", true);
    }

    private void HandlePickedItem(Item pickedItem) {
        pickedItem.transform.SetParent(itemSlot);
        pickedItem.transform.localPosition = Vector3.zero;
        pickedItem.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        itemInHand = pickedItem;
    }

    private void DropItem() {
        var closestFurniture = interactableController.ClosestInteractableFurniture;
        if (closestFurniture == null) return;

        if (itemInHand is Dish) {
            Dish dishInHand = itemInHand as Dish;

            if (closestFurniture.ItemOnTop is Ingridient) {
                Ingridient ingridientOnClosestFurniture = (Ingridient)closestFurniture.ItemOnTop;

                if (dishInHand.CanAddIngridient(ingridientOnClosestFurniture)) {
                    dishInHand.AddIngridient(ingridientOnClosestFurniture);
                }
            }
        }
        else {
            if (closestFurniture.CanSetItemOnTop(itemInHand)) {
                closestFurniture.SetItemOnTop(itemInHand);
                itemInHand = null;
                playerAnimator.SetBool("PickedUp", false);
            }
        }

    }

    private void Dash(InputAction.CallbackContext context) {
        if (!_isDashingPossible) return;
        StartCoroutine(Dash());
    }

    private IEnumerator Dash() {
        float startTime = Time.time;
        _isDashingPossible = false;

        while (Time.time < startTime + dashDuration) {
            playerRigidbody.AddRelativeForce(dashForce * Vector3.forward);
            yield return null;
        }

        yield return dashCooldown;
        _isDashingPossible = true;
    }

    private void Interact(InputAction.CallbackContext context) {
        Debug.Log("interact performed");
    }

    private void InteractStart(InputAction.CallbackContext context) {
        Debug.Log("interact start");
    }

    private void InteractCancel(InputAction.CallbackContext context) {
        Debug.Log("interact cancel");
    }
}
