using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : Dish {

    [SerializeField] private Transform ingridientSlot;

    private void Awake() {
        ingridientCapacity = 1;
    }

    public override bool CanAddIngridient(Item droppedItem) {
        if (currentIngridientQuantity >= ingridientCapacity) return false;
        if (!(droppedItem is Ingridient)) return false;

        Ingridient droppedIngridient = droppedItem as Ingridient;

        if (droppedIngridient.IngridientStatus != IngridientStatus.Processed) return false;
        if (!(droppedIngridient is IFryable)) return false;

        return true;
    }

    public override void AddIngridient(Ingridient droppedIngridient) {
        droppedIngridient.transform.SetParent(ingridientSlot);
        droppedIngridient.transform.localPosition = Vector3.zero;
        currentIngridientQuantity++;
    }

    public override void ClearCurrentIngridients() {
        foreach (Transform ingridient in ingridientSlot) {
            Destroy(ingridient.gameObject);
        }

        currentIngridientQuantity = 0;
        Debug.Log("Clear Pan");
    }
}
