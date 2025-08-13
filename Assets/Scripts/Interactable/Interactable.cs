using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, i_Interactable
{
    private GameObject interactUI;

    private void Start()
    {
        interactUI = GetComponent<GameObject>();

    }
    public void Interact()
    {
        Debug.Log("Hey wassup im a statue");
    }
    public InputType GetRequiredInputType()
    {
        return InputType.None;
    }
}
