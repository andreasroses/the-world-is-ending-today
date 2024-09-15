using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Interactable
{
    public void Interact(PlayerManager player);

    public Transform GetTransform();

    public string GetInteractText();
}

public interface I_DialogueInitiator
{
    public void StartDialogue(PlayerManager player);
}

public interface I_Pickup
{
    public void StoreItem(PlayerManager player);
}
