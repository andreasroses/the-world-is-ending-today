using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerInteractUI : MonoBehaviour {

    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private ThirdPersonPlayerController playerInteract;
    [SerializeField] private TextMeshProUGUI interactTxt;

    private void Update() {
        if (playerInteract.GetInteractableObject() != null) {
            Show(playerInteract.GetInteractableObject());
        } else {
            Hide();
        }
    }

    private void Show(I_Interactable interactable) {
        containerGameObject.SetActive(true);
        interactTxt.text = interactable.GetInteractText();
    }

    private void Hide() {
        containerGameObject.SetActive(false);
    }

}