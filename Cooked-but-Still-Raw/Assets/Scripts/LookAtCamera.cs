using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    private Camera mainCamera;

    private void Awake() {
        mainCamera = Camera.main;
    }

    private void LateUpdate() {
        transform.forward = mainCamera.transform.forward;
    }
}
