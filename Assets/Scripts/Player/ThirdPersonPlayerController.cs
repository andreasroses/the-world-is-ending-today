using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayerController : MonoBehaviour
{
    [SerializeField] private PlayerManager manager;
    [SerializeField] private Transform mainCam;
    [SerializeField] private CharacterController cc;
    [SerializeField] private Transform player;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform model;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Animator anim;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement();
        if (Input.GetKeyDown(KeyCode.E)) {
            I_Interactable interactable = GetInteractableObject();
            if (interactable != null) {
                interactable.Interact(manager);
            }
        }
    }

    private void playerMovement(){
        Vector3 viewDir = player.position - new Vector3(mainCam.position.x, player.position.y, mainCam.position.z);
        orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveInput = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(moveInput != Vector3.zero){
            anim.SetBool("isWalking", true);
            model.forward = Vector3.Slerp(model.forward, moveInput.normalized, Time.deltaTime * rotationSpeed);
            cc.SimpleMove(moveInput * moveSpeed);
        }
        else{
            anim.SetBool("isWalking", false);
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
