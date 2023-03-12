using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {

    [SerializeField] protected Transform ingredientUI_Icons;

    public virtual void ShowUI() {
        ingredientUI_Icons.gameObject.SetActive(true);
    }

    public virtual void HideUI() {
        ingredientUI_Icons.gameObject.SetActive(false);
    }

    public virtual void ThrowInTheGarbage() { }

}
