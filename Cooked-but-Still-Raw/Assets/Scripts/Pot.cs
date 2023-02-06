using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Dish {

    private List<IngridientType> soupIngridients = new List<IngridientType> {   
                                                                IngridientType.Tomato,
                                                                IngridientType.Onion};

    [SerializeField] private List<Ingridient> currentIngridients = new List<Ingridient>();

    private void Awake() {
        ingridientCapacity = 3;
    }

    public override bool CanAddIngridient(Item droppedItem) {
        if (currentIngridientQuantity >= ingridientCapacity) return false;
        if (!(droppedItem is Ingridient)) return false;

        Ingridient droppedIngridient = droppedItem as Ingridient;

        if (!soupIngridients.Contains(droppedIngridient.IngridientType)) return false;
        if (droppedIngridient.IngridientStatus != IngridientStatus.Processed) return false;

        return true;
    }

    public override void AddIngridient(Ingridient droppedIngridient) {
        currentIngridients.Add(droppedIngridient);
        currentIngridientQuantity++;
        droppedIngridient.gameObject.SetActive(false);
    }
}