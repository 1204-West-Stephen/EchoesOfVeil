using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour, i_Interactable
{
    public void Interact() 
    {
        Debug.Log("Interacted");
    }
}
