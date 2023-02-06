using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingridient : Item {

    [SerializeField] private IngridientType ingridientType;
    [SerializeField] private IngridientStatus ingridientStatus;
    [SerializeField] private int processCountMax;
    public IngridientType IngridientType { get { return ingridientType; } }
    public IngridientStatus IngridientStatus { get { return ingridientStatus; } }
    public int ProcessCountMax { get { return processCountMax; } }

    public void ChangeStatus(IngridientStatus newStatus) {
        ingridientStatus = newStatus;
    }
}
