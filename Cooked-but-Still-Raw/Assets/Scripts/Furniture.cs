using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Furniture : Interactable {

    [SerializeField] protected Item itemOnTop;
    [SerializeField] protected Transform itemSlot;
    public bool HasItemOnTop { get { return itemOnTop == null ? false : true; } }
    public Item ItemOnTop { get { return itemOnTop; } }

    public virtual Item GetItemOnTop() {
        Item tempItem = itemOnTop;
        itemOnTop = null;
        return tempItem;
    }

    public virtual void SetItemOnTop(Item droppedItem) { }
    public virtual bool CanSetItemOnTop(Item droppedItem) { return false; }

    public virtual void Interact() { }
}
