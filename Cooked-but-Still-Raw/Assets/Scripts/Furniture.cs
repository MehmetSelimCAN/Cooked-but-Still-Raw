using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Furniture : MonoBehaviour {

    [SerializeField] protected Item itemOnTop;
    [SerializeField] protected Transform itemSlot;
    public bool HasItemOnTop { get { return itemOnTop == null ? false : true; } }

    public virtual void ClearItemOnTop() {
        itemOnTop = null;
    }

    public virtual Item GetItemOnTop() {
        return itemOnTop;
    }

    public virtual void SetItemOnTop(Item droppedItem) { }
    public virtual bool CanSetItemOnTop(Item droppedItem) { return false; }

    public virtual void HandleDroppedItemPosition(Item droppedItem) {
        droppedItem.transform.SetParent(itemSlot);
        droppedItem.transform.localPosition = Vector3.zero;
        droppedItem.transform.localRotation = Quaternion.identity;
    }

    public virtual void Interact() { }
}
