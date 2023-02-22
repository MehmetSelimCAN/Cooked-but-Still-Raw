using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractController : MonoBehaviour {

    public static InteractController Instance { get; private set; }

    private HashSet<Furniture> interactableFurnitures = new HashSet<Furniture>();
    private Furniture closestReachableInteractableFurniture;
    public Furniture ClosestInteractableFurniture { get { return closestReachableInteractableFurniture; } }

    public event EventHandler<OnClosestFurnitureChangedEventArgs> OnClosestFurnitureChanged;
    public class OnClosestFurnitureChangedEventArgs : EventArgs {
        public Furniture closestFurniture;
    }

    private void Awake() {
        Instance = this;
    }

    private void FixedUpdate() {
        GetClosestInteractable();
    }

    private void GetClosestInteractable() {
        if (interactableFurnitures.Count == 0) {
            //closestReachableInteractableFurniture = null;
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
                    //closestReachableInteractableFurniture = interactableFurniture;
                }
            }
        }
    }

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
