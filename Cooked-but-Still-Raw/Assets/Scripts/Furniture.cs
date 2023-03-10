using UnityEngine;

public abstract class Furniture : MonoBehaviour {

    [SerializeField] protected ParticleSystem dropParticleEffect;
    [SerializeField] protected Item itemOnTop;
    [SerializeField] protected Transform itemSlot;
    public bool HasItemOnTop { get { return itemOnTop == null ? false : true; } }

    //Removing the item from the furniture.
    public virtual void ClearItemOnTop() {
        itemOnTop = null;
    }

    //Access the item currently on the furniture.
    public virtual Item GetItemOnTop() {
        return itemOnTop;
    }

    //Responsible for placing a new item on top of the furniture.
    public virtual void SetItemOnTop(Item droppedItem) { }
    //Responsible for checking if the furniture can accept a new item.
    public virtual bool CanSetItemOnTop(Item droppedItem) { return false; }

    //Responsible for handling the position of a newly dropped item.
    public virtual void HandleDroppedItemPosition(Item droppedItem) {
        droppedItem.transform.SetParent(itemSlot);
        droppedItem.transform.localPosition = Vector3.zero;
        dropParticleEffect.Play();
    }

    //Responsible for handling interactions with the furniture.
    public virtual bool Interact() { return false; }

    public virtual void InteractAnimation(PlayerController player) { }
}
