using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour, i_Interactable
{
    public bool journalAcquired = false;

    public void Interact()
    {
        Destroy(gameObject);
        journalAcquired = true;
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
