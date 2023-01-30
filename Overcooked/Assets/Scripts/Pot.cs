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

    public override bool AcceptIngridients(Item droppedItem) {
        if (currentIngridientQuantity >= ingridientCapacity) return false;
        if (!(droppedItem is Ingridient)) return false;

        Ingridient droppedIngridient = droppedItem as Ingridient;

        if (soupIngridients.Contains(droppedIngridient.IngridientType)) {
            if (droppedIngridient.IngridientStatus == IngridientStatus.Processed) {
                //Çorbaya malzeme ekle
                //TODO: Her ingridient ekleniþinde Mesh renderer deðiþecek.
                currentIngridients.Add(droppedIngridient);
                currentIngridientQuantity++;
                droppedIngridient.gameObject.SetActive(false);
                return true;
            }
        }

        return false;
    }
}