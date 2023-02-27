using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //Movement Properties
    private float movementSpeed = 15f;
    private float rotationSpeed = 15f;
    private Vector3 movementDirection;
    private Vector3 inputDirection;
    private Rigidbody playerRigidbody;

    //Dash Properties
    private float dashForce = 900f;
    private float dashDuration = 0.17f;
    private WaitForSeconds dashCooldown = new WaitForSeconds(0.25f);
    private bool isDashingPossible = true;

    //Controllers
    private GameInputController gameInputController;
    private InteractController interactableController;

    [SerializeField] private Transform itemSlot;
    private Item itemInHand;

    private Animator playerAnimator;

    private void Awake() {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        gameInputController = GetComponent<GameInputController>();
        interactableController = GetComponent<InteractController>();
    }

    private void Start() {
        gameInputController.OnPickDropAction += GameInputController_OnPickDropAction;
        gameInputController.OnInteractAction += GameInputController_OnInteractAction;
        gameInputController.OnDashAction += GameInputController_OnDashAction;
    }

    private void GameInputController_OnPickDropAction(object sender, System.EventArgs e) {
        if (itemInHand) {
            DropItem();
        }
        else {
            PickItem();
        }
    }

    private void GameInputController_OnInteractAction(object sender, System.EventArgs e) {
        var closestFurniture = interactableController.ClosestInteractableFurniture;
        if (closestFurniture == null) return;

        closestFurniture.Interact();
    }

    private void GameInputController_OnDashAction(object sender, System.EventArgs e) {
        if (!isDashingPossible) return;
        StartCoroutine(Dash());
    }

    private void Update() {
        CalculateMovementDirection();
    }

    private void FixedUpdate() {
        MoveThePlayer();
        TurnThePlayer();
        AnimateThePlayer();
    }

    private  void CalculateMovementDirection() {
        inputDirection = gameInputController.GetMovementDirectionNormalized();
        movementDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
    }

    public void MoveThePlayer() {
        playerRigidbody.velocity = movementDirection * movementSpeed;
    }

    private void TurnThePlayer() {
        if (!(playerRigidbody.velocity.magnitude > 0.1f) || inputDirection == Vector3.zero) return;

        Quaternion newRotation = Quaternion.LookRotation(new Vector3(inputDirection.x, 0, inputDirection.y));
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
    }

    private void AnimateThePlayer() {
        playerAnimator.SetBool("Running", movementDirection != Vector3.zero ? true : false);
    }

    private void PickItem() {
        Item pickedItem = interactableController.ClosestInteractableFurniture?.GetItemOnTop();
        if (pickedItem == null) return;

        HandlePickedItemPosition(pickedItem);
        playerAnimator.SetBool("PickedUp", true);
    }

    private void HandlePickedItemPosition(Item pickedItem) {
        pickedItem.transform.SetParent(itemSlot);
        pickedItem.transform.localPosition = Vector3.zero;
        pickedItem.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        itemInHand = pickedItem;
    }

    private void DropItem() {
        var closestFurniture = interactableController.ClosestInteractableFurniture;
        if (closestFurniture == null) return;

        if (closestFurniture.CanSetItemOnTop(itemInHand)) {
            closestFurniture.SetItemOnTop(itemInHand);

            //Eðer drop edeceðimiz furniture trash can ise Dish'i clearlayýp player'ýn elinde tutmaya devam edecek.
            if (closestFurniture is TrashCan) {
                if (itemInHand is Dish) { return; }
            }

            itemInHand = null;
            playerAnimator.SetBool("PickedUp", false);
        }
    }

    private IEnumerator Dash() {
        float startTime = Time.time;
        isDashingPossible = false;

        while (Time.time < startTime + dashDuration) {
            playerRigidbody.AddRelativeForce(dashForce * Vector3.forward);
            yield return null;
        }

        yield return dashCooldown;
        isDashingPossible = true;
    }
}
