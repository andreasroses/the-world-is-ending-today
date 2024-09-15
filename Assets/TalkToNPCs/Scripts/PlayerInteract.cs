using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {


    private void Update() {
        PlayerManager tmp = new();
        if (Input.GetKeyDown(KeyCode.E)) {
            I_Interactable interactable = GetInteractableObject();
            if (interactable != null) {
                interactable.Interact(tmp);
            }
        }
    }

    public I_Interactable GetInteractableObject() {
        List<I_Interactable> interactableList = new List<I_Interactable>();
        float interactRange = 3f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray) {
            if (collider.TryGetComponent(out I_Interactable interactable)) {
                interactableList.Add(interactable);
            }
        }

        I_Interactable closestInteractable = null;
        foreach (I_Interactable interactable in interactableList) {
            if (closestInteractable == null) {
                closestInteractable = interactable;
            } else {
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) < 
                    Vector3.Distance(transform.position, closestInteractable.GetTransform().position)) {
                    // Closer
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;
    }

}