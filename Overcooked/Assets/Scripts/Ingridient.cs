using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingridient : Item {

    [SerializeField] private IngridientType ingridientType;
    [SerializeField] private IngridientStatus ingridientStatus;
    public IngridientType IngridientType { get { return ingridientType; } }
    public IngridientStatus IngridientStatus { get { return ingridientStatus; } }

}
