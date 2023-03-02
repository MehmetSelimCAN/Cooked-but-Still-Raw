using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour {

    [SerializeField] private Furniture furniture;

    private void Start() {
        InteractController.Instance.OnClosestFurnitureChanged += Player_OnClosestFurnitureChanged;
        Hide();
    }

    private void Player_OnClosestFurnitureChanged(object sender, InteractController.OnClosestFurnitureChangedEventArgs e) {
        if (e.closestFurniture == furniture) {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
