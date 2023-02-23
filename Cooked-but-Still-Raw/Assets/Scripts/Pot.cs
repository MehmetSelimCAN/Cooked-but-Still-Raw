using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Dish {

    [SerializeField] private List<Ingridient> currentIngridients = new List<Ingridient>();

    private void Awake() {
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
        droppedIngridient.gameObject.SetActive(false);
    }
}