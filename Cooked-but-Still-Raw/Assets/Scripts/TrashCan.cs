public class TrashCan : Furniture {

    //Everything can be thrown into the trash can except the dirty plates.
    public override bool CanSetItemOnTop(Item droppedItem) {
        if (droppedItem is DirtyPlateStack) return false;

        return true;
    }

    //Call the destruction behaviour of the dropped item.
    public override void SetItemOnTop(Item droppedItem) {
        droppedItem.ThrowInTheGarbage();
    }
}
