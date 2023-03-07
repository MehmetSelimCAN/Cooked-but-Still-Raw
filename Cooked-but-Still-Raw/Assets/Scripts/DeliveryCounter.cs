using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : Furniture {

    private float plateComebackTimerMax = 5f;
    private float plateComebackTimer = 5f;

    private List<Plate> dirtyPlates = new List<Plate>();
    [SerializeField] private DirtyPlateCounter dirtyPlateCounter;

    public override bool CanSetItemOnTop(Item droppedItem) {
        //Only a plate can be placed on top.
        if (droppedItem is Plate) {
            Plate droppedPlate = droppedItem as Plate;
            //And the plate has to be non-empty.
            if (droppedPlate.CurrentIngredientQuantity > 0) {
                return true;
            }
        }

        return false;
    }

    public override void SetItemOnTop(Item droppedItem) {
        Plate droppedPlate = droppedItem as Plate;
        //Checks if the served dish matches any order.
        OrderManager.Instance.CheckOrder(droppedPlate);

        dirtyPlates.Add(droppedPlate);
        droppedPlate.gameObject.transform.SetParent(null);
        droppedPlate.gameObject.SetActive(false);

        droppedPlate.ThrowInTheGarbage();
        droppedPlate.SetDirty();
        StartCoroutine(BringBackDirtyPlate());
    }

    //Bring the plate after the delivery as a dirty plate.
    private IEnumerator BringBackDirtyPlate() {
        while (plateComebackTimer > 0) {
            plateComebackTimer -= Time.deltaTime;        
            yield return null;
        }

        plateComebackTimer = plateComebackTimerMax;

        Plate dirtyPlate = dirtyPlates[0];
        dirtyPlates.Remove(dirtyPlates[0]);
        dirtyPlate.gameObject.SetActive(true);

        dirtyPlateCounter.SetItemOnTop(dirtyPlate);
    }
}
