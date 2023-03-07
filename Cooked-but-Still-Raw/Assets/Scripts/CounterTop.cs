public class CounterTop : Furniture {

    public override bool CanSetItemOnTop(Item droppedItem) {
        if (itemOnTop == null) return true;

        //If item on top is Dish, checks if the item can be added to the Dish.
        if (itemOnTop is Dish) {
            Dish dishOnTop = itemOnTop as Dish;
            return dishOnTop.CanAddIngredient(droppedItem);
        }

        return false;
    }

    public override void SetItemOnTop(Item droppedItem) {
        //If there is no item on top, it sets the item on top and handles its position.
        if (itemOnTop == null) {
            HandleDroppedItemPosition(droppedItem);
            itemOnTop = droppedItem;
        }
        //If there is already a dish on top, add the ingredient into the dish.
        else {
            Dish dishOnTop = itemOnTop as Dish;
            Ingredient droppedIngredient = droppedItem as Ingredient;
            dishOnTop.AddIngredient(droppedIngredient);
        }
    }
}
