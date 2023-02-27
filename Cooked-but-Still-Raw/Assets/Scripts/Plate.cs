using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : Dish {

    [SerializeField] private Transform ingridientSlot;
    [SerializeField] private List<Ingridient> currentIngridients = new List<Ingridient>();

    public override bool CanAddIngridient(Item droppedItem) {
        if (!(droppedItem is Ingridient)) return false;

        Ingridient droppedIngridient = droppedItem as Ingridient;

        if (droppedIngridient.IngridientStatus != IngridientStatus.Processed) return false;

        return true;
    }

    public override void AddIngridient(Ingridient droppedIngridient) {
        currentIngridients.Add(droppedIngridient);
        currentIngridientQuantity++;
        droppedIngridient.gameObject.SetActive(false);
    }

    public override void ClearCurrentIngridients() {
        foreach (Transform ingridient in ingridientSlot) {
            Destroy(ingridient.gameObject);
        }

        currentIngridientQuantity = 0;
        Debug.Log("Clear Plate");
    }

}
