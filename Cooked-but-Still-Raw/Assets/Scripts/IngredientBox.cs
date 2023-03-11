public class IngredientBox : Furniture {

    //Provide the ingredient instance on request.
    public override Item GetItemOnTop() {
        /*
        var ingredient = Instantiate(itemOnTop, itemSlot);
        */
        var ingredient = PoolingManager.Instance.ActivateFromPool(itemOnTop);
        return ingredient;
    }

    //Item on top will never be null.
    public override void ClearItemOnTop() {
        return;
    }
}
