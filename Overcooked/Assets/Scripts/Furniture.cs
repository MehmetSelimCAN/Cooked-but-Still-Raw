using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Furniture : Interactable {

    [SerializeField] protected Item itemOnTop;
    [SerializeField] protected Transform itemSlot;
    public bool HasItemOnTop { get { return itemOnTop == null ? false : true; } }
    public Item ItemOnTop { get { return itemOnTop; } }

    public virtual Item GetItemOnTop() { return null; }
    public virtual bool TrySetItemOnTop(Item droppedItem) { return false; }
}
