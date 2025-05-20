using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, i_Interactable
{
    private ChestLid lid;

    private void Start()
    {
        lid = GetComponentInChildren<ChestLid>();
    }
    private void Update()
    {
        
    }

    public void Interact()
    {
        Debug.Log("Interacted with chest");

        lid.PlayAnimation();
    }
    public void DetectPlayer()
    {

    }
    public void ShowUI()
    {

    }
    public void HideUI()
    {

    }
}
