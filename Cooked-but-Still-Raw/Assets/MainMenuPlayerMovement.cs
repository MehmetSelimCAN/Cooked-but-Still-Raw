using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlayerMovement : MonoBehaviour {

    [SerializeField] private Transform initial;
    [SerializeField] private Transform target;
    [SerializeField] private float movementSpeed = 10f;

    private void Awake() {
        transform.LookAt(target);
    }

    private void Update() {
        transform.position = Vector3.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1f) {
            transform.position = initial.position;
            //Transform temp = target;
            //target = initial;
            //initial = temp;
        }
    }
}
