using System.Collections;
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
    public Animator PlayerAnimator { get { return playerAnimator; } }

    [SerializeField] protected AudioClip pickingItemClipAudio;

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
        var closestFurniture = interactableController.ClosestInteractableFurniture;
        if (closestFurniture == null) return;

        if (itemInHand) {
            //If we have an item in our hand, call handler to decide next action.
            HandlePickDrop(closestFurniture);
        }
        else {
            //If we don't have an item in our hand, we basically try to pick item from furniture.
            PickItem(closestFurniture);
        }
    }

    private void GameInputController_OnInteractAction(object sender, System.EventArgs e) {
        var closestFurniture = interactableController.ClosestInteractableFurniture;
        if (closestFurniture == null) return;

        bool interactAccomplished = closestFurniture.Interact();
        if (interactAccomplished) {
            closestFurniture.InteractAnimation(this);
        }
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

    private void HandlePickDrop(Furniture closestFurniture) {
        //If the item we have is an ingredient or a dirty plate stack, try to drop it.
        if (itemInHand is Ingredient || itemInHand is DirtyPlateStack) {
            DropItem();
            return;
        }

        if (itemInHand is Dish) {
            //If the item we have is a dish...
            //...and there is no item on closest top of furniture, try to drop it.
            if (!closestFurniture.HasItemOnTop) {
                DropItem();
                return;
            }

            Dish dishInHand = itemInHand as Dish;
            Item pickableItem = closestFurniture.GetItemOnTop();
            //If the item we have is a dish and also there is item on closest top of furniture.
            if (pickableItem is Ingredient) {
                Ingredient pickableIngredient = pickableItem as Ingredient;
                //If the ingredient on top of the furniture can be added to Dish, add it.
                if (dishInHand.CanAddIngredient(pickableIngredient)) {
                    dishInHand.AddIngredient(pickableIngredient);
                    closestFurniture.ClearItemOnTop();
                }
                //If the ingredient on top of the furniture cannot be added to Dish...
                else {
                    //...Furniture Ingredient Box has already created the ingredient...
                    //...so we need to delete it.
                    if (closestFurniture is IngredientBox) {
                        pickableItem.ThrowInTheGarbage();
                    }
                }
            }

            //If there is dish on top of furniture.
            else if (pickableItem is Dish) {
                Dish pickableDish = pickableItem as Dish;
                //If the dish on top of the furniture has ingredient...
                //...try to take on our dish.
                if (pickableDish.HasAnyIngredientOnTop) {
                    bool isTransferDone = pickableDish.TryToTransferIngredients(dishInHand);
                    //If the transfer failed, try the opposite transfer.
                    if (!isTransferDone) {
                        dishInHand.TryToTransferIngredients(pickableDish);
                    }
                }
                //If the dish on top of the furniture is empty...
                //...try to put the ingredients into the dish on top of the furniture.
                else {
                    dishInHand.TryToTransferIngredients(pickableDish);
                }
            }
        }
    }

    private void PickItem(Furniture closestFurniture) {
        Item pickedItem = closestFurniture.GetItemOnTop();
        if (pickedItem == null) return;

        closestFurniture.ClearItemOnTop();
        //Handle the position of the picked up item.
        HandlePickedItemPosition(pickedItem);
        playerAnimator.SetBool("PickedUp", true);
    }

    private void HandlePickedItemPosition(Item pickedItem) {
        pickedItem.transform.SetParent(itemSlot);
        pickedItem.transform.localPosition = Vector3.zero;
        pickedItem.transform.localRotation = Quaternion.identity;
        itemInHand = pickedItem;

        AudioManager.Instance.PlayEffectAudio(pickingItemClipAudio);
    }

    private void DropItem() {
        var closestFurniture = interactableController.ClosestInteractableFurniture;
        if (closestFurniture == null) return;

        //Check if the closest furniture object can accept the currently held item.
        if (closestFurniture.CanSetItemOnTop(itemInHand)) {
            //Place the item on top of the furniture.
            closestFurniture.SetItemOnTop(itemInHand);

            //Dish ingredients are destroyed when placed in the trash can...
            //...and the player should continue holding the dish after destroying.
            if (closestFurniture is TrashCan) {
                if (itemInHand is Dish) return;
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
