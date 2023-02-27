using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Dish {

    [SerializeField] private float soupSnappingOffSet;
    [SerializeField] private Transform ingridientSlot;
    [SerializeField] private List<Ingridient> currentIngridients = new List<Ingridient>();

    private void Awake() {
        soupSnappingOffSet = 20;
        ingridientCapacity = 3;
    }

    public override bool CanAddIngridient(Item droppedItem) {
        if (currentIngridientQuantity >= ingridientCapacity) return false;
        if (!(droppedItem is Ingridient)) return false;

        Ingridient droppedIngridient = droppedItem as Ingridient;

        if (droppedIngridient.IngridientStatus != IngridientStatus.Processed) return false;
        if (!(droppedIngridient is ICookable)) return false;

        return true;
    }

    public override void AddIngridient(Ingridient droppedIngridient) {
        currentIngridients.Add(droppedIngridient);
        currentIngridientQuantity++;

        ICookable cookableIngridient = droppedIngridient as ICookable;
        cookableIngridient.Liquize();
        droppedIngridient.transform.SetParent(ingridientSlot);
        droppedIngridient.transform.localPosition = Vector3.up * soupSnappingOffSet * (currentIngridientQuantity - 1);
        droppedIngridient.transform.localScale = Vector3.one;
    }


    public override void ClearCurrentIngridients() {
        foreach (Transform ingridient in ingridientSlot) {
            Destroy(ingridient.gameObject);
        }

        currentIngridientQuantity = 0;
        Debug.Log("Clear Pot");
    }
}