using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractController : MonoBehaviour {

    private HashSet<Furniture> interactableFurnitures = new HashSet<Furniture>();
    private Furniture closestReachableInteractableFurniture;
    public Furniture ClosestInteractableFurniture { get { return closestReachableInteractableFurniture; } }

    private void FixedUpdate() {
        GetClosestInteractable();
    }

    private void GetClosestInteractable() {
        if (interactableFurnitures.Count == 0) {
            closestReachableInteractableFurniture = null;
            return;
        }

        float minimumDistance = float.MaxValue;
        float distanceBetweenFurniture;
        foreach (var interactableFurniture in interactableFurnitures) {
            distanceBetweenFurniture = Vector3.Distance(interactableFurniture.transform.position, transform.position);

            if (distanceBetweenFurniture < minimumDistance) {
                minimumDistance = distanceBetweenFurniture;
                closestReachableInteractableFurniture = interactableFurniture;
            }
        }
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
