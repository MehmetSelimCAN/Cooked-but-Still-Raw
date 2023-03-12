using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractController : Singleton<InteractController> {

    private HashSet<Furniture> interactableFurnitures = new HashSet<Furniture>();
    private Furniture closestReachableInteractableFurniture;
    public Furniture ClosestInteractableFurniture { get { return closestReachableInteractableFurniture; } }

    public event EventHandler<OnClosestFurnitureChangedEventArgs> OnClosestFurnitureChanged;
    public class OnClosestFurnitureChangedEventArgs : EventArgs {
        public Furniture closestFurniture;
    }

    private void FixedUpdate() {
        GetClosestInteractable();
    }

    //Determines the closest furniture in our range.
    private void GetClosestInteractable() {
        if (interactableFurnitures.Count == 0) {
            SetClosestFurniture(null);
            return;
        }

        float minimumDistance = float.MaxValue;
        float distanceBetweenFurniture;
        foreach (var interactableFurniture in interactableFurnitures) {
            distanceBetweenFurniture = Vector3.Distance(interactableFurniture.transform.position, transform.position);

            if (distanceBetweenFurniture < minimumDistance) {
                minimumDistance = distanceBetweenFurniture;
                if (closestReachableInteractableFurniture != interactableFurniture) {
                    SetClosestFurniture(interactableFurniture);
                }
            }
        }
    }

    //Updates the reference closest furniture.
    private void SetClosestFurniture(Furniture closestFurniture) {
        closestReachableInteractableFurniture = closestFurniture;

        OnClosestFurnitureChanged?.Invoke(this, new OnClosestFurnitureChangedEventArgs { 
            closestFurniture = closestReachableInteractableFurniture
        });
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Furniture")) {
            interactableFurnitures.Add(other.GetComponent<Furniture>());
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Furniture")) {
            interactableFurnitures.Remove(other.GetComponent<Furniture>());
        }
    }

}
