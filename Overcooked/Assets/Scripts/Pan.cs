using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : Dish {

    [SerializeField] private Transform ingridientSlot;

    private List<IngridientType> cookableIngridients = new List<IngridientType> { IngridientType.Meat };

    private void Awake() {
        ingridientCapacity = 1;
    }

    public override bool AcceptIngridients(Item droppedItem) {
        if (currentIngridientQuantity >= ingridientCapacity) return false;
        if (!(droppedItem is Ingridient)) return false;

        Ingridient droppedIngridient = droppedItem as Ingridient;

        if (cookableIngridients.Contains(droppedIngridient.IngridientType)) {
            if (droppedIngridient.IngridientStatus == IngridientStatus.Raw) {
                //Tavaya et ekle
                droppedIngridient.transform.SetParent(ingridientSlot);
                droppedIngridient.transform.localPosition = Vector3.zero;
                currentIngridientQuantity++;
                return true;
            }
        }

        return false;
    }
}
